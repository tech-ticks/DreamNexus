# SkyEditor.RomEditor.Rtdx

A work-in-progress ROM editor for Pokémon Mystery Dungeon: Rescue Team DX.

To use this, you need a decrypted and extracted copy of the game. If your console has the update installed, you'll need the updated executable.

## Licensing

The UI project (`SkyEditor.UI`) is based on [SkyTemple](https://github.com/SkyTemple/skytemple) and licensed under GPLv3.
All other all components including the core library and CLI are licensed under the MIT license.

## Building

This is in the early stages of development, so you'll need to import the [skyeditor NuGet feed](https://dev.azure.com/project-pokemon/Sky%20Editor/_packaging?_a=feed&feed=skyeditor%40Local):
```
https://pkgs.dev.azure.com/project-pokemon/4a5da2d7-09b8-4354-9450-562116aac7b5/_packaging/skyeditor/nuget/v3/index.json
```

## Usage

### UI

Run `SkyEditor.UI.exe` or your platform-specific variant.
The UI should be self-explanatory, and a tutorial is planned once more progress has been made. 
In the mean time, feel free to open a [GitHub Issue](https://github.com/tech-ticks/SkyEditor.RomEditor.Rtdx/issues) if you have any questions.

### Console App

```
Usage: 
dotnet SkyEditor.RomEditor.Rtdx.Console.dll <RomDirectory|library:itemName> <Script1.lua|Command> [Script2.lua|Command2] [...] [--save | --save-to <RomDirectory>]
```

The most basic usage is to load a ROM and run a Lua script against it. Example:
```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Queries/ListStarters.lua
```

Saving is not done automatically, so you must specify you want it done after the scripts:
```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua --save
```

The above will modify ./RTDX. If you want to save modified files to a different directory, use --save-to instead:
```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua --save-to ./RTDX-modified
```

Scripts can be chained together to operator on the same ROM:
```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Samples/ChangeStarters.lua ./Scripts/Hypothetical/ChangeMoreStarters.lua --save-to ./RTDX-modified
```

Operations on multiple ROMs can be chained together simply by specifying another ROM directory. The next example will do the following:

1. Load a ROM from ./RTDX
2. List the starters in the ROM that was just loaded
3. Load a ROM from ./RTDX-copy and reset Lua global state
4. Change the starters in ./RTDX-copy without saving
5. Change different starters in ./RTDX-copy without saving, but with the changes from step 4.
6. Save the changes made in steps 4 and 5 to ./RTDX-copy without affecting ./RTDX.

```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX ./Scripts/Queries/ListStarters.lua ./RTDX-Copy ./Scripts/Samples/ChangeStarters.lua ./Scripts/Hypothetical/ChangeMoreStarters.lua --save
```

In reality, you're likely to have long path names that look something like `H:\atmosphere\contents\01003D200BAA2000` or some variant. If this gets to be a hassle, you can Add the ROM to Sky Editor's library:
```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll ./RTDX Import MyRom
```

Once imported, it can be accessed with `library:ItemName`:
```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll library:MyRom ./Scripts/Queries/ListStarters.lua
```

You can have as many ROMs in your library as disk space allows, and you can list what's in the library using:
```
dotnet SkyEditor.RomEditor.Rtdx.Console.dll ListLibrary
```

## Organization

- SkyEditor.RomEditor - Code editing library containg the fun bits
    - Domain - Code in the domain of ROM editing
        - Library - A simple library of ROMs for users' convenience. For the console project, this powers all "library:" operations.
        - RTDX - Code specific to Pokémon Mystery Dungeon: Rescue Team DX
            - Models - Aggregations of data into logical units
            - Structures - Lower level data access to interact with specific file formats
        - PSMD - Code specific to Pokémon Super Mystery Dungeon
    - Infrastructure - Helper code not specific to the RTDX domain
        - Automation - Code to host 3rd party scripts to modify ROMs
    - Reverse - Stubs and custom implementation of portions of the RTDX executable, such as enumerations and data strutures.
- SkyEditor.RomEditor.ConsoleApp - Console app made primarily to make the library usable
    - Scripts - Premade Lua scripts containing queries and other samples
- SkyEditor.RomEditor.Tests - Exactly what it says on the tin

## Notable Features

### Editing Starters

See Scripts/Samples/ChangeStarters.lua in the console app project for a rough idea how to do it.

Note that the models during the personality test _might not_ match your selected Pokémon, but the portrait and in-game models will match.
You can currently fix this mismatch by manually modifying and executing `SkyEditor.RomEditor.Rtdx.ConsoleApp/Scripts/CSharp/ActDatabaseEditing.csx`.

### Change Script Generation

Most of the control over ROMs is done with scripts. These can be written in your favorite code editor, but Sky Editor can _generate_ scripts for certain kinds of edits, such as starter editing. The idea behind this is that a user will use a GUI to make edits, then will be able to generate a script that allows their friends to do the same.

Supported languages are C# and Lua, but this is subject to change in the future.

Right now, the change starters sample script can be used to generate itself, minus certain comments, as a proof-of-concept.

More than just starters will be supported in the future.

## Mods
Sky Editor features a mod system allowing people to create and distribute custom modifications that aren't supported by the UI.

Note that all mods will be applied to Pokémon Mystery Dungeon: Rescue Team DX, but more ROMs will be supported in the future.

### Scripts
The most basic layer of modification is the script. These are C# (.csx files) or Lua (.lua files) that have access to a ROM and can modify its contents in any way the Sky Editor library supports.

See SkyEditor.RomEditor.Infrastructure.Automation.ScriptContext for which globals are available.

### Mods
A mod is a collection of one or more scripts that serve the same end goal. To make a mod, create a directory and make a mod.json file with contents that look like the following:

```
{
  "Id": "SkyEditor.Rtdx.MyMod",
  "Version": "1.0.0",
  "Target": "RTDX",
  "Name": "My Mod",
  "Description": "Some text that will someday be shown in a UI somewhere some day",
  "Scripts": [
    "MyFirstScript.csx",
    "MySecondScript.lua"
  ]
}
```

This JSON refers to two scripts which must be located in the same directory as the mod.json file.

When this mod is applied, first MyFirstScript.csx will run against the ROM, then MySecondScript.lua.

If any other files are present in this directory, they can be read from a script using a method such as `Mod.ReadResourceText("myFile.txt")`. Reading resources in this manor is not supported when scripts are run standalone.

If the mod is ready to distribute, you can put the contents of this directory into a .zip file, which will work fine so long as the mod.json is in the root of the zip file.

### Modpacks

Modpacks are collections of mods that will be distributed together. Eventually, the end user will have the option of choosing which mods to apply and which ones not to, which can be useful if some changes should be optional or are otherwise not for everyone.

Modpacks can be created in two ways: From scratch or from existing mods.

#### Creating a modpack from scratch

This method is very similar to the creating a mod. To make a modpack from scratch, create a directory and make a modpack.json file with contents that look like the following:

```
{
  "Id": "SkyEditor.Rtdx.Randomizer.Starters",
  "Version": "1.0.0",
  "Target": "RTDX",
  "Name": "Randomizer",
  "Description": "Are you bored of Pokémon Mystery Dungeon always being too predicatable, despite dungeons being randomly generated? Wouldn't it be great if even MORE things were random? This modpack is for you!",
  "Mods": [
    {
      "Id": "SkyEditor.Rtdx.Randomizer.Starters",
      "Version": "1.0.0",
      "Name": "Randomize Starters",
      "Description": "Randomizes the Pokémon you and your partner can be at the start of the game.",
      "Enabled": true,
      "Scripts": [
        "RandomizeStarters.csx"
      ]
    },    
    {
      "Id": "SkyEditor.Rtdx.Randomizer.LevelUpModes",
      "Version": "1.0.0",
      "Name": "Randomize Level Up Moves",
      "Description": "Randomizes the moves Pokémon gain when gaining levels, and the levels at which they're earned.",
      "Enabled": true,
      "Scripts": [
        "RandomizeLevelUpMoves.csx"
      ]
    },    
    {
      "Id": "SkyEditor.Rtdx.Randomizer.LevelUpModes",
      "Version": "1.0.0",
      "Name": "Randomize Level Up Levels",
      "Description": "Randomizes the levels at which moves are learned. This mod is disabled, because this could wreck game balance if you're unlucky enough to start as a Pokémon who only learns moves at levels 80-100.",
      "Enabled": false,
      "Scripts": [
        "RandomizeLevelUpLevels.csx"
      ]
    }
  ]
}
```

These mods will be applied in the order specified in the JSON file. Mods whose "Enabled" value is false will be skipped unless the user changes it (using a UI that does not yet exist).

If the modpack is ready to distribute, you can put the contents of this directory into a .zip file, which will work fine so long as the modpack.json is in the root of the zip file.

#### Creating a modpack from mods

If you have one or more mods already available, and you would like to make a modpack out of them, you can use either the UI or the console. The UI is easiest and is accessible from the Automation -> Create Modpack menu item. Altnernatively the console can be used with a command such as the following:

```
dotnet SkyEditor.RomEditor.dll pack Mods/TheFennekinMod Mods/TheRioluMod --id=evandixonFavoriteStarters --version=0.1.0 --name="evandixon's Favorite Starters" --description="Allows playing as some of evandixon's favorite starter Pokémon" --author="evandixon" --target=RTDX --save-to "Modpacks/evandixon's Favorites.zip"
```

### When to use Scripts, Mods, or Modpacks

Which option to use depends on your specific circumstances, but in general:

- If you plan to give your modification to others, create a Mod or a Modpack.
- If your script needs access to a static resource (such as a Pokémon model), create a Mod or a Modpack.
- If you don't need users' input (e.g. you don't want them to disable something), create a Mod.
- If you want to make your own Mod, but you also want to use someone else's Mod(s), create a Mod, then combine it with the other(s) with the Sky Editor console to make a Modpack.
- If you want users to be able to enable or disable specific features, create a Modpack, with each feature that can be enabled or disabled as a separate Mod.
- If you don't intend to make any changes (e.g. if you're extracting Pokémon data), create a Script.
- If you plan to keep your modification for yourself, a Script is easiest unless one of the above points applies.
