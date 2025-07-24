using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using HarmonyLib.Tools;
using Archipelago.MultiClient.Net.Models;
using Newtonsoft.Json;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine.UI;
using Stacklands_Randomizer_Mod.GUI;
using static UnityEngine.InputSystem.InputRemoting;

namespace Stacklands_Randomizer_Mod
{
    public class StacklandsRandomizer : Mod
    {
        #region Private members

        // Static Member(s)
        private static readonly string EXPECTED_APWORLD_VERSION = "0.1.7";
        private static readonly string GAME_NAME = "Stacklands";
        private static readonly string QUEST_COMPLETE_LABEL = "label_quest_completed";

        // Slot Data Tags
        private static readonly string TAG_DEATHLINK = "death_link";

        public static StacklandsRandomizer instance;
        public ModLogger ModLogger => this.Logger;

        // Queue(s)
        private readonly Queue<Action> _actionQueue = new();
        private readonly Queue<Action> _itemQueue = new();
        private readonly Queue<Action> _deathlinkQueue = new();

        // Session Data(s)
        private ArchipelagoSession _session;
        private DeathLinkService _deathlinkService;
        private Dictionary<string, object> _slotData;

        // Lock(s)
        private readonly object _lock = new object();

        // Config(s)
        private ConfigEntry<string> host;
        private ConfigEntry<string> slotName;
        private ConfigEntry<string> password;
        private ConfigEntry<bool> attemptConnection;
        private ConfigEntry<bool> sendGoal;
        private ConfigEntry<bool> deathlink;
        private ConfigEntry<bool> hideCheckedQuests;

        // Other(s)
        private CustomButton _connectionStatus;
        //private SokScreen _connectionStatus;
        private bool _initialized = false;
        private string initialConnectionReason = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// English localization set to help translate quest IDs to english names for location checks.
        /// </summary>
        public LoadedLocSet EnglishLocSet = new LoadedLocSet(LocResources.Default, "English");

        /// <summary>
        /// Gets or sets whether a DeathLink trigger is currently being handled.
        /// </summary>
        public bool HandlingDeathLink
        {
            get { return _handlingDeathLink; }
            set
            {
                if (_handlingDeathLink != value)
                {
                    lock (_lock)
                    {
                        _handlingDeathLink = value;
                    }
                }
            }
        }
        private bool _handlingDeathLink = false;

        /// <summary>
        /// Gets whether or not deathlink is enabled.
        /// </summary>
        public bool DeathlinkEnabled
            => deathlink.Value &&
               _deathlinkService is not null;

        /// <summary>
        /// Check if currently connected to an archipelago server.
        /// </summary>
        public bool IsConnected =>
            _session is not null
            && _session.Socket is not null
            && _session.Socket.Connected;


        /// <summary>
        /// Whether or not we are currently in a game.
        /// </summary>
        public bool IsInGame =>
            WorldManager.instance.CurrentGameState is WorldManager.GameState.Playing or WorldManager.GameState.Paused;

        /// <summary>
        /// Gets whether or not to hide completed quests from the quest log.
        /// </summary>
        public bool HideCompletedQuests
            => hideCheckedQuests.Value;

        /// <summary>
        /// Gets or sets the YAML Options for this run.
        /// </summary>
        public YamlOptions Options { get; private set; }

        /// <summary>
        /// Get the player name for the current world.
        /// </summary>
        public string PlayerName =>
            IsConnected
                ? GetPlayerName(_session.ConnectionInfo.Slot)
                : string.Empty;

        /// <summary>
        /// Get the current room seed.
        /// </summary>
        public string Seed =>
            IsConnected
                ? instance._session.RoomState.Seed
                : string.Empty;

        #endregion

        #region Unity Methods

