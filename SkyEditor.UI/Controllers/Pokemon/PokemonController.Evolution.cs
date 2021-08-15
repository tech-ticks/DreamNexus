using System;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using SkyEditorUI.Infrastructure;
using UI = Gtk.Builder.ObjectAttribute;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private Entry? entryEvolvesFrom;
        [UI] private Entry? entryMegaEvo1;
        [UI] private Entry? entryMegaEvo2;
        [UI] private ListStore? evolutionsStore;
        [UI] private EntryCompletion? evolutionListCompletion;
        [UI] private TreeSelection? evolutionsTreeSelection;

        [UI] private Switch? switchEvolutionLevel;
        [UI] private Entry? entryEvolutionLevel;
        [UI] private Switch? switchEvolutionItem;
        [UI] private Entry? entryEvolutionItem;
        [UI] private Entry? entryEvolutionItemAmount;
        [UI] private Entry? entryEvolutionUnknown;
        [UI] private Box? boxItemIndexAndAmount;
        [UI] private Button? btnRemoveEvolution;

        private const int EvolutionIndexColumn = 0;
        private const int EvolutionNameColumn = 1;
        private const int EvolutionCreatureIndexColumn = 2;

        private PokemonEvolution.PokemonEvolutionBranch? selectedEvolution;

        private void LoadEvolutionTab()
        {
            entryEvolvesFrom!.Text = AutocompleteHelpers.FormatPokemon(rom, pokemon.EvolvesFrom);
            entryMegaEvo1!.Text = AutocompleteHelpers.FormatPokemon(rom, pokemon.MegaEvolutions.First);
            entryMegaEvo2!.Text = AutocompleteHelpers.FormatPokemon(rom, pokemon.MegaEvolutions.Second);

            RefreshEvolutionList();

            if (evolutionsStore!.GetIterFirst(out var iter))
            {
                evolutionsTreeSelection!.SelectIter(iter);
            }
        }

        private void RefreshEvolutionList()
        {
            evolutionsStore!.Clear();
            for (int i = 0; i < pokemon.EvolutionBranches.Count; i++)
            {
                var evolution = pokemon.EvolutionBranches[i];
                evolutionsStore!.AppendValues(i, AutocompleteHelpers.FormatPokemon(rom, evolution.Evolution),
                    (int) evolution.Evolution);
            }
        }

        private void OnEvolvesFromChanged(object sender, EventArgs args)
        {
            var creatureIndex = AutocompleteHelpers.ExtractPokemon(entryEvolvesFrom!.Text);
            if (creatureIndex.HasValue)
            {
                pokemon.EvolvesFrom = creatureIndex.Value;
                entryEvolvesFrom!.Text = AutocompleteHelpers.FormatPokemon(rom, creatureIndex.Value);
            }
        }

        private void OnMegaEvo1Changed(object sender, EventArgs args)
        {
            var creatureIndex = AutocompleteHelpers.ExtractPokemon(entryMegaEvo1!.Text);
            if (creatureIndex.HasValue)
            {
                pokemon.MegaEvolutions = (creatureIndex.Value, pokemon.MegaEvolutions.Second);
                entryMegaEvo1!.Text = AutocompleteHelpers.FormatPokemon(rom, creatureIndex.Value);
            }
        }

        private void OnMegaEvo2Changed(object sender, EventArgs args)
        {
            var creatureIndex = AutocompleteHelpers.ExtractPokemon(entryMegaEvo2!.Text);
            if (creatureIndex.HasValue)
            {
                pokemon.MegaEvolutions = (pokemon.MegaEvolutions.First, creatureIndex.Value);
                entryMegaEvo2!.Text = AutocompleteHelpers.FormatPokemon(rom, creatureIndex.Value);
            }
        }

        private void OnEvolutionSpeciesEditingStarted(object sender, EditingStartedArgs args)
        {
            (args.Editable as Entry)!.Completion = evolutionListCompletion;
        }

        private void OnEvolutionSpeciesEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (evolutionsStore!.GetIter(out TreeIter iter, path))
            {
                var creatureIndex = AutocompleteHelpers.ExtractPokemon(args.NewText);
                if (creatureIndex.HasValue)
                {
                    int index = (int) evolutionsStore.GetValue(iter, EvolutionIndexColumn);
                    evolutionsStore.SetValue(iter, EvolutionCreatureIndexColumn, (int) creatureIndex.Value);
                    evolutionsStore.SetValue(iter, EvolutionNameColumn,
                        AutocompleteHelpers.FormatPokemon(rom!, creatureIndex.Value));
                    
                    pokemon.EvolutionBranches[index].Evolution = creatureIndex.Value;
                }
            }
        }

        private void OnEvolutionSelectionChanged(object sender, EventArgs args)
        {
            var selection = (TreeSelection) sender;
            if (selection.GetSelected(out ITreeModel model, out TreeIter iter))
            {
                var evolutionIndex = (int) model.GetValue(iter, EvolutionIndexColumn);
                selectedEvolution = pokemon.EvolutionBranches[evolutionIndex];

                ShowEvolutionRequirements();
                btnRemoveEvolution!.Sensitive = true;
            }
            else
            {
                selectedEvolution = null;
                switchEvolutionLevel!.Sensitive = false;
                switchEvolutionItem!.Sensitive = false;
                entryEvolutionUnknown!.Sensitive = false;

                switchEvolutionLevel.Active = false;
                switchEvolutionItem.Active = false;
                btnRemoveEvolution!.Sensitive = false;
            }
        }

        private void OnAddEvolutionClicked(object sender, EventArgs args)
        {
           pokemon.EvolutionBranches.Add(new PokemonEvolution.PokemonEvolutionBranch());
           RefreshEvolutionList();
        }

        private void OnRemoveEvolutionClicked(object sender, EventArgs args)
        {
            if (evolutionsTreeSelection!.GetSelected(out ITreeModel model, out TreeIter iter))
            {
                var evolutionIndex = (int) model.GetValue(iter, EvolutionIndexColumn);
                pokemon.EvolutionBranches.RemoveAt(evolutionIndex);
                RefreshEvolutionList();
            }
        }

        private void ShowEvolutionRequirements()
        {
            if (selectedEvolution == null)
            {
                return;
            }

            switchEvolutionLevel!.Sensitive = true;
            switchEvolutionItem!.Sensitive = true;
            entryEvolutionUnknown!.Sensitive = true;

            switchEvolutionLevel.Active = selectedEvolution.HasMinimumLevel;
            switchEvolutionItem.Active = selectedEvolution.RequiresItem;
            entryEvolutionLevel!.Text = selectedEvolution.MinimumLevel.ToString();
            entryEvolutionItem!.Text = AutocompleteHelpers.FormatItem(rom, selectedEvolution.EvolutionItem);
            entryEvolutionItemAmount!.Text = selectedEvolution.ItemsRequired.ToString();
            entryEvolutionUnknown!.Text = selectedEvolution.Int00.ToString();
        }

        [GLib.ConnectBefore]
        private void OnEvolutionLevelStateSet(object sender, StateSetArgs args)
        {
            if (selectedEvolution == null)
            {
                return;
            }

            bool state = switchEvolutionLevel!.Active;
            selectedEvolution.RequirementFlags = selectedEvolution.RequirementFlags
                .SetFlag(PokemonEvolution.Requirements.Level, state);

            entryEvolutionLevel!.Sensitive = state;
        }

        [GLib.ConnectBefore]
        private void OnEvolutionItemStateSet(object sender, StateSetArgs args)
        {
            if (selectedEvolution == null)
            {
                return;
            }

            bool state = switchEvolutionItem!.Active;
            selectedEvolution.RequirementFlags = selectedEvolution.RequirementFlags
                .SetFlag(PokemonEvolution.Requirements.Item, state);

            boxItemIndexAndAmount!.Sensitive = state;
        }

        private void OnEvolutionLevelChanged(object sender, EventArgs args)
        {
            if (selectedEvolution == null)
            {
                return;
            }

            if (byte.TryParse(entryEvolutionLevel!.Text, out byte value))
            {
                selectedEvolution.MinimumLevel = value;
            }
            else if (!string.IsNullOrEmpty(entryEvolutionLevel!.Text))
            {
                entryEvolutionLevel!.Text = selectedEvolution.MinimumLevel.ToString();
            }
        }

        private void OnEvolutionItemChanged(object sender, EventArgs args)
        {
            if (selectedEvolution == null)
            {
                return;
            }

            var itemIndex = AutocompleteHelpers.ExtractItem(entryEvolutionItem!.Text);
            if (itemIndex.HasValue)
            {
                selectedEvolution.EvolutionItem = itemIndex.Value;
            }
        }

        private void OnEvolutionItemAmountChanged(object sender, EventArgs args)
        {
            if (selectedEvolution == null)
            {
                return;
            }

            if (short.TryParse(entryEvolutionItemAmount!.Text, out short value))
            {
                selectedEvolution.ItemsRequired = value;
            }
            else if (!string.IsNullOrEmpty(entryEvolutionItemAmount!.Text))
            {
                entryEvolutionItemAmount!.Text = selectedEvolution.ItemsRequired.ToString();
            }
        }

        private void OnEvolutionUnknownChanged(object sender, EventArgs args)
        {
            if (selectedEvolution == null)
            {
                return;
            }

            if (int.TryParse(entryEvolutionUnknown!.Text, out int value))
            {
                selectedEvolution.Int00 = value;
            }
            else if (!string.IsNullOrEmpty(entryEvolutionUnknown!.Text))
            {
                entryEvolutionUnknown!.Text = selectedEvolution.Int00.ToString();
            }
        }
    }
}
