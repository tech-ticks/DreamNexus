using System;
using IOPath = System.IO.Path;
using Gtk;
using GtkSource;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    class SourceFileController : Box
    {
        private SourceFile file;
        private IRtdxRom rom;
        private Modpack modpack;

        [UI] private ScrolledWindow? sourceViewContainer;
        [UI] private Box? addToModpackContainer;
        [UI] private Box? removeFromModpackContainer;
        [UI] private Label? fileNameLabel;

        private SourceView sourceView;

        public SourceFileController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("SourceFile.glade"), rom, modpack, context) {
        }

        private SourceFileController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            file = (context as SourceFileControllerContext)!.SourceFile;
            this.rom = rom;
            this.modpack = modpack;
            
            sourceView = new SourceView();
            SetupSourceView();
            
            sourceViewContainer!.Add(sourceView);
            sourceViewContainer.ShowAll();

            addToModpackContainer!.Visible = file.External && !file.InProject;
            removeFromModpackContainer!.Visible = file.External && file.InProject;
            RefreshPath();
        }

        private void SetupSourceView()
        {
            sourceView = new SourceView();
            sourceView.Monospace = true;
            sourceView.AutoIndent = true;
            sourceView.ShowLineNumbers = true;
            sourceView.TabWidth = 4;
            sourceView.IndentWidth = 4;
            sourceView.IndentOnTab = true;
            sourceView.HighlightCurrentLine = true;
            sourceView.InsertSpacesInsteadOfTabs = true;
            sourceView.SmartBackspace = true;
            sourceView.Buffer.Language = new LanguageManager().GetLanguage(GetLanguageIdFromExtension());
            sourceView.Buffer.Text = file.Contents;
            sourceView.Editable = file.InProject;

            sourceView.FocusOutEvent += OnSourceViewFocusOut;
        }

        private void OnOpenInVSCodeClicked(object sender, EventArgs args)
        {
            UIUtils.OpenInVSCode(modpack.Directory!, MainWindow.Instance!, file.Path);
        }

        private void OnAddToModpackClicked(object sender, EventArgs args)
        {
            file.CopyToModpack(rom, modpack);
            addToModpackContainer!.Visible = false;
            removeFromModpackContainer!.Visible = true;
            sourceView.Editable = true;
            RefreshPath();
        }

        private void OnRemoveFromModpackClicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.Modal, MessageType.Question,
                ButtonsType.OkCancel, false, "Do you really want to restore this file? "
                    + "Your changes to this file will be lost IRRECOVERABLY!");

            dialog.Title = $"Restore {IOPath.GetFileName(file.Path)}";
            var response = (ResponseType) dialog.Run();
            dialog.Destroy();

            if (response != ResponseType.Ok)
            {
                return;
            }

            file.RemoveFromModpack(modpack);
            addToModpackContainer!.Visible = true;
            removeFromModpackContainer!.Visible = false;

            sourceView.Buffer.Text = file.Contents;
            sourceView.Editable = false;
            RefreshPath();
        }

        private void OnRefreshClicked(object sender, EventArgs args)
        {
            if (file.IsDirty)
            {
                var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.Modal, MessageType.Question,
                ButtonsType.OkCancel, false, "Do you want to reload the file from disk? Your changes will be overwritten.");

                dialog.Title = $"Reload from disk";
                var response = (ResponseType) dialog.Run();
                dialog.Destroy();

                if (response != ResponseType.Ok)
                {
                    return;
                }
            }

            file.Refresh();
            file.IsDirty = false;
            sourceView.Buffer.Text = file.Contents;
        }


        private void OnSourceViewFocusOut(object sender, EventArgs args)
        {
            if (file.InProject)
            {
                Console.WriteLine("Applying file contents");
                file.Contents = sourceView.Buffer.Text;
            }
        }

        private void RefreshPath()
        {
            fileNameLabel!.Text = file.InProject ? IOPath.GetRelativePath(modpack.Directory!, file.Path) : file.Path;
        }

        private string? GetLanguageIdFromExtension()
        {
            var extension = IOPath.GetExtension(file.Path);
            if (extension == ".csx")
            {
                return "c-sharp";
            }
            else if (extension == ".lua")
            {
                return "lua";
            }
            else if (extension == ".data")
            {
                return "json";
            }
            return null;
        }
    }
}
