using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private SpinButton? sbCategoryId;

        [UI] private Entry? entryNameJapanese;
        [UI] private Entry? entryNameEnglish;
        [UI] private Entry? entryNameFrench;
        [UI] private Entry? entryNameGerman;
        [UI] private Entry? entryNameItalian;
        [UI] private Entry? entryNameSpanish;

        [UI] private Entry? entryCategoryJapanese;
        [UI] private Entry? entryCategoryEnglish;
        [UI] private Entry? entryCategoryFrench;
        [UI] private Entry? entryCategoryGerman;
        [UI] private Entry? entryCategoryItalian;
        [UI] private Entry? entryCategorySpanish;

        private void LoadNameTab()
        {
            sbCategoryId!.Value = pokemon.Taxon;

            entryNameJapanese!.Text = japaneseStrings.GetPokemonName(pokemon.Id);
            entryNameEnglish!.Text = englishStrings.GetPokemonName(pokemon.Id);
            entryNameFrench!.Text = frenchStrings.GetPokemonName(pokemon.Id);
            entryNameGerman!.Text = germanStrings.GetPokemonName(pokemon.Id);
            entryNameItalian!.Text = italianStrings.GetPokemonName(pokemon.Id);
            entryNameSpanish!.Text = spanishStrings.GetPokemonName(pokemon.Id);

            LoadCategoryStrings();
        }

        private void LoadCategoryStrings()
        {
            entryCategoryJapanese!.Text = japaneseStrings.GetPokemonTaxonomy(pokemon.Taxon);
            entryCategoryEnglish!.Text = englishStrings.GetPokemonTaxonomy(pokemon.Taxon);
            entryCategoryFrench!.Text = frenchStrings.GetPokemonTaxonomy(pokemon.Taxon);
            entryCategoryGerman!.Text = germanStrings.GetPokemonTaxonomy(pokemon.Taxon);
            entryCategoryItalian!.Text = italianStrings.GetPokemonTaxonomy(pokemon.Taxon);
            entryCategorySpanish!.Text = spanishStrings.GetPokemonTaxonomy(pokemon.Taxon);
        }

        private void OnCategoryIdChanged(object sender, EventArgs args)
        {
            pokemon.Taxon = (short) sbCategoryId!.ValueAsInt;
            LoadCategoryStrings();
        }

        private void OnNameJapaneseChanged(object sender, EventArgs args)
        {
            japaneseStrings.SetCommonString(japaneseStrings.GetPokemonNameHash(pokemon.Id), entryNameJapanese!.Text);
        }

        private void OnNameEnglishChanged(object sender, EventArgs args)
        {
            englishStrings.SetCommonString(englishStrings.GetPokemonNameHash(pokemon.Id), entryNameEnglish!.Text);
        }

        private void OnNameFrenchChanged(object sender, EventArgs args)
        {
            frenchStrings.SetCommonString(frenchStrings.GetPokemonNameHash(pokemon.Id), entryNameFrench!.Text);
        }

        private void OnNameGermanChanged(object sender, EventArgs args)
        {
            germanStrings.SetCommonString(germanStrings.GetPokemonNameHash(pokemon.Id), entryNameGerman!.Text);
        }

        private void OnNameItalianChanged(object sender, EventArgs args)
        {
            italianStrings.SetCommonString(italianStrings.GetPokemonNameHash(pokemon.Id), entryNameItalian!.Text);
        }

        private void OnNameSpanishChanged(object sender, EventArgs args)
        {
            spanishStrings.SetCommonString(spanishStrings.GetPokemonNameHash(pokemon.Id), entryNameSpanish!.Text);
        }

        private void OnCategoryJapaneseChanged(object sender, EventArgs args)
        {
            japaneseStrings.SetCommonString(japaneseStrings.GetPokemonTaxonomyHash(pokemon.Taxon),
                entryCategoryJapanese!.Text);
        }

        private void OnCategoryEnglishChanged(object sender, EventArgs args)
        {
            englishStrings.SetCommonString(englishStrings.GetPokemonTaxonomyHash(pokemon.Taxon),
                entryCategoryEnglish!.Text);
        }

        private void OnCategoryFrenchChanged(object sender, EventArgs args)
        {
            frenchStrings.SetCommonString(frenchStrings.GetPokemonTaxonomyHash(pokemon.Taxon),
                entryCategoryFrench!.Text);
        }

        private void OnCategoryGermanChanged(object sender, EventArgs args)
        {
            germanStrings.SetCommonString(germanStrings.GetPokemonTaxonomyHash(pokemon.Taxon),
                entryCategoryGerman!.Text);
        }

        private void OnCategoryItalianChanged(object sender, EventArgs args)
        {
            italianStrings.SetCommonString(italianStrings.GetPokemonTaxonomyHash(pokemon.Taxon),
                entryCategoryItalian!.Text);
        }

        private void OnCategorySpanishChanged(object sender, EventArgs args)
        {
            spanishStrings.SetCommonString(spanishStrings.GetPokemonTaxonomyHash(pokemon.Taxon),
                entryCategorySpanish!.Text);
        }
    }
}
