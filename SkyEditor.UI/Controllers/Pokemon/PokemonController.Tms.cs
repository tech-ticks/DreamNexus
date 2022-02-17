using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private TreeView? tmTree;
        [UI] private ListStore? tmStore;
        [UI] private EntryCompletion? tmCompletion;

        const int TmIndexColumn = 0;
        const int TmNameColumn = 1;

        private void LoadTmsTab()
        {
            foreach (var tm in pokemon.LearnableTMs)
            {
                tmStore!.AppendValues((int) tm, AutocompleteHelpers.FormatItem(rom, tm));
            }
        }

        private void OnTmEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (tmStore!.GetIter(out var iter, path))
            {
                int index = path.Indices[0];
                var tmItem = AutocompleteHelpers.ExtractItem(args.NewText);
                if (tmItem.HasValue
                    && tmItem.Value >= ItemIndexConstants.BROKENMACHINE_MIN && tmItem.Value <= ItemIndexConstants.BROKENMACHINE_MAX)
                {
                    pokemon.LearnableTMs[index] = tmItem.Value;
                    tmStore!.SetValue(iter, TmIndexColumn, (int) tmItem);
                    tmStore!.SetValue(iter, TmNameColumn, AutocompleteHelpers.FormatItem(rom, tmItem.Value));
                }
            }
        }

        private void OnTmEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = tmCompletion;
        }

        private void OnAddTmClicked(object sender, EventArgs args)
        {
            var newTm = Enum.GetValues<ItemIndex>().FirstOrDefault(item => item >= ItemIndexConstants.BROKENMACHINE_MIN
                && item <= ItemIndexConstants.BROKENMACHINE_MAX && !pokemon.LearnableTMs.Contains(item));

            if (newTm == default)
            {
                UIUtils.ShowInfoDialog(MainWindow.Instance, "Cannot add TM", "All TMs were already added.");
                return;
            }

            tmStore!.AppendValues((int) newTm, AutocompleteHelpers.FormatItem(rom, newTm));
            pokemon.LearnableTMs.Add(newTm);
        }

        private void OnRemoveTmClicked(object sender, EventArgs args)
        {
            if (tmTree!.Selection.GetSelected(out var _, out var iter))
            {
                var index = tmStore!.GetPath(iter).Indices[0];
                pokemon.LearnableTMs.RemoveAt(index);
                tmStore!.Remove(ref iter);
            }
        }
    }
}
