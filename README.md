# Stacklands Archipelago Randomizer
A mod for Stacklands that allows it to be integrated into an Archipelago world.

This repo was forked from and is a continuation of the development of the original mod by **chandler05**: [chandler05/Stacklands-Randomizer](https://github.com/chandler05/Stacklands-Randomizer)

## How does this AP Randomizer work?
In an archipelago randomized run of Stacklands, all quests are Location Checks and all Progression Items received will be **Ideas** _(AKA 'Blueprints')_ in order to make the craftable items required to complete the quests. The quests themselves have not changed so the requirements for each quest remain the same as vanilla, but the order in which they are completed will depend on the randomisation of the **Ideas** and the order in which you receive them.

### Goals
- Complete the **Kill the Demon** quest on Mainland
- Complete the **Fight the Wicked Witch** quest on The Dark Forest
- Complete both of the aboce

### Location Checks
- All **Mainland** Quests
- All **The Dark Forest** Quests
- **Mobsanity** Quests _(killing one of each enemy type)_
- _More coming soon..._

### Progression Items
- Booster Packs
- Ideas for items required to complete Quests such as `Plank`, `Quarry`, `Smithy`, `Temple` etc.
- Board Expansion _(works like Shed and Warehouse and expands your card limit and board size)_

### Useful Items
- Ideas for items that help make the run easier but don't impact progression such as `Coin Chest`, `Hammer`, `Warehouse` etc.

### Filler / Junk Items
- Ideas for items that have very little impact on progression such as `Butchery`, `Club`, `Dustbin` etc.
- Resource Booster Pack _(pulls 1x Structure, 2x Food and 2x Resource cards)_
- _More to be added soon..._

### Trap Items
- Feed Villagers Trap _(immediately forces the 'Feed Villagers' cutscene)_
- Mob Trap _(spawns a random, basic enemy)_
- Sell Cards Trap _(immediately forces the 'Sell Cards' cutscene)_
- Strange Portal Trap _(spawns a strange portal)_
- _More to be added soon..._

## Current In-Game Support
- Mainland _(full support)_
- The Dark Forest _(full support)_
- ~~The Island~~ _(coming soon...)_
- ~~Stacklands 2000 DLC~~ _(not yet planned)_
- ~~Cursed Worlds DLC~~ _(not yet planned)_

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
