using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;

namespace SkyEditorUI.Controllers
{
    class StartersController : Widget
    {
        [UI] private ListStore? startersStore;
        [UI] private ListStore? defaultStartersStore;
        [UI] private Entry? defaultPlayerSpecies;
        [UI] private ComboBox? defaultPlayerGender;
        [UI] private Entry? defaultPlayerName;
        [UI] private Entry? defaultPartnerSpecies;
        [UI] private ComboBox? defaultPartnerGender;
        [UI] private Entry? defaultPartnerName;
        [UI] private Entry? defaultTeamName;
        [UI] private ComboBoxText? cbNameLanguage;

        [UI] private ListStore? creaturesStore;
        [UI] private ListStore? movesStore;
        [UI] private EntryCompletion? creaturesCompletion;
        [UI] private EntryCompletion? movesCompletion;

        private IRtdxRom? rom;
        private IStarterCollection starters;
        private LocalizedStringCollection? strings;

        private const int IndexColumn = 0;
        private const int CreatureIdColumn = 1;
        private const int Move1Column = 4;
        private const int Move2Column = 5;
        private const int Move3Column = 6;
        private const int Move4Column = 7;

        public StartersController(IRtdxRom rom, Modpack modpack) : this(new Builder("Starters.glade"), rom, modpack)
        {
        }

        private StartersController(Builder builder, IRtdxRom rom, Modpack modpack)
            : base(builder.GetRawOwnedObject("main"))
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

            // Load starters directly from the rom to figure out which ones are used by default
            var defaultStarters = rom.GetMainExecutable().StarterFixedPokemonMaps;
            foreach (var starter in defaultStarters)
            {
                defaultStartersStore!.AppendValues(AutocompleteHelpers.FormatPokemon(rom, starter.PokemonId));
            }

            defaultPlayerSpecies!.Text = AutocompleteHelpers.FormatPokemon(rom, starters.HeroCreature);
            defaultPartnerSpecies!.Text = AutocompleteHelpers.FormatPokemon(rom, starters.PartnerCreature);
            defaultPlayerGender!.Active = (int) starters.HeroGender + 1; // Starts at -1
            defaultPartnerGender!.Active = (int) starters.PartnerGender + 1;

            creaturesStore!.AppendAll(AutocompleteHelpers.GetPokemon(rom));
            movesStore!.AppendAll(AutocompleteHelpers.GetMoves(rom));

            if (!modpack.Metadata.EnableCodeInjection)
            {
                defaultPlayerSpecies!.Sensitive = false;
                defaultPlayerGender!.Sensitive = false;
                defaultPlayerName!.Sensitive = false;
                defaultPartnerSpecies!.Sensitive = false;
                defaultPartnerGender!.Sensitive = false;
                defaultPartnerName!.Sensitive = false;
            }

            LoadNames(LanguageType.EN);
        }

        private void OnDefaultPlayerSpeciesChanged(object sender, EventArgs args)
        {
            var creatureIndex = AutocompleteHelpers.ExtractPokemon(defaultPlayerSpecies!.Text);
            if (creatureIndex.HasValue)
            {
                starters.HeroCreature = creatureIndex.Value;
                defaultPlayerSpecies!.Text = AutocompleteHelpers.FormatPokemon(rom!, creatureIndex.Value);
            }
        }

        private void OnDefaultPlayerNameChanged(object sender, EventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(defaultPlayerName!.Text))
            {
                strings!.SetCommonString(TextIDHash.DEBUG_MENU__DEBUG_HERO_NAME, defaultPlayerName!.Text);
            }
        }

        private void OnDefaultPlayerGenderChanged(object sender, EventArgs args)
        {
            starters.HeroGender = (PokemonGenderType) defaultPlayerGender!.Active - 1; // Starts at -1
        }

        private void OnDefaultPartnerSpeciesChanged(object sender, EventArgs args)
        {
            var creatureIndex = AutocompleteHelpers.ExtractPokemon(defaultPartnerSpecies!.Text);
            if (creatureIndex.HasValue)
            {
                starters.PartnerCreature = creatureIndex.Value;
                defaultPartnerSpecies!.Text = AutocompleteHelpers.FormatPokemon(rom!, creatureIndex.Value);
            }
        }

        private void OnDefaultPartnerNameChanged(object sender, EventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(defaultPartnerName!.Text))
            {
                strings!.SetCommonString(TextIDHash.DEBUG_MENU__DEBUG_PARTNER_NAME, defaultPartnerName!.Text);
            }
        }

        private void OnDefaultPartnerGenderChanged(object sender, EventArgs args)
        {
            starters.PartnerGender = (PokemonGenderType) defaultPartnerGender!.Active - 1; // Starts at -1
        }

        private void OnDefaultTeamNameChanged(object sender, EventArgs args)
        {
            if (!string.IsNullOrWhiteSpace(defaultTeamName!.Text))
            {
                strings!.SetCommonString(TextIDHash.GAME_MENU__DEFAULT_TEAM_NAME, defaultTeamName!.Text);
            }
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
            if (startersStore!.GetIter(out var iter, path))
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
            if (startersStore!.GetIter(out var iter, path))
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
            if (startersStore!.GetIter(out var iter, path))
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
            if (startersStore!.GetIter(out var iter, path))
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
            if (startersStore!.GetIter(out var iter, path))
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

        private void OnNameLanguageChanged(object sender, EventArgs args)
        {
            LoadNames((LanguageType) cbNameLanguage!.Active);
        }

        private void LoadNames(LanguageType language)
        {
            strings = rom!.GetStrings().GetStringsForLanguage(language);

            defaultPlayerName!.Text = strings!.GetCommonString(TextIDHash.DEBUG_MENU__DEBUG_HERO_NAME);
            defaultPartnerName!.Text = strings!.GetCommonString(TextIDHash.DEBUG_MENU__DEBUG_PARTNER_NAME);
            defaultTeamName!.Text = strings!.GetCommonString(TextIDHash.GAME_MENU__DEFAULT_TEAM_NAME);
        }
    }
}
