using System;
using System.Linq;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditorUI.Infrastructure;
using IOPath = System.IO.Path;

namespace SkyEditorUI.Controllers
{
    class ModpackScriptsController : SimpleController
    {
        private Modpack modpack;

        public ModpackScriptsController(IRtdxRom rom, Modpack modpack) : base("Modpack Automation Scripts")
        {
            this.modpack = modpack;
        }

        protected override Widget GetContent()
        {
            return CreateContentButtonWithLabel("These scripts get executed when a modpack is loaded. "
                + "Please reload the modpack when you make changes. C# scripts (.csx) and Lua scripts (.lua) are supported.",
                "Add", "skytemple-list-add-symbolic");
        }

        protected override void OnButtonClicked(object? sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.Modal, MessageType.Other, ButtonsType.Ok,
                "Enter the name of the new script (ending with .csx or .lua):");
            dialog.Title = "New Script";

            var entry = new Entry();
            dialog.ContentArea.PackEnd(entry, false, false, 0);

            dialog.ShowAll();
            var response = (ResponseType) dialog.Run();
            var fileName = entry.Text;
            dialog.Destroy();

            if (response != ResponseType.Ok)
            {
                return;
            }

            var lowerFileName = fileName.ToLower();

            if (!lowerFileName.EndsWith(".csx") && !lowerFileName.EndsWith(".lua"))
            {
                UIUtils.ShowErrorDialog(MainWindow.Instance, "Couldn't create script", "Name must end with .csx or .lua");
                return;
            }

            if (!lowerFileName.StartsWith("scripts/") || lowerFileName.StartsWith("scripts\\"))
            {
                // The path must use "/" to be portable across platforms
                fileName = IOPath.Combine("scripts", fileName).Replace("\\", "/");
            }

            var defaultMod = modpack.GetDefaultMod();
            if (defaultMod == null)
            {
                throw new Exception("Could not find default mod.");
            }

            if (defaultMod.Metadata?.Scripts?.Contains(fileName) ?? false)
            {
                UIUtils.ShowErrorDialog(MainWindow.Instance, "Couldn't create script",
                    "A script with the given name already exists.");
                return;
            }
            defaultMod.AddScript(fileName);
            MainWindow.Instance?.InitMainList();
        }
    }
}
