// This is a stub script to give Visual Studio an idea what data is available

#r "netstandard.dll"

// If you're editing the template from the console app project
#r "../bin/Debug/netcoreapp3.1/SkyEditor.RomEditor.dll"

// If you're editing the template from the release
#r "../SkyEditor.RomEditor.dll"

SkyEditor.RomEditor.Domain.Rtdx.IRtdxRom Rom;
SkyEditor.RomEditor.Infrastructure.Automation.Modpacks.IScriptModAccessor Mod;
SkyEditor.RomEditor.Infrastructure.Automation.ScriptUtilities Utilities;