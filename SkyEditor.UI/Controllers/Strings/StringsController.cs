using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

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
            stringsTree!.Model = new TreeModelSort(filter);
            filter.VisibleFunc = StringVisibleFunc;
        }

        private void OnImportClicked(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private void OnExportClicked(object sender, EventArgs args)
        {
            throw new NotImplementedException();
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
            var stringList = strings.GetStrings(type).ToArray();
            var knownHashes = type != StringType.Script
                ? new HashSet<int>(Enum.GetValues<TextIDHash>().Cast<int>()) : new HashSet<int>();

            for (int i = 0; i < stringList.Length; i++)
            {
                (int hash, string value, bool isOverrideString) str = stringList[i];
                int fontWeight = str.isOverrideString ? 600 : 400;
                string hashName = knownHashes.Contains(str.hash) ? ((TextIDHash) str.hash).ToString() : "";
                stringsStore!.AppendValues(str.hash, str.value, fontWeight, hashName, i);
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
            return hash.ToString().Contains(searchText) || value.ToLower().Contains(lowerSearchText) 
                || (hashName?.ToLower().Contains(lowerSearchText) ?? false);
        }
    }
}