        /// <summary>
        /// Called first
        /// </summary>
        public void Awake()
        {
            instance = this;

            // Extend Location enum (need to do this early so the mod can load the
            // resource_booster.json correctly
            EnumHelper.ExtendEnum<Location>("Archipelago");

            // Prepare mod config
            host = Config.GetEntry<string>("Server", "archipelago.gg:12345");
            slotName = Config.GetEntry<string>("Slot Name", "Slot");
            password = Config.GetEntry<string>("Password", "");
            
            // Hide 'Attempt Connect' toggle to replace with a button
            attemptConnection = Config.GetEntry<bool>("Attempt Connect", false);
            attemptConnection.UI.Hidden = true;

            // Hide 'Send Goal' toggle to replace with a button and set up event to create all buttons
            sendGoal = Config.GetEntry<bool>("Send Goal", false);
            sendGoal.UI.Hidden = true;

            // Set up Deathlink toggle
            deathlink = Config.GetEntry<bool>("Deathlink", false);
            deathlink.UI.Tooltip = "Send deathlink when a villager dies and kill a villager when deathlink received";
            deathlink.OnChanged = (bool newValue) =>
            {
                ModLogger.Log($"Deathlink changed to: {newValue}");

                if (_deathlinkService is not null)
                {
                    // Enable / disable deathlink based on new value
                    if (newValue) { _deathlinkService.EnableDeathLink(); }
                    else { _deathlinkService.DisableDeathLink(); }
                }
            };

            // Set up hide checked quests toggle
            hideCheckedQuests = Config.GetEntry<bool>("Hide Checked Quests", false);
            hideCheckedQuests.UI.Tooltip = "Hide completed quests from the in-game quest log";

            sendGoal.UI.OnUI = (ConfigEntryBase entry) =>
            {
                CustomButton connectButton = Instantiate(PrefabManager.instance.ButtonPrefab, ModOptionsScreen.instance.ButtonsParent);
                connectButton.transform.localScale = Vector3.one;
                connectButton.transform.localPosition = Vector3.zero;
                connectButton.transform.localRotation = Quaternion.identity;

                connectButton.TextMeshPro.text = "Connect (game will restart!)";
                connectButton.TooltipText = "Attempt to connect to the archipelago server.";
                connectButton.Clicked += ConnectButton_Clicked;

                CustomButton disconnectButton = Instantiate(PrefabManager.instance.ButtonPrefab, ModOptionsScreen.instance.ButtonsParent);
                disconnectButton.transform.localScale = Vector3.one;
                disconnectButton.transform.localPosition = Vector3.zero;
                disconnectButton.transform.localRotation = Quaternion.identity;

                disconnectButton.TextMeshPro.text = "Disconnect";
                disconnectButton.TooltipText = "Disconnect from the Archipelago server";
                disconnectButton.Clicked += DisconnectButton_Clicked;

                CustomButton sendGoalButton = Instantiate(PrefabManager.instance.ButtonPrefab, ModOptionsScreen.instance.ButtonsParent);
                sendGoalButton.transform.localScale = Vector3.one;
                sendGoalButton.transform.localPosition = Vector3.zero;
                sendGoalButton.transform.localRotation = Quaternion.identity;

                sendGoalButton.TextMeshPro.text = "Send Goal";
                sendGoalButton.TooltipText = "Send victory condition, if completed. (TEMPORARY SOLUTION)";
                sendGoalButton.Clicked += SendGoalButton_Clicked;
            };

            

            // If 'Attempt Connect' value is true, attempt to connect to the AP server
            if (attemptConnection.Value)
            {
                // Change 'Attempt Connect' value back to false to prevent a re-attempt on next game load
                attemptConnection.Value = false;
                Config.Save();

                // Attempt to connect and log in
                (bool success, string reason) = ConnectAndLogin(host.Value, slotName.Value, password.Value.Length > 0 ? password.Value : null);

                ModLogger.Log($"Connected and Logged In: {success}");
                ModLogger.Log($"Reason: {reason}");

                // Check for success
                if (success)
                {
                    // For some reason, trying to connect in any method other than Awake() causes the game to completely freeze.
                    // I haven't figured out why yet, so to get around this, the OP quick-restarts the game to force Awake() to call again.

                    // If 'Send Goal' is set to true, send the goal
                    if (sendGoal.Value)
                    {
                        // Send goal completion
                        SendGoalCompletionAsync();

                        sendGoal.Value = false;
                        Config.Save();
                    }
                }
                else
                {
                    // Set initial connection reason
                    initialConnectionReason = reason;

                    // Disconnect and clear
                    Disconnect(reason);
                }
            }

            // Apply patches only if successfully connected
            if (IsConnected)
            {
                ModLogger.Log($"Applying patches...");

                // Apply patches
                HarmonyFileLog.Enabled = true;
                Harmony = new Harmony("Stacklands_AP");
                Harmony.PatchAll();

                // Update the pause game quest
                UpdatePauseGameQuest();
            }
            else
            {
                ModLogger.Log($"Patches not applied due to no archipelago connection.");
            }
        }

        /// <summary>
        /// Called after Awake()
        /// </summary>
        public void Start()
        {
            ModLogger.Log($"Start...");

            // Set connection status
            SetConnectionStatus(IsConnected, initialConnectionReason);
        }

