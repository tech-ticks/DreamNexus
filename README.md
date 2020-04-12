# SkyEditor.RomEditor.Rtdx

A work-in-progress ROM editor for Pokémon Mystery Dungeon: Rescue Team DX.

## Building

This is in the early stages of development, so you'll need to import the [skyeditor NuGet feed](https://dev.azure.com/project-pokemon/Sky%20Editor/_packaging?_a=feed&feed=skyeditor%40Local):
```
https://pkgs.dev.azure.com/project-pokemon/4a5da2d7-09b8-4354-9450-562116aac7b5/_packaging/skyeditor/nuget/v3/index.json
```

If you want to make use of conversion to/from NSO/ELF files, you'll need to make sure dependencies such as elf2nso.exe and nx2elf-win-x64.dll are in the output directory. This should be fixed in the near future, but in the mean time, can be worked around by selecting the files in Solution Explorer, and using the Properties dialog to enable "Copy to Output Directory".

## Usage

This doesn't yet do anything the average user will care about. SkyEditor.RomEditor.Rtdx.ConsoleApp can be used as a manual test harness if you have the a decrypted and extracted ROM already available.

## Organization

- SkyEditor.RomEditor.Rtdx - Code editing library containg the fun bits
    - Domain - Custom code dedicated to RTDX stuff
        - Automation - Code dedicated to giving Lua scripts access to RTDX classes
        - Models - Aggregations of data into logical units
        - Structures - Lower level data access to interact with specific file formats
    - Infrastructure - Helper code not specific to the RTDX domain
    - Reverse - Stubs and custom implementation of portions of the RTDX executable, such as enumerations and data strutures.
- SkyEditor.RomEditor.Rtdx.ConsoleApp - Console app made primarily to make the library usable
    - Scripts - Premade Lua scripts containing queries and other samples
- SkyEditor.RomEditor.Rtdx.Tests - Exactly what it says on the tin
