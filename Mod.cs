using HarmonyLib;
using UnityEngine;
using UnityEngine.InputSystem;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using HarmonyLib.Tools;
using Archipelago.MultiClient.Net.Models;
using System.Collections.ObjectModel;

namespace Stacklands_Randomizer_Mod
{
    public class StacklandsRandomizer : Mod
    {
        #region Private members

        // Static Member(s)
        private static readonly string GAME_NAME = "Stacklands";
        private static readonly string QUEST_COMPLETE_LABEL = "label_quest_completed";
        private static readonly List<string> BASIC_CARDS = ["berrybush", "rock", "tree"];

        private static readonly string TAG_BASIC_PACK = "BasicPack";
        private static readonly string TAG_DEATHLINK = "DeathLink";
        private static readonly string TAG_GOAL = "Goal";
        private static readonly string TAG_PAUSE_ENABLED = "PauseEnabled";

        public static StacklandsRandomizer instance;

        // Queue(s)
        private readonly Queue<Action> _mainThreadQueue = new();

        // Session Data(s)
        private ArchipelagoSession _session;
        private DeathLinkService _deathlink;
        private Dictionary<string, object> _slotData;

        // Progress tracker(s)
        private Goal _currentGoal;

        // Lock(s)
        private readonly object _lock = new object();

        // Config(s)
        private ConfigEntry<string> host;
        private ConfigEntry<string> slotName;
        private ConfigEntry<string> password;
        private ConfigEntry<bool> attemptConnection;
        private ConfigEntry<bool> sendGoal;

        // Other(s)
        private CustomButton _connectionStatus;

        #endregion

        #region Public Properties

        /// <summary>
        /// Check if DeathLink is enabled.
        /// </summary>
        public bool IsDeathlinkEnabled =>
            _session is not null
            && _deathlink is not null
            && _slotData is not null
            && _slotData.TryGetValue(TAG_DEATHLINK, out object result)
            && result is bool deathlinkEnabled
            && deathlinkEnabled;

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

        public string CurrentSaveId =>
            WorldManager.instance.CurrentSave.SaveId;

        /// <summary>
        /// Check if currently connected to an archipelago server.
        /// </summary>
        public bool IsConnected =>
            _session is not null
            && _session.Socket is not null
            && _session.Socket.Connected;

        /// <summary>
        /// Whether or not pausing is enabled for this run.
        /// </summary>
        public bool IsPauseEnabled { get; private set; }

        /// <summary>
        /// Whether or not we are currently in a game.
        /// </summary>
        public bool IsInGame =>
            WorldManager.instance.CurrentGameState is WorldManager.GameState.Playing or WorldManager.GameState.Paused;

        /// <summary>
        /// Whether or not the run should start with the basic pack unlocked by default.
        /// </summary>
        public bool IsStartWithBasicPack { get; private set; }

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

            // Prepare mod config
            host = Config.GetEntry<string>("Server", "archipelago.gg:12345");
            slotName = Config.GetEntry<string>("Slot Name", "Slot");
            password = Config.GetEntry<string>("Password", "");
            attemptConnection = Config.GetEntry<bool>("Attempt Connect", false);
            sendGoal = Config.GetEntry<bool>("Send Goal", false);