        /// <summary>
        /// Mod is ready
        /// </summary>
        public override void Ready()
        {
            ModLogger.Log("Ready...");

            // Prevent being called multiple times
            if (!_initialized)
            {
                _initialized = true;

                if (GUIManager.instance is null)
                {
                    new GameObject("GUIManager").AddComponent<GUIManager>();
                }

                SokLoc.instance.LanguageChanged += SokLoc_LanguageChanged;

                // Hook into scene loaded event
                SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
                {
                    ModLogger.Log($"Scene '{scene.name}' loaded!");

                    // Create connection status element
                    GUIManager.instance.CreateConnectionStatusElement();
                };
            }
        }

        /// <summary>
        /// Called once every frame.
        /// </summary>
        public void Update()
        {
            // Prepare to connect if told to attempt to connect or if told to send goal
            if (attemptConnection.Value && ((_session is null || !_session.Socket.Connected) || sendGoal.Value))
            {
                PrepareToConnect();
            }

            // Test triggers for use during development
            if (InputController.instance.GetKeyDown(Key.F5))
            {
                // ItemHandler.SpawnBoosterPack(ModBoosterPacks.resource_booster);
            }
            else if (InputController.instance.GetKeyDown(Key.F6))
            {
                
            }
            else if (InputController.instance.GetKeyDown(Key.F7))
            {
                // ItemHandler.SpawnStack(Cards.gold, 25);
            }
            else if (InputController.instance.GetKeyDown(Key.F8))
            {
                // SimulateUnlockBooster();
            }
            else if (InputController.instance.GetKeyDown(Key.F9))
            {
                
            }
            else if (InputController.instance.GetKeyDown(Key.F10))
            {
                
            }
            else if (InputController.instance.GetKeyDown(Key.F11))
            {
                
            }
            else if (InputController.instance.GetKeyDown(Key.F12))
            {
                
            }

            // Handle next queue actions, if any
            ProcessNextInQueue();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disconnect from the archipelago server.
        /// </summary>
        public void Disconnect(string? reason = null)
        {
            ModLogger.LogWarning($"Disconnecting from archipelago server. Reason: {reason}");

            if (_session is not null && _session.Socket is not null)
            {
                _session.Socket.DisconnectAsync();
            }

            _session = null;

            // Set connection status
            SetConnectionStatus(IsConnected, reason);
        }

        /// <summary>
        /// Display an in-game notification to the player.
        /// </summary>
        /// <param name="title">The title for the notification.</param>
        /// <param name="message">The message text for the notification.</param>
        public void DisplayNotification(string title, string message)
        {
            // Only display if in-game
            if (WorldManager.instance.CurrentGameState is not WorldManager.GameState.InMenu or WorldManager.GameState.GameOver)
            {
                ModLogger.Log($"Displaying notification...");

                GameScreen.instance.AddNotification(title, message);
            }
        }
        
        /// <summary>
        /// Prepare the game to attempt a connection to the Archipelago server.
        /// </summary>
        public void PrepareToConnect()
        {
            // Disconnect current session
            Disconnect();

            // Restart application (and skip the intro)
            System.Diagnostics.Process.Start(Application.dataPath.Replace("_Data", ".exe"), "--no-intro");
            Application.Quit();

            // Wait for main thread queue to finish if currently in a game
            lock (_lock)
            {
                if (WorldManager.instance.CurrentGameState != WorldManager.GameState.InMenu)
                {
                    // Complete all remaining items in queues, if any
                    ProcessAllInQueue();
                }
            }
        }

        /// <summary>
        /// Send all completed locations to the server.
        /// </summary>
        public async Task SendAllCompletedLocations()
        {
            // Get all completed quests 
            foreach (string questId in WorldManager.instance.CurrentSave.CompletedAchievementIds)
            {
                // Get the completed quest
                Quest quest = QuestManager.GetAllQuests()
                    .Find(q => q.Id == questId);

                // Add to queue (do not display notifications)
                await AsyncQueue.Enqueue(() => SendCompletedLocation(quest));
            }
        }

        /// <summary>
        /// Send a completed quest to the archipelago server as a checked location.
        /// </summary>
        /// <param name="quest">The quest to complete.</param>
        /// <param name="notify">Whether or not a notification should be displayed.</param>
        public async Task SendCompletedLocation(Quest quest, bool notify = false)
        {
            ModLogger.Log($"Attempting to send completed location with Desc: '{quest.DescriptionTerm}' and DescOver: '{quest.DescriptionTerm}'");

            // Get english description (as these are used in the apworld)
            string description = !string.IsNullOrWhiteSpace(quest.DescriptionTermOverride)
                ? quest.RequiredCount != -1
                    ? EnglishLocSet.TranslateTerm(quest.DescriptionTermOverride, LocParam.Create("count", quest.RequiredCount.ToString()))
                    : EnglishLocSet.TranslateTerm(quest.DescriptionTermOverride)
                : EnglishLocSet.TranslateTerm(quest.DescriptionTerm);

            ModLogger.Log($"Processing completed quest: '{description}' as a location check...");

            ScoutedItemInfo location = null;

            try
            {
                // Check if location exists as a check
                long locationId = _session.Locations.GetLocationIdFromName(GAME_NAME, description);
                Dictionary<long, ScoutedItemInfo> locations = await _session.Locations.ScoutLocationsAsync(locationId);

                // Check if location has been returned
                if (locations.TryGetValue(locationId, out location))
                {
                    ModLogger.Log($"Location Check found with ID: {locationId}");
                }
                else
                {
                    ModLogger.Log($"Location '{quest.Description}' does not appear to be a location check.");
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"Error when attempting to find '{quest.Description}' as a location check.. Reason: '{ex.Message}'.");
            }

            // Did this quest exist as a location?
            if (location != null)
            {
                // Has it already been checked?
                if (!_session.Locations.AllLocationsChecked.Contains(location.LocationId))
                {
                    ModLogger.Log($"Sending location check completion...");

                    // Send completed location
                    await _session.Locations.CompleteLocationChecksAsync(location.LocationId);
                }
                else
                {
                    ModLogger.Log($"This location has already been checked.");

                    // Do not notify
                    notify = false;
                }
            }

            // Should we display a notification?
            if (notify) 
            {
                string title = $"{SokLoc.Translate(QUEST_COMPLETE_LABEL)} ";
                string message = string.Empty;

                // Was it a location check?
                if (location != null)
                {
                    // Is the receiver of the check this player?
                    if (location.IsReceiverRelatedToActivePlayer)
                    {
                        message = $"You found your {location.ItemName}\n({location.LocationName})";
                    }
                    // Is the receiver another player?
                    else
                    {
                        message = $"You sent {location.ItemName} to {location.Player.Name}\n({location.LocationName})";
                    }
                }
                // If it wasn't...
                else
                {
                    // Are all goals completed?
                    if (CheckGoalCompleted())
                    {
                        title = $"Goal Completed!";
                        message = $"Congratulations, you have completed your goal! Please go to the Mods menu and click 'Send Goal' to complete your run.";
                    }
                    else
                    {
                        message = $"{quest.Description}";
                    }
                }

                // Display the notification
                DisplayNotification(title, message);
            }
        }

        /// <summary>
        /// Send a DeathLink trigger to the server.
        /// </summary>
        /// <param name="cause"></param>
        public void SendDeathlink(string combatable, string? cause = null)
        {
            // If deathlink is enabled, send trigger
            if (IsConnected && DeathlinkEnabled)
            {
                ModLogger.Log("Sending Deathlink trigger to server...");

                // Send the deathlink trigger
                _deathlinkService.SendDeathLink(new DeathLink(PlayerName, cause));

                // Display an in-game notification
                DisplayNotification(
                    $"Your {combatable} Died",
                    "As a consequence, you have sent a DeathLink trigger to your team.");
            }
        }

        

        /// <summary>
        /// Sync all received items from the server and spawn them if necessary.
        /// </summary>
        /// <param name="forceCreate">Whether or not to force creation of all items.</param>
        public void SyncAllReceivedItems(bool forceCreate)
        {
            ModLogger.Log($"Performing re-sync of all unlocked items from server...");

            ModLogger.Log($"Total starting items: {Options.StartInventory.Count}");

            // Add starting inventory to queue (if any)
            if (Options.StartInventory.Count > 0)
            {
                ItemHandler.SyncItems(Options.StartInventory.Keys, forceCreate);
            }

            ModLogger.Log($"Total received items: {_session.Items.AllItemsReceived.Count}");

            // Add all received items from server (if any)
            if (_session.Items.AllItemsReceived.Count > 0)
            {
                ItemHandler.SyncItems(_session.Items.AllItemsReceived, forceCreate);
            }
        }

        /// <summary>
        /// Set the connection status
        /// </summary>
        /// <param name="connected"></param>
        public void SetConnectionStatus(bool connected, string reason = null)
        {
            ModLogger.Log($"Attempting to update connection status to {connected} with reason: {reason}");

            // Show notification if in-game
            if (WorldManager.instance is not null && WorldManager.instance.CurrentGameState is not WorldManager.GameState.InMenu)
            {
                ModLogger.Log($"Sending notification of connection status...");

                string message = string.Format(
                    "{0} the archipelago server.{1}",
                    connected ? "Connected to" : "Disconnected from",
                    connected ? "" : "\nPlease re-connect to the server manually using the Mods menu."
                );

                // Display message to user
                DisplayNotification(string.Empty, message);
            }

            // Set connection status of the UI element
            if (GUIManager.instance is not null)
            {
                GUIManager.instance.SetConnectionStatus(connected, reason);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Triggered when the 'Connect' button is clicked from the Mods menu.
        /// </summary>
        private void ConnectButton_Clicked()
        {
            ModLogger.Log($"Server: {host.Value}");
            ModLogger.Log($"Slot: {slotName.Value}");
            ModLogger.Log($"Password: {password.Value}");

            // Set 'Attempt Connection' value to true to trigger a connection attempt
            attemptConnection.Value = true;
            Config.Save();
        }

        /// <summary>
        /// Triggered when a DeathLink is received from the session.
        /// </summary>
        /// <param name="deathlink">The <see cref="DeathLink"/> that was received.</param>
        private void DeathLink_DeathLinkReceived(DeathLink deathlink)
        {
            AddToDeathlinkQueue(() => StartCoroutine(HandleDeathLink(deathlink)));
        }

        /// <summary>
        /// Triggered when the 'Connect' button is clicked from the Mods menu.
        /// </summary>
        private void DisconnectButton_Clicked()
        {
            // Set 'Attempt Connection' value to false, just to be safe
            attemptConnection.Value = false;
            Config.Save();

            // Disconnect
            Disconnect();
        }

        /// <summary>
        /// Triggered when an items received event is fired by the session.
        /// </summary>
        /// <param name="itemsHelper">The <see cref="ReceivedItemsHelper"/> from the event.</param>
        private void Items_ItemReceived(ReceivedItemsHelper itemsHelper)
        {
            ModLogger.Log($"Item received! Adding to queue...");
            AddToItemQueue(() => ItemHandler.ReceiveItem(itemsHelper.DequeueItem()));
        }

        /// <summary>
        /// Triggered when the 'Send Goal' button is clicked from the Mods menu.
        /// </summary>
        private void SendGoalButton_Clicked()
        {
            // Check that all goals are completed
            if (CheckGoalCompleted())
            {
                ModLogger.Log("Goal has been reached, preparing to send goal update...");

                attemptConnection.Value = true;
                sendGoal.Value = true;

                Config.Save();
            }
            else
            {
                ModLogger.Log("Goal quest(s) have not yet been completed.");
            }
        }

        /// <summary>
        /// Triggered when the session socket closes.
        /// </summary>
        /// <param name="packet">The <see cref="ArchipelagoPacketBase"/> received from the socket.</param>
        private void Socket_ErrorReceived(Exception e, string message)
        {
            // If message is empty it's usually because we've purposely disconnected
            if (!string.IsNullOrEmpty(message))
            {
                AddToActionQueue(() => ModLogger.Log("Socket error received"));
                AddToActionQueue(() => Disconnect($"Socket Error: {message}"));
            }
        }

        /// <summary>
        /// Triggered when the session socket closes.
        /// </summary>
        /// <param name="packet">The <see cref="ArchipelagoPacketBase"/> received from the socket.</param>
        private void Socket_SocketClosed(string reason)
        {  
            // If a reason is provided, it's probably because it was unexpected.
            if (!string.IsNullOrEmpty(reason))
            {
                AddToActionQueue(() => ModLogger.Log("Socket closed received"));
                AddToActionQueue(() => Disconnect(reason));
            }
        }

        /// <summary>
        /// Triggered when the language is changed.
        /// </summary>
        private void SokLoc_LanguageChanged()
        {
            AddToActionQueue(() => GUIManager.instance.RefreshConnectionStatus());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add an action to the action queue.
        /// </summary>
        /// <param name="action">The action to be added to the queue.</param>
        private void AddToActionQueue(Action action)
        {
            lock (_lock)
            {
                _actionQueue.Enqueue(action);
            }
        }

        /// <summary>
        /// Add an item action to the deathlink queue.
        /// </summary>
        /// <param name="action">The action to be added to the queue.</param>
        private void AddToDeathlinkQueue(Action action)
        {
            lock (_lock)
            {
                _deathlinkQueue.Enqueue(action);
            }
        }

        /// <summary>
        /// Add a death action to the item queue.
        /// </summary>
        /// <param name="action">The action to be added to the queue.</param>
        private void AddToItemQueue(Action action)
        {
            lock (_lock)
            {
                _itemQueue.Enqueue(action);
            }
        }

        /// <summary>
        /// Attempt to connect to an archipelago session and log in to a slot.
        /// </summary>
        /// <param name="host">The full host address, including the port number if required.</param>
        /// <param name="slotName">The name of the slot to join as.</param>
        /// <param name="password">The password for the session, if required.</param>
        /// <returns></returns>
        private (bool, string) ConnectAndLogin(string host, string slotName, string? password)
        {
            bool success = false;
            string reason = string.Empty;

            if (CreateSession(host))
            {
                ModLogger.Log("Attempting to log in...");

                (bool loginSuccess, string loginReason) = Login(slotName, password);

                if (loginSuccess)
                {
                    ModLogger.Log("Logged in successfully!");

                    // Get and parse options from YAML
                    Options = new YamlOptions(_slotData);

                    // Set return variables
                    success = EXPECTED_APWORLD_VERSION.Equals(Options.Version);
                    reason = success ? string.Empty : $".apworld version {Options.Version} does not match expected version '{EXPECTED_APWORLD_VERSION}'";
                }
                else
                {
                    reason = loginReason;
                }
            }
            else
            {
                reason = "Failed to create archipelago session.";
            }

            return (success, reason);
        }

        /// <summary>
        /// Check if the goal has been completed.
        /// </summary>
        /// <returns>True if completed, false if not.</returns>
        private bool CheckGoalCompleted()
        {
            return Options.GoalQuests.All(gq => QuestManager.instance.QuestIsComplete(gq));
        }
        

        /// <summary>
        /// Create a session for the archipelago connection.
        /// </summary>
        /// <param name="host">The host name, including port if required.</param>
        /// <returns><see cref="true"/> on success, <see cref="false"/> on failure.</returns>
        private bool CreateSession(string host)
        {
            ModLogger.Log("Attempting to create session...");

            try
            {
                _session = ArchipelagoSessionFactory.CreateSession(host);

                // Add event handlers
                _session.Socket.SocketClosed += Socket_SocketClosed;
                _session.Socket.ErrorReceived += Socket_ErrorReceived;

                ModLogger.Log("Session created successfully!");

                return true;
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"Unable to create archipelago session. Reason: '{ex.Message}'.");
            }

            return false;
        }

        /// <summary>
        /// Retrieve all items that have been received so far this session.
        /// </summary>
        /// <returns>A list of <see cref="NetworkItem"/> containing all items that have been received this session.</returns>
        private List<ItemInfo> GetAllReceivedItems()
        {
            return _session.Items.AllItemsReceived
                .ToList();
        }

        /// <summary>
        /// Get the name of a player from their player (slot) ID.
        /// </summary>
        /// <param name="playerId">The ID of the player / slot.</param>
        /// <returns>A string containing the player's name.</returns>
        private string GetPlayerName(int playerId)
        {
            return _session.Players.GetPlayerName(playerId);
        }

        /// <summary>
        /// Triggered when a DeathLink trigger is received from the server.
        /// </summary>
        /// <param name="deathLink">The DeathLink trigger that has been received.</param>
        private IEnumerator HandleDeathLink(DeathLink deathLink)
        {
            ModLogger.Log($"DeathLink trigger received!");

            // Wait for current animation to end
            while (!WorldManager.instance.CanInteract) 
            {
                yield return null;
            }

            // Set deathlink received to true if deathlink is enabled, otherwise false
            bool deathlinkEnabled = DeathlinkEnabled;
            lock (_lock)
            {
                _handlingDeathLink = deathlinkEnabled;
            }

            if (!_handlingDeathLink)
            {
                // Bail out if we are not handling DeathLinks (due to them being disabled)
                ModLogger.Log($"Ignoring DeathLink - it is not enabled.");
            }
            else if (!IsInGame)
            {
                // Bail out if we are not currently in-game
                ModLogger.Log($"Ignoring DeathLink - not currently in-game.");
            }
            else
            {
                // Kill a random villager
                KillRandomVillager(deathLink.Source);
            }
        }

        /// <summary>
        /// Find and kill a villager at random.
        /// </summary>
        /// <param name="source">Who / what triggered the killing of the villager.</param>
        private void KillRandomVillager(string source)
        {
            // Get active villagers
            List<Villager> activeVillagers = WorldManager.instance.AllCards
                .Where(c => c.Combatable is Villager && c.isActiveAndEnabled)
                .Select(c => c.Combatable)
                .Cast<Villager>()
                .ToList();

            ModLogger.Log($"Villagers found: {activeVillagers.Count}");

            try
            {
                // If at least one villager exists...
                if (activeVillagers.Any())
                {
                    // If more than one villager, select at random
                    int index = activeVillagers.Count > 1
                        ? UnityEngine.Random.Range(0, activeVillagers.Count)
                        : 0;

                    // Select chosen villager
                    Villager villager = activeVillagers[index];

                    // Kill the villager
                    WorldManager.instance.KillVillager(villager);

                    // Display notification
                    DisplayNotification(
                        $"{villager.Name} killed by {source}",
                        $"Your {villager.Name} has suffered the consequences of DeathLink.");
                }
            }
            catch (Exception ex)
            {
                ModLogger.LogError($"Failed to handle DeathLink. Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Attempt to log in to a slot.
        /// </summary>
        /// <param name="slotName">The name of the slot to log in as.</param>
        /// <param name="password">The password for the session, if required.</param>
        /// <returns></returns>
        private (bool, string) Login(string slotName, string? password = null)
        {
            bool success = false;
            string reason = string.Empty;

            try
            {
                // Attempt to connect to slot in session
                LoginResult result = _session.TryConnectAndLogin(
                    GAME_NAME,
                    slotName,
                    ItemsHandlingFlags.AllItems,
                    password: password);

                ModLogger.Log($"Login attempt success: {result.Successful}");

                if (result.Successful)
                {
                    // Retrieve and store slot data
                    Dictionary<string, object> slotData = ((LoginSuccessful)result).SlotData;
                    lock (_lock)
                    {
                        _slotData = slotData;
                    }

                    ModLogger.Log($"Slot data retrieved with tags: {string.Join(", ", _slotData.Keys)}");

                    // Add event handlers
                    _session.Items.ItemReceived += Items_ItemReceived;

                    try
                    {
                        ModLogger.Log("Attempting to create deathlink service...");

                        // Create and store deathlink service
                        DeathLinkService deathlinkService = _session.CreateDeathLinkService();
                        if (deathlinkService is not null)
                        {
                            ModLogger.Log($"Deathlink service created!");

                            lock (_lock)
                            {
                                // Create and store deathlink service and attach event handler
                                _deathlinkService = deathlinkService;
                                _deathlinkService.OnDeathLinkReceived += DeathLink_DeathLinkReceived;

                                // If deathlink enabled in config, enable it in the service
                                if (DeathlinkEnabled)
                                {
                                    _deathlinkService.EnableDeathLink();
                                }

                            }
                        }
                        else
                        {
                            ModLogger.LogError("DeathLink service was unexpectedly null.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Continue as success, this won't stop the session from running
                        reason = $"Failed to start deathlink service - deathlink will be unavailable.";
                        ModLogger.LogError($"Failed to start deathlink service. Reason: '{ex.Message}'.");
                    }

                    success = true;
                }
                else
                {
                    reason = "Login attempt was unsuccessful.";
                    ModLogger.LogError(result.ToString());
                }
            }
            catch (Exception ex)
            {
                reason = "An error occurred when attempting login.";
                ModLogger.LogError($"Unable to log in to session. Reason: '{ex.Message}',");
            }

            return (success, reason);
        }

        /// <summary>
        /// Provess all remaining items in all queues.
        /// </summary>
        private void ProcessAllInQueue()
        {
            // Handle all remaining items in queue
            while (_itemQueue.TryDequeue(out Action item))
            {
                item.Invoke();
            }

            // Handle all remaining deaths in queue
            while (_deathlinkQueue.TryDequeue(out Action deathlink))
            {
                deathlink.Invoke();
            }

            // Handle all remaining actions in queue
            while (_actionQueue.TryDequeue(out Action action))
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Process the next item in all queues.
        /// </summary>
        private void ProcessNextInQueue()
        {
            // Handle next item in queue
            if (_itemQueue.TryDequeue(out Action item))
            {
                item.Invoke();
            }

            // Handle next death in queue
            if (_deathlinkQueue.TryDequeue(out Action deathlink))
            {
                deathlink.Invoke();
            }

            // Handle next action in queue
            if (_actionQueue.TryDequeue(out Action action))
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Send a goal completion trigger to the server.
        /// </summary>
        private void SendGoalCompletionAsync()
        {
            ModLogger.Log($"Sending goal completion...");

            // Send goal completion
            _session.SetGoalAchieved();

            ModLogger.Log($"Sent goal completion status!");
        }

        /// <summary>
        /// Update the 'Pause Game' quest so that it can be completed before creating a stick.
        /// </summary>
        private void UpdatePauseGameQuest()
        {
            // Update special action to no longer require creating a stick first
            AllQuests.PauseGame = new Quest("pause_game")
            {
                OnSpecialAction = ((string action) => action == "pause_game"),
                QuestGroup = QuestGroup.Starter
            };
        }

        #endregion

        #region Testing Methods

        /// <summary>
        /// Simulate a booster pack spawning.
        /// </summary>
        /// <param name="packId">The ID of the booster pack</param>
        private void SimulateCreateBooster(string packId)
        {
            WorldManager.instance.CreateBoosterpack(
                WorldManager.instance.GetRandomSpawnPosition(),
                packId);
        }

        /// <summary>
        /// Simulate a card spawning.
        /// </summary>
        /// <param name="cardId">The ID of the card.</param>
        private void SimulateCreateCard(string cardId)
        {
            WorldManager.instance.CreateCard(
                WorldManager.instance.GetRandomSpawnPosition(),
                cardId,
                true,
                false,
                true);
        }

        /// <summary>
        /// Simulate a card stack spawning.
        /// </summary>
        /// <param name="cardId">The ID of the card.</param>
        /// <param name="amount">How many of the card to be spawned.</param>
        private void SimulateCreateCard(string cardId, int amount)
        {
            WorldManager.instance.CreateCardStack(
                WorldManager.instance.GetRandomSpawnPosition(),
                amount,
                cardId,
                false);
        }

        private void SimulateUnlockBooster()
        {
            string unfoundBooster = CommonPatchMethods.MAINLAND_PACKS.LastOrDefault(p => !WorldManager.instance.CurrentSave.FoundBoosterIds.Contains(p));

            if (!string.IsNullOrWhiteSpace(unfoundBooster))
            {
                ItemHandler.UnlockBoosterPack(unfoundBooster, true);
            }
        }

        /// <summary>
        /// Simulate a villager dying.
        /// </summary>
        private void SimulateDeath(bool deathLink = false)
        {
            ModLogger.Log($"Simulating villager death...");
            WorldManager.instance.GetCard<Villager>()?.Die();
        }

        /// <summary>
        /// Simulate a villager being killed by a DeathLink trigger.
        /// </summary>
        private void SimulateDeathLinkReceived()
        {
            ModLogger.Log($"Simulating DeathLink received...");

            // Add deathlink action to the queue
            AddToDeathlinkQueue(() => StartCoroutine(HandleDeathLink(new DeathLink("A Test", "Ran a test DeathLink received."))));
        }

        /// <summary>
        /// Simulate a goal completion trigger.
        /// </summary>
        private void SimulateGoalComplete()
        {
            ModLogger.Log($"Simulating goal complete...");

            foreach (Quest quest in Options.GoalQuests)
            {
                // If kill demon quest
                if (quest.Id == AllQuests.KillDemon.Id)
                {
                    // Create a demon
                    WorldManager.instance.CreateCard(
                        WorldManager.instance.GetRandomSpawnPosition(),
                        Cards.demon,
                        true,
                        false,
                        true);

                    // Find the demon and kill it
                    if (FindObjectOfType<Demon>() is Demon demon)
                    {
                        demon.Die();
                    }
                }
                else if (quest.Id == AllQuests.FightWickedWitch.Id)
                {

                    // Create a demon lord
                    WorldManager.instance.CreateCard(
                        WorldManager.instance.GetRandomSpawnPosition(),
                        Cards.wicked_witch,
                        true,
                        false,
                        true);

                    // Find the witch and kill it
                    if (FindObjectOfType<WickedWitch>() is WickedWitch witch)
                    {
                        witch.Die();
                    }
                }
                else
                {
                    ModLogger.LogError($"Unbound goal quest: '{quest.Id}'.");
                }
            }
        }

        /// <summary>
        /// Simulate an item received trigger.
        /// </summary>
        /// <param name="type">The type of item to simulate.</param>
        private void SimulateItemReceived(ItemType type)
        {
            ModLogger.Log($"Simulating {type} item received...");

            // Get all possible item unlocks
            List<Item> items = ItemMapping.Map
                .Where(m => m.ItemType == type)
                .ToList();

            // Select blueprint at random and receive it
            Item item = items.ElementAt(UnityEngine.Random.Range(0, items.Count));
            AddToItemQueue(() => ItemHandler.ReceiveItem(item.Name));
        }

        /// <summary>
        /// Simulate a quest completion.
        /// </summary>
        private void SimulateQuestComplete()
        {
            ModLogger.Log($"Simulating quest completion...");

            // Get a list of all currently incomplete quests (for mainland)
            List<Quest> incompleteQuests = QuestManager.instance.AllQuests
               .Where(q => q.QuestLocation == Location.Mainland && !QuestManager.instance.QuestIsComplete(q))
               .ToList();

            // Select random quest
            Quest quest = incompleteQuests[UnityEngine.Random.Range(0, incompleteQuests.Count)];

            // Complete a random quest from the list
            WorldManager.instance.QuestCompleted(quest);
        }

        /// <summary>
        /// Simulate a specific special action.
        /// </summary>
        private void SimulateSpecialAction(string specialAction)
        {
            ModLogger.Log($"Simulating special action '{specialAction}'...");
            
            // Trigger special action
            QuestManager.instance.SpecialActionComplete(specialAction);
        }

        #endregion
    }
}