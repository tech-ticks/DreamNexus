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
