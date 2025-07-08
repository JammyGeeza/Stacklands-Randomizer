# Stacklands Archipelago Randomizer
A mod for Stacklands that allows it to be integrated into an Archipelago world.

This repo was forked from and is a continuation of the development of the original mod: [chandler05/Stacklands-Randomizer](https://github.com/chandler05/Stacklands-Randomizer)

## How it Works
In an archipelago run of Stacklands, you will be completing the in-game quests to complete Location Checks and items received will be Ideas _(AKA 'Blueprints')_ to make craftable items.
Quest completions remain the same as the base game, but the order in which they can be completed will depend on the randomisation of the ideas received.

### Locations
- 'Mainland' Quests
- 'The Dark Forest' Quests _(toggled on/off in YAML)_
- Mobsanity _(killing one of each enemy type - toggled on/off in YAML)_

### Items
**Progression** 
- Booster Packs
- Ideas _(AKA 'Blueprints')_ for items required to complete Quests such as `Plank`, `Quarry`, `Smithy`, `Temple` etc.
**Useful** 
- Ideas _(AKA 'Blueprints')_ for items that help make the run easier but don't impact progression such as `Coin Chest`, `Hammer`, `Warehouse` etc.
**Filler / Junk**
- Food and Material resources such as ``
**Traps**
- Enemy Spawns
- Flood of useless items worth 0 coins
- _(More to be added soon...)_

### Current Support
- Mainland _(full support)_
- The Dark Forest _(full support)_
- ~~The Island~~ _(coming soon...)_
- ~~DLC~~ _(not yet planned)_

## Known Issues
For the list of currently known issues, please see the [open issues](https://github.com/JammyGeeza/Stacklands-Randomizer/issues)

## How to Install the Mod to Stacklands
1. Download the `StacklandsRandomizer-vX.X.X.zip` file included in your chosen release. _([find the latest release here](https://github.com/JammyGeeza/Stacklands-Randomizer/releases/latest))_
2. Navigate to `%LocalAppData%low\sokpop\Stacklands\Mods`.
   - _This path will typically be: `C:\Users\[your user]\AppData\LocalLow\sokpop\Stacklands\Mods`_
3. Extract the contents of the `StacklandsRandomizer-vX.X.X.zip` file to this directory.
   - _Your file and directory structure should look like: `..\sokpop\Stacklands\Mods\StacklandsRandomizer\[all .dll, .json etc. files here]`_
     ![image](https://github.com/user-attachments/assets/d83d0da5-e053-4de8-af1d-aa01b02e669c)
4. Start the game

## How to Use the Mod
1. From the Main Menu, go to `Mods -> Archipelago Randomizer`
2. Enter the server name (including port), the Slot Name and the password (if required)
3. Click 'Connect' - the game will quickly restart. _(This is a temporary solution for now)_
4. If successful, the main menu box should now display `"Archipelago: Connected"`
5. Start a new run - a new game save dedicated to the server's seed will have been created so your existing game saves are safe. 
6. Once you have completed your goal, go back to `Mods -> Archipelago Randomizer` and click `Send Goal`. It will _only_ send once you have completed your goal. _(This is a temporary solution for now)_
