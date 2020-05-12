# SkyEditor.RomEditor.Rtdx

A work-in-progress ROM editor for Pokémon Mystery Dungeon: Rescue Team DX.

To use this, you need a decrypted and extracted copy of the game. If your console has the update installed, you'll need the updated executable.

## Building

This is in the early stages of development, so you'll need to import the [skyeditor NuGet feed](https://dev.azure.com/project-pokemon/Sky%20Editor/_packaging?_a=feed&feed=skyeditor%40Local):
```
https://pkgs.dev.azure.com/project-pokemon/4a5da2d7-09b8-4354-9450-562116aac7b5/_packaging/skyeditor/nuget/v3/index.json
```

## Usage

For now, the only user interface with the code is a command-line app. A cross-platform UI is planned, but won't be started until the code library is more mature.

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

- SkyEditor.RomEditor.Rtdx - Code editing library containg the fun bits
    - Domain - Custom code dedicated to RTDX stuff
        - Automation - Code dedicated to giving Lua scripts access to RTDX classes
        - Library - A simple library of ROMs for users' convenience. For the console project, this powers all "library:" operations.
        - Models - Aggregations of data into logical units
        - Structures - Lower level data access to interact with specific file formats
    - Infrastructure - Helper code not specific to the RTDX domain
    - Reverse - Stubs and custom implementation of portions of the RTDX executable, such as enumerations and data strutures.
- SkyEditor.RomEditor.Rtdx.ConsoleApp - Console app made primarily to make the library usable
    - Scripts - Premade Lua scripts containing queries and other samples
- SkyEditor.RomEditor.Rtdx.Tests - Exactly what it says on the tin

## Notable Features

### Editing Starters

See Scripts/Samples/ChangeStarters.lua in the console app project for a rough idea how to do it. Note that the models during the personality test _might not_ match your selected Pokémon, but the portrait and in-game models will match. This is unlikely to change in the forseable future due to how heavily hard-coded it is in the game.

### Lua Generation

Most of the control over ROMs is done with Lua scripts. These can be written in your favorite code editor, but Sky Editor can _generate_ scripts for certain kinds of edits, such as starter editing. The idea behind this is that a user will use a GUI to make edits, then will be able to generate a script that allows their friends to do the same.

Right now, the change starters sample script can be used to generate itself, minus certain comments, as a proof-of-concept.

More than just starters will be supported in the future.