            attemptConnection.UI.Hidden = true;
            sendGoal.UI.Hidden = true;

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
                if (ConnectAndLogin(host.Value, slotName.Value, password.Value.Length > 0 ? password.Value : null))
                {
                    // For some reason, trying to connect in any method other than Awake() causes the game to completely freeze.
                    // I haven't figured out why yet, so to get around this, the OP quick-restarts the game to force Awake() to call again.

                    GoalType goal = _slotData.TryGetValue(TAG_GOAL, out object goalId) ? (GoalType)Convert.ToInt32(goalId) : GoalType.KillDemon;

                    // Apply other settings
                    lock (_lock)
                    {
                        IsPauseEnabled = _slotData.TryGetValue(TAG_PAUSE_ENABLED, out object pause) ? (bool)pause : true;
                        IsStartWithBasicPack = _slotData.TryGetValue(TAG_BASIC_PACK, out object basicPack) ? (bool)basicPack : true;
                        _currentGoal = GoalMapping.Map.SingleOrDefault(g => g.Type == goal);
                    }

                    Debug.Log($"Current goal: {_currentGoal.Name}");

                    // If 'Send Goal' is set to true, send the goal
                    if (sendGoal.Value)
                    {
                        // Send goal goal completion
                        SendGoalCompletionAsync(_currentGoal.Name);

                        sendGoal.Value = false;
                        Config.Save();
                    }
                }
                else
                {
                    // Disconnect and clear
                    Disconnect();
                }
            }

