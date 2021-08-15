using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private Label? labelIdName;
        [UI] private Entry? entryPokedexNum;
        [UI] private ComboBox? cbTypePrimary;
        [UI] private ComboBox? cbTypeSecondary;
        [UI] private ListStore? creatureCompletionStore;
        [UI] private ListStore? itemCompletionStore;
        [UI] private ListStore? typesStore;

        private LocalizedStringCollection englishStrings;
        private LocalizedStringCollection japaneseStrings;
        private LocalizedStringCollection frenchStrings;
        private LocalizedStringCollection germanStrings;
        private LocalizedStringCollection italianStrings;
        private LocalizedStringCollection spanishStrings;

        private PokemonModel pokemon;
        private IRtdxRom rom;
        private Modpack modpack;
        private Builder builder;

        public PokemonController(IRtdxRom rom, Modpack modpack, ControllerContext context)
            : this(new Builder("Pokemon.glade"), rom, modpack, context)
        {
        }

        private PokemonController(Builder builder, IRtdxRom rom, Modpack modpack, ControllerContext context) 
            : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);
            
            var pokemonId = (context as PokemonControllerContext)!.Index;
            this.pokemon = rom.GetPokemon().GetPokemonById(pokemonId)
                ?? throw new ArgumentException("Pokemon from context ID is null", nameof(context));
            this.rom = rom;
            this.modpack = modpack;
            this.builder = builder;

            this.englishStrings = rom.GetStrings().English;
            this.japaneseStrings = rom.GetStrings().Japanese;
            this.frenchStrings = rom.GetStrings().French;
            this.germanStrings = rom.GetStrings().German;
            this.italianStrings = rom.GetStrings().Italian;
            this.spanishStrings = rom.GetStrings().Spanish;

            for (PokemonType i = 0; i <= PokemonType.NASHI; i++)
            {
                typesStore!.AppendValues((int) i, englishStrings.GetPokemonTypeName(i) ?? $"({i.ToString()})");
            }

            creatureCompletionStore!.AppendAll(AutocompleteHelpers.GetPokemon(rom));
            itemCompletionStore!.AppendAll(AutocompleteHelpers.GetItems(rom));

            string formattedId = ((int) pokemonId).ToString("0000");
            string? name = englishStrings.GetPokemonName(pokemonId);
            labelIdName!.Text = $"#{formattedId}: {name} ({pokemonId.ToString()})";
            entryPokedexNum!.Text = pokemon.PokedexNumber.ToString();
            cbTypePrimary!.Active = (int) pokemon.Type1;
            cbTypeSecondary!.Active = (int) pokemon.Type2;

            LoadNameTab();
            LoadGeneralTab();
            LoadBaseStatsTab();
            LoadEvolutionTab();
            LoadOtherTab();
        }

        private void OnPokedexNumChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryPokedexNum!.Text, out short value))
            {
                pokemon.PokedexNumber = value;
            }
            else if (!string.IsNullOrEmpty(entryPokedexNum!.Text))
            {
                entryPokedexNum!.Text = pokemon.PokedexNumber.ToString();
            }
        }

        private void OnPrimaryTypeChanged(object sender, EventArgs args)
        {
            pokemon.Type1 = (PokemonType) cbTypePrimary!.Active;
        }

        private void OnSecondaryTypeChanged(object sender, EventArgs args)
        {
            pokemon.Type2 = (PokemonType) cbTypeSecondary!.Active;
        }

        private void OnImportClicked(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private void OnExportClicked(object sender, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private void OnDrawPortrait(object sender, DrawnArgs args)
        {
            // TODO: implement
        }
    }
}
