using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using IOPath = System.IO.Path;
using SkyEditorUI.Infrastructure;
using System.IO;

namespace SkyEditorUI.Controllers
{
    class StringsController : Widget
    {
        [UI] private Label? labelLangName;
        [UI] private ListStore? stringsStore;
        [UI] private TreeViewColumn? keyColumn;
        [UI] private ListStore? categoryStore;
        [UI] private TreeView? stringsTree;
        [UI] private TreeSelection? stringsTreeSelection;
        [UI] private TreeSelection? categoryTreeSelection;
        [UI] private SearchEntry? searchEntry;
        private TreeModelFilter filter;
        private string searchText = "";

        private StringType category;
        private LocalizedStringCollection strings;

        private const int CategoryIdColumn = 0;
        private const int StringHashColumn = 0;
        private const int StringValueColumn = 1;
        private const int StringFontWeightColumn = 2;
        private const int StringHashNameColumn = 3;
        private const int StringIndexColumn = 4;

        public StringsController(IRtdxRom rom, ControllerContext context)
            : this(new Builder("Strings.glade"), rom, context)
        {
        }

        private StringsController(Builder builder, IRtdxRom rom, ControllerContext context)
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            var language = ((StringsControllerContext) context).Language;
            strings = rom.GetStrings().GetStringsForLanguage(language);
            labelLangName!.Text = $"{language.GetFriendlyName()} Strings";

            if (categoryStore!.GetIterFirst(out var iter))
            {
                categoryTreeSelection!.SelectIter(iter);
            }

            filter = new TreeModelFilter(stringsStore, null);
            stringsTree!.Model = filter;
            filter.VisibleFunc = StringVisibleFunc;
        }

        private void OnImportClicked(object sender, EventArgs args)
        {
            var fileDialog = new FileChooserNative("Select a folder", MainWindow.Instance,
                FileChooserAction.SelectFolder | FileChooserAction.Open, null, null);

            var response = (ResponseType) fileDialog.Run();

            if (response != ResponseType.Accept)
            {
                fileDialog.Destroy();
                return;
            }

            var path = fileDialog.File.Path;
            fileDialog.Destroy();

            var commonPath = IOPath.Combine(path, "common.csv");
            var dungeonPath = IOPath.Combine(path, "dungeon.csv");
            var scriptPath = IOPath.Combine(path, "script.csv");

            if (!File.Exists(commonPath) || !File.Exists(dungeonPath) || !File.Exists(scriptPath)) {
                UIUtils.ShowErrorDialog(MainWindow.Instance, "Import failed", "Folder must contain the files "
                    + "'common.csv', 'dungeon.csv' and 'script.csv'");
                return;
            }

            strings.ImportFromCsvFile(StringType.Common, commonPath);
            strings.ImportFromCsvFile(StringType.Dungeon, dungeonPath);
            strings.ImportFromCsvFile(StringType.Script, scriptPath);

            UIUtils.ShowInfoDialog(MainWindow.Instance, "Import successful", "Strings have been successfully imported from CSV files");

            LoadStrings(category);
        }

        private void OnExportClicked(object sender, EventArgs args)
        {
            var fileDialog = new FileChooserNative("Select a folder", MainWindow.Instance,
                FileChooserAction.SelectFolder | FileChooserAction.Save, null, null);

            var response = (ResponseType) fileDialog.Run();

            if (response != ResponseType.Accept)
            {
                fileDialog.Destroy();
                return;
            }

            var path = fileDialog.File.Path;
            fileDialog.Destroy();

            strings.ExportToCsvFile(StringType.Common, IOPath.Combine(path, "common.csv"));
            strings.ExportToCsvFile(StringType.Dungeon, IOPath.Combine(path, "dungeon.csv"));
            strings.ExportToCsvFile(StringType.Script, IOPath.Combine(path, "script.csv"));

            UIUtils.ShowInfoDialog(MainWindow.Instance, "Export successful", "Exported 3 files: "
                + "'common.csv', 'dungeon.csv' and 'script.csv'");
        }

        private void OnCategorySelectionChanged(object sender, EventArgs args)
        {
            if (categoryTreeSelection!.GetSelected(out var model, out var iter))
            {
                category = (StringType) model.GetValue(iter, CategoryIdColumn);
                LoadStrings(category);
            }
        }
        
        private void OnEditClicked(object sender, EventArgs args)
        {
            if (stringsTreeSelection!.GetSelected(out var model, out var iter))
            {
                int hash = (int) model.GetValue(iter, StringHashColumn);
                string value = (string) model.GetValue(iter, StringValueColumn) ?? "";
                int index = (int) model.GetValue(iter, StringIndexColumn);

                var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.Modal, MessageType.Other,
                    ButtonsType.OkCancel, "Edit String");
                dialog.Title = "String Editor";

                var scrolledWindow = new ScrolledWindow();
                var textView = new TextView();
                textView.Buffer.Text = value;
                scrolledWindow.Add(textView);
                scrolledWindow.SetSizeRequest(550, 200);
                dialog.ContentArea.PackEnd(scrolledWindow, true, true, 0);

                dialog.ShowAll();
                var response = (ResponseType) dialog.Run();
                var text = textView.Buffer.Text;
                dialog.Destroy();

                if (response == ResponseType.Ok)
                {
                    strings.SetString(category, hash, text);

                    // GTK madness since TreeModelFilter.SetValue() throws a NotImplementedException
                    if (stringsStore!.GetIterFromString(out var stringIter, index.ToString()))
                    {
                        stringsStore!.SetValue(stringIter, StringValueColumn, text);
                        stringsStore!.SetValue(stringIter, StringFontWeightColumn, 600);
                    }
                }
            }
        }

        private void OnSearchChanged(object sender, EventArgs args)
        {
            searchText = searchEntry!.Text;
            filter.Refilter();
        }

        private void LoadStrings(StringType type)
        {
            stringsStore!.Clear();

            int i = 0;
            foreach (var str in strings.GetStrings(type))
            {
                int fontWeight = str.isOverrideString ? 600 : 400;
                stringsStore!.AppendValues(str.hash, str.value, fontWeight, str.hashName, i++);
            }

            keyColumn!.Visible = type != StringType.Script;
        }

        private bool StringVisibleFunc(ITreeModel model, TreeIter iter)
        {
            int hash = (int) model.GetValue(iter, StringHashColumn);
            string value = (string) model.GetValue(iter, StringValueColumn);
            string? hashName = (string?) model.GetValue(iter, StringHashNameColumn);

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            string lowerSearchText = searchText.ToLower();
            return hash.ToString().Contains(searchText) || (value?.ToLower()?.Contains(lowerSearchText) ?? false)
                || (hashName?.ToLower()?.Contains(lowerSearchText) ?? false);
        }
    }
}