            // Apply patches only if successfully connected
            if (IsConnected)
            {
                Debug.Log($"Applying patches...");

                // Apply patches
                HarmonyFileLog.Enabled = true;
                Harmony = new Harmony("Stacklands_AP");
                Harmony.PatchAll();

                // Update the pause game quest
                UpdatePauseGameQuest();
            }
            else
            {
                Debug.Log($"Patches not applied due to no archipelago connection.");
            }
        }

        /// <summary>
        /// Called after Awake()
        /// </summary>
        public void Start()
        {
            Debug.Log($"Start...");

            // Create connection status in menu
            CreateConnectionStatus();
            UpdateConnectionStatus(IsConnected);
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

            // Test triggers
            if (InputController.instance.GetKeyDown(Key.F5))
            {
                SimulateItemReceived(ItemType.BoosterPack);
            }
            else if (InputController.instance.GetKeyDown(Key.F6))
            {
                SimulateItemReceived(ItemType.Idea);
            }
            else if (InputController.instance.GetKeyDown(Key.F7))
            {
                SimulateItemReceived(ItemType.Resource);
            }
            else if (InputController.instance.GetKeyDown(Key.F8))
            {
                SimulateDeath();
            }
            else if (InputController.instance.GetKeyDown(Key.F9))
            {
                SimulateDeathLinkReceived();
            }
            else if (InputController.instance.GetKeyDown(Key.F10))
            {
                SimulateQuestComplete();
            }
            else if (InputController.instance.GetKeyDown(Key.F11))
            {
                SimulateGoalComplete();
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
            Debug.Log($"Disconnecting. Reason: {reason}");

            if (_session is not null && _session.Socket is not null)
            {
                _session.Socket.DisconnectAsync();
            }

            _session = null;

            // Update connection status
            UpdateConnectionStatus(false);
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
                Debug.Log($"Displaying notification...");

                GameScreen.instance.AddNotification(title, message);
            }
        }

        /// <summary>
        /// Generate the ID of a random, basic card.
        /// </summary>
        /// <returns>A randomly generated card ID of a basic card.</returns>
        public string GetRandomBasicCard()
        {
            return BASIC_CARDS.ElementAt(UnityEngine.Random.Range(0, BASIC_CARDS.Count));
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

                // Send completed location (but do not show notifications)
                await SendCompletedLocation(quest);
            }
        }

        /// <summary>
        /// Send a completed quest to the archipelago server as a checked location.
        /// </summary>
        /// <param name="questDescription">The description of the quest to complete.</param>
        /// <param name="notify">Whether or not a notification should be displayed.</param>
        public async Task SendCompletedLocation(Quest quest, bool notify = false)
        {
            Debug.Log($"Processing completed quest: '{quest.Description}' as a location check...");

            ScoutedItemInfo location = null;

            try
            {
                // Check if location exists as a check
                long locationId = _session.Locations.GetLocationIdFromName(GAME_NAME, quest.Description);
                Dictionary<long, ScoutedItemInfo> locations = await _session.Locations.ScoutLocationsAsync(locationId);

                // Check if location has been returned
                if (locations.TryGetValue(locationId, out location))
                {
                    Debug.Log($"Location Check found with ID: {locationId}");
                }
                else
                {
                    Debug.Log($"Location '{quest.Description}' does not appear to be a location check.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error when attempting to find '{quest.Description}' as a location check.. Reason: '{ex.Message}'.");
            }

            // Did this quest exist as a location?
            if (location != null)
            {
                // Has it already been checked?
                if (!_session.Locations.AllLocationsChecked.Contains(location.LocationId))
                {
                    Debug.Log($"Sending location check completion...");

                    // Send completed location
                    await _session.Locations.CompleteLocationChecksAsync(location.LocationId);
                }
                else
                {
                    Debug.Log($"This location has already been checked.");

                    // Do not notify
                    notify = false;
                }
            }

            // Should we display a notification?
            if (notify) 
            {
                string title = string.Empty;
                string message = string.Empty;

                // Was it a location check?
                if (location != null)
                {
                    // Is this quest the goal?
                    if (location.LocationName.Equals(_currentGoal.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        title = $"Goal Complete: {location.LocationName}";
                        message = "Congratulations! Please go to the Mods menu and click 'Send Goal' to complete your run.";
                    }
                    // Is the receiver of the check this player?
                    else if (location.IsReceiverRelatedToActivePlayer)
                    {
                        title = $"{SokLoc.Translate(QUEST_COMPLETE_LABEL)} ";
                        message = $"You found your {location.ItemName}\n({location.LocationName})";
                    }
                    // Is the receiver another player?
                    else
                    {
                        title = $"{SokLoc.Translate(QUEST_COMPLETE_LABEL)} ";
                        message = $"You sent {location.ItemName} to {location.Player.Name}\n({location.LocationName})";
                    }
                }
                // If it wasn't...
                else
                {
                    title = $"{SokLoc.Translate(QUEST_COMPLETE_LABEL)} ";
                    message = $"{quest.Description}";
                }

                // Display the notification
                DisplayNotification(title, message);
            }
        }

        /// <summary>
        /// Send a DeathLink trigger to the server.
        /// </summary>
        /// <param name="cause"></param>
        public void SendDeathlink(string? cause = null)
        {
            // If deathlink is enabled, send trigger
            if (IsConnected && IsDeathlinkEnabled)
            {
                Debug.Log("Sending Deathlink trigger to server...");

                _deathlink.SendDeathLink(new DeathLink(PlayerName, cause));
                _session.Socket.SendPacket(new SayPacket() { Text = $"One of {PlayerName}'s villager has ceased to be." });
            }
        }

        /// <summary>
        /// Sync all received items from the server and spawn them if necessary.
        /// </summary>
        /// <param name="forceCreate">Whether or not to force creation of all items.</param>
        public void SyncAllServerItems(bool forceCreate)
        {
            Debug.Log($"Performing re-sync of all unlocked items from server...");

            // If starting with basic pack, force humble beginnings unlock
            if (IsStartWithBasicPack)
            {
                AddToQueue(() => ItemHandler.HandleItem("Humble Beginnings Booster Pack", forceCreate));
            }

            // Get all items received from server
            foreach (ItemInfo item in _session.Items.AllItemsReceived)
            {
                AddToQueue(() => ItemHandler.HandleItem(item, forceCreate));
            }
        }

        /// <summary>
        /// Sync all currently logged items in the current save and spawn them if necessary.
        /// </summary>
        /// <param name="forceCreate">Whether or not to force creation of all items.</param>
        public void SyncAllLoggedItems(bool forceCreate)
        {
            Debug.Log($"Performing re-sync of all items logged this session...");

            // If starting with basic pack is enabled, force humble beginnings unlock
            if (IsStartWithBasicPack)
            {
                AddToQueue(() => ItemHandler.HandleItem("Humble Beginnings Booster Pack", forceCreate));
            }

            // Get all items from current save that exist in the mapping (or only unhandled ones if not forced to create)        
            foreach (SerializedKeyValuePair item in WorldManager.instance.SaveExtraKeyValues
                .Where(kvp => ItemMapping.Map.Exists(m => m.Matches(kvp.Key)) && (forceCreate || !Convert.ToBoolean(kvp.Value))))
            {
                AddToQueue(() => ItemHandler.HandleItem(item.Key, forceCreate));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connected"></param>
        public void UpdateConnectionStatus(bool connected)
        {
            Debug.Log($"Attempting to update connection status...");

            // Show notification if in-game
            if (WorldManager.instance is not null && WorldManager.instance.CurrentGameState is not WorldManager.GameState.InMenu)
            {
                Debug.Log($"Sending notification of connection status...");

                DisplayNotification(
                    string.Empty,
                    $"{(connected ? "Connected to" : "Disconnected from")} Archipelago server.");
            }

            // Update text in alert
            if (_connectionStatus is not null)
            {
                Debug.Log($"Updating status in Main Menu...");
                _connectionStatus.TextMeshPro.text = $"Archipelago: {(connected ? "Connected" : "Disconnected")}";
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Triggered when the 'Connect' button is clicked from the Mods menu.
        /// </summary>
        private void ConnectButton_Clicked()
        {
            Debug.Log($"Server: {host.Value}");
            Debug.Log($"Slot: {slotName.Value}");
            Debug.Log($"Password: {password.Value}");

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
            AddToQueue(() => HandleDeathLink(deathlink));
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
            Debug.Log($"Item received! Adding to queue...");
            AddToQueue(() => ItemHandler.HandleItem(itemsHelper.DequeueItem()));
        }

        /// <summary>
        /// Triggered when the session socket closes.
        /// </summary>
        /// <param name="packet">The <see cref="ArchipelagoPacketBase"/> received from the socket.</param>
        private void Socket_SocketClosed(string reason)
        {
            AddToQueue(() => Disconnect(reason));
        }

        /// <summary>
        /// Triggered when the 'Send Goal' button is clicked from the Mods menu.
        /// </summary>
        private void SendGoalButton_Clicked()
        {
            Debug.Log("Completed Achievements:");

            foreach(string completedAchievementId in WorldManager.instance.CurrentSave.CompletedAchievementIds)
            {
                Debug.Log(completedAchievementId);
            }

            // Check if the goal has been completed
            if (WorldManager.instance.CurrentSave.CompletedAchievementIds.Contains(_currentGoal.QuestId))
            {
                Debug.Log("Goal quest completed!");

                attemptConnection.Value = true;
                sendGoal.Value = true;

                Config.Save();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add an action to the main thread queue.
        /// </summary>
        /// <param name="action">The action to be added to the queue.</param>
        private void AddToQueue(Action action)
        {
            lock (_lock)
            {
                _mainThreadQueue.Enqueue(action);
            }
        }

        /// <summary>
        /// Attempt to connect to an archipelago session and log in to a slot.
        /// </summary>
        /// <param name="host">The full host address, including the port number if required.</param>
        /// <param name="slotName">The name of the slot to join as.</param>
        /// <param name="password">The password for the session, if required.</param>
        /// <returns></returns>
        private bool ConnectAndLogin(string host, string slotName, string? password)
        {
            if (CreateSession(host))
            {
                Debug.Log("Attempting to log in...");

                if (Login(slotName, password))
                {
                    Debug.Log("Logged in successfully!");
                    return true;
                }
                else
                {
                    Debug.LogError("Failed to log in to archipelago session.");
                }
            }
            else
            {
                Debug.LogError("Failed to create archipelago session.");
            }

            return false;
        }

        /// <summary>
        /// Create a connection status UI element in the main menu.
        /// </summary>
        private void CreateConnectionStatus()
        {
            if (GameCanvas.instance.GetScreen<MainMenu>() is MainMenu menu)
            {
                Debug.Log($"Creating archipelago connection status UI element...");

                // Add new button to menu
                CustomButton status = Instantiate(PrefabManager.instance.ButtonPrefab, menu.ContinueButton.transform.parent);
                status.transform.SetAsFirstSibling();
                status.transform.localScale = Vector3.one;
                status.transform.localPosition = Vector3.one;
                status.transform.localRotation = Quaternion.identity;
                status.ButtonEnabled = false;

                _connectionStatus = status;
            }
        }

        /// <summary>
        /// Create a session for the archipelago connection.
        /// </summary>
        /// <param name="host">The host name, including port if required.</param>
        /// <returns><see cref="true"/> on success, <see cref="false"/> on failure.</returns>
        private bool CreateSession(string host)
        {
            Debug.Log("Attempting to create session...");

            try
            {
                _session = ArchipelagoSessionFactory.CreateSession(host);

                Debug.Log("Session created successfully!");

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unable to create archipelago session. Reason: '{ex.Message}'.");
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
        private void HandleDeathLink(DeathLink deathLink)
        {
            Debug.Log($"DeathLink trigger received!");

            // Set deathlink received to true if deathlink is enabled, otherwise false
            bool deathlinkEnabled = IsDeathlinkEnabled;
            lock (_lock)
            {
                _handlingDeathLink = deathlinkEnabled;
            }

            // Bail out if we are not handling DeathLinks (due to them being disabled)
            if (!_handlingDeathLink)
            {
                Debug.Log($"Ignoring DeathLink - it is not enabled.");
                return;
            }

            // Bail out if we are not currently in-game
            if (!IsInGame)
            {
                Debug.Log($"Ignoring DeathLink - not currently in-game.");
                return;
            }

            // Kill a random villager
            KillRandomVillager(deathLink.Source);
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

            Debug.Log($"Villagers found: {activeVillagers.Count}");

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
                Debug.LogError($"Failed to handle DeathLink. Reason: '{ex.Message}'.");
            }
        }

        /// <summary>
        /// Attempt to log in to a slot.
        /// </summary>
        /// <param name="slotName">The name of the slot to log in as.</param>
        /// <param name="password">The password for the session, if required.</param>
        /// <returns></returns>
        private bool Login(string slotName, string? password = null)
        {
            try
            {
                // Attempt to connect to slot in session
                LoginResult result = _session.TryConnectAndLogin(
                    GAME_NAME,
                    slotName,
                    ItemsHandlingFlags.AllItems,
                    password: password,
                    tags: [TAG_DEATHLINK]);

                Debug.Log($"Login attempt success: {result.Successful}");

                if (result.Successful)
                {
                    // Retrieve and store slot data
                    Dictionary<string, object> slotData = ((LoginSuccessful)result).SlotData;
                    lock (_lock)
                    {
                        _slotData = slotData;
                    }

                    Debug.Log($"Slot data retrieved with tags: {string.Join(", ", _slotData.Keys)}");

                    // Add event handlers
                    _session.Items.ItemReceived += Items_ItemReceived;
                    //_session.Socket.PacketReceived += Socket_PacketReceived;
                    _session.Socket.SocketClosed += Socket_SocketClosed;

                    try
                    {
                        bool deathlinkTag = _slotData.TryGetValue(TAG_DEATHLINK, out object deathlink) ? (bool)deathlink : false;

                        // If deathlink is enabled for this slot, attempt to create deathlink service
                        if (deathlinkTag)
                        {
                            Debug.Log("Attempting to create deathlink service...");

                            // Create and store deathlink service
                            DeathLinkService deathlinkService = _session.CreateDeathLinkService();
                            if (deathlinkService is not null)
                            {
                                Debug.Log($"Deathlink service created: {deathlinkService != null}");

                                lock (_lock)
                                {
                                    // Create and store deathlink service and attach event handler
                                    _deathlink = deathlinkService;
                                    _deathlink.OnDeathLinkReceived += DeathLink_DeathLinkReceived;

                                    _deathlink.EnableDeathLink();
                                }
                            }
                            else
                            {
                                Debug.LogError("DeathLink service was unexpectedly null.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to start deathlink service. Reason: '{ex.Message}'.");
                    }

                    return true;
                }

                Debug.LogError(result);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unable to log in to session. Reason: '{ex.Message}',");
            }

            return false;
        }

        /// <summary>
        /// Provess all remaining items in all queues.
        /// </summary>
        private void ProcessAllInQueue()
        {
            // Handle all remaining actions in queue
            while (_mainThreadQueue.TryDequeue(out Action item))
            {
                item.Invoke();
            }
        }

        /// <summary>
        /// Process the next item in all queues.
        /// </summary>
        private void ProcessNextInQueue()
        {
            // Handle next action in queue
            if (_mainThreadQueue.TryDequeue(out Action item))
            {
                item.Invoke();
            }
        }

        /// <summary>
        /// Send a goal completion trigger to the server.
        /// </summary>
        /// <param name="goalDescription">The name of the goal that has been completed.</param>
        private void SendGoalCompletionAsync(string goalDescription)
        {
            Debug.Log($"Sending goal completion...");

            // Send goal completion
            _session.SetGoalAchieved();

            Debug.Log($"Sent goal completion status!");
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
        /// Simulate a villager dying and triggering sending a DeathLink (if enabled)
        /// </summary>
        private void SimulateDeath()
        {
            Debug.Log($"Simulating villager death...");
            KillRandomVillager(PlayerName);
        }

        /// <summary>
        /// Simulate a DeathLink received trigger.
        /// </summary>
        private void SimulateDeathLinkReceived()
        {
            Debug.Log($"Simulating DeathLink received...");

            // Add deathlink action to the queue
            AddToQueue(() => HandleDeathLink(new DeathLink("A Test", "Ran a test DeathLink received.")));
        }

        /// <summary>
        /// Simulate a goal completion trigger.
        /// </summary>
        private void SimulateGoalComplete()
        {
            Debug.Log($"Simulating goal complete...");

            // Get current goal type
            switch (_currentGoal.Type)
            {
                case GoalType.KillDemon:
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
                    break;

                case GoalType.KillDemonLord:
                    {
                        // Create a demon lord
                        WorldManager.instance.CreateCard(
                            WorldManager.instance.GetRandomSpawnPosition(),
                            Cards.demon_lord,
                            true,
                            false,
                            true);

                        // Find the demon and kill it
                        if (FindObjectOfType<Demon>() is Demon demon)
                        {
                            demon.Die();
                        }
                    }
                    break;

                default:
                    Debug.LogError($"Unbound goal type: '{_currentGoal.Type}'.");
                    break;
            }
        }

        /// <summary>
        /// Simulate an item received trigger.
        /// </summary>
        /// <param name="type">The type of item to simulate.</param>
        private void SimulateItemReceived(ItemType type)
        {
            Debug.Log($"Simulating {type} item received...");

            // Get all possible item unlocks
            List<Item> items = ItemMapping.Map
                .Where(m => m.ItemType == type)
                .ToList();

            // Select blueprint at random and receive it
            Item item = items.ElementAt(UnityEngine.Random.Range(0, items.Count));
            AddToQueue(() => ItemHandler.HandleItem(item.Name, false));
        }

        /// <summary>
        /// Simulate a quest completion.
        /// </summary>
        private void SimulateQuestComplete()
        {
            Debug.Log($"Simulating quest completion...");

            // Get a list of all currently incomplete quests (for mainland)
            List<Quest> incompleteQuests = QuestManager.instance.AllQuests
                .Where(q => q.QuestLocation == Location.Mainland && !QuestManager.instance.QuestIsComplete(q))
                .ToList();

            // Select random quest
            Quest quest = incompleteQuests[UnityEngine.Random.Range(0, incompleteQuests.Count)];

            // Complete a random quest from the list
            WorldManager.instance.QuestCompleted(quest);
        }

        #endregion
    }
}