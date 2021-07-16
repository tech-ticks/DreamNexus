using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    class StartersController : Widget
    {
        [UI] private ListStore? startersStore;
        [UI] private Entry? defaultPlayerName;
        [UI] private Entry? defaultPartnerName;
        [UI] private Entry? defaultTeamName;

        [UI] private ListStore? creaturesStore;
        [UI] private ListStore? movesStore;
        [UI] private EntryCompletion? creaturesCompletion;
        [UI] private EntryCompletion? movesCompletion;

        private IRtdxRom? rom;
        private IStarterCollection starters;

        private const int IndexColumn = 0;
        private const int CreatureIdColumn = 1;
        private const int Move1Column = 4;
        private const int Move2Column = 5;
        private const int Move3Column = 6;
        private const int Move4Column = 7;

        public StartersController(IRtdxRom rom) : this(new Builder("Starters.glade"), rom)
        {
        }

        private StartersController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.rom = rom;
            this.starters = rom.GetStarters();
            for (int i = 0; i < starters.Starters.Length; i++)
            {
                var starter = starters.Starters[i];
                startersStore!.AppendValues(i,
                    AutocompleteHelpers.FormatPokemon(rom, starter.PokemonId),
                    starter.MaleNature.ToString(),
                    starter.FemaleNature.ToString(),
                    AutocompleteHelpers.FormatMove(rom, starter.Move1),
                    AutocompleteHelpers.FormatMove(rom, starter.Move2),
                    AutocompleteHelpers.FormatMove(rom, starter.Move3),
                    AutocompleteHelpers.FormatMove(rom, starter.Move4)
                );
            }

            defaultPlayerName!.Text = starters.HeroName;
            defaultPartnerName!.Text = starters.PartnerName;
            defaultTeamName!.Text = starters.TeamName;

            creaturesStore!.AppendAll(AutocompleteHelpers.GetPokemon(rom));
            movesStore!.AppendAll(AutocompleteHelpers.GetMoves(rom));
        }

        private void OnDefaultPlayerSpeciesChanged(object sender, EventArgs args)
        {

        }

        private void OnDefaultPlayerNameChanged(object sender, EventArgs args)
        {

        }

        private void OnDefaultPlayerGenderChanged(object sender, EventArgs args)
        {

        }

        private void OnDefaultPartnerSpeciesChanged(object sender, EventArgs args)
        {

        }

        private void OnDefaultPartnerNameChanged(object sender, EventArgs args)
        {

        }

        private void OnDefaultPartnerGenderChanged(object sender, EventArgs args)
        {

        }

        private void OnPlayerSpeciesEdited(object sender, EventArgs args)
        {

        }

        private void OnDefaultTeamNameChanged(object sender, EventArgs args)
        {

        }

        private void OnStarterNameLabelEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = creaturesCompletion;
        }

        private void OnStarterMoveEditingStarted(object sender, EditingStartedArgs args)
        {
            var editable = (Entry) args.Editable;
            editable.Completion = movesCompletion;
        }

        private void OnStarterNameLabelEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (startersStore!.GetIter(out TreeIter iter, path))
            {
                var creatureIndex = AutocompleteHelpers.ExtractPokemon(args.NewText);
                if (creatureIndex.HasValue)
                {
                    startersStore.SetValue(iter, CreatureIdColumn,
                    AutocompleteHelpers.FormatPokemon(rom!, creatureIndex.Value));
                    starters.Starters[path.Indices[0]].PokemonId = creatureIndex.Value;
                }
            }
        }

        private void OnStarterMove1Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (startersStore!.GetIter(out TreeIter iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    startersStore.SetValue(iter, Move1Column,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    starters.Starters[path.Indices[0]].Move1 = moveIndex.Value;
                }
            }
        }

        private void OnStarterMove2Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (startersStore!.GetIter(out TreeIter iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    startersStore.SetValue(iter, Move2Column,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    starters.Starters[path.Indices[0]].Move2 = moveIndex.Value;
                }
            }
        }

        private void OnStarterMove3Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (startersStore!.GetIter(out TreeIter iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    startersStore.SetValue(iter, Move3Column,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    starters.Starters[path.Indices[0]].Move3 = moveIndex.Value;
                }
            }
        }

        private void OnStarterMove4Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (startersStore!.GetIter(out TreeIter iter, path))
            {
                var moveIndex = AutocompleteHelpers.ExtractMove(args.NewText);
                if (moveIndex.HasValue)
                {
                    startersStore.SetValue(iter, Move4Column,
                    AutocompleteHelpers.FormatMove(rom!, moveIndex.Value));
                    starters.Starters[path.Indices[0]].Move4 = moveIndex.Value;
                }
            }
        }
    }
}
