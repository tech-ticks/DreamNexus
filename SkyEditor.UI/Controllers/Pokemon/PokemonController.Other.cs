using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private Entry? entryMegaEvolutionRelated;
        [UI] private Entry? entryUnkMinLevel;
        [UI] private Entry? entryUnkMaxLevel;
        [UI] private Entry? entryShort62;
        [UI] private Entry? entryShort66;
        [UI] private Entry? entryShort6E;
        [UI] private Entry? entryShort70;
        [UI] private Entry? entryShort72;
        [UI] private Entry? entryShort80;
        [UI] private Entry? entrySByte8B;
        [UI] private Entry? entrySByte8C;
        [UI] private Entry? entrySByte8D;
        [UI] private Entry? entrySByte8E;
        [UI] private Entry? entryByte95;
        [UI] private Entry? entryByte96;

        private void LoadOtherTab()
        {
            entryMegaEvolutionRelated!.Text = pokemon.MegaRelatedProperty.ToString();
            entryUnkMinLevel!.Text = pokemon.SomeMinimumLevel.ToString();
            entryUnkMaxLevel!.Text = pokemon.SomeMaximumLevel.ToString();
            entryShort62!.Text = pokemon.Unknown62.ToString();
            entryShort66!.Text = pokemon.Unknown66.ToString();
            entryShort6E!.Text = pokemon.Unknown6E.ToString();
            entryShort70!.Text = pokemon.Unknown70.ToString();
            entryShort72!.Text = pokemon.Unknown72.ToString();
            entryShort80!.Text = pokemon.Unknown80.ToString();
            entrySByte8B!.Text = pokemon.Unknown8B.ToString();
            entrySByte8C!.Text = pokemon.Unknown8C.ToString();
            entrySByte8D!.Text = pokemon.Unknown8D.ToString();
            entrySByte8E!.Text = pokemon.Unknown8E.ToString();
            entryByte95!.Text = pokemon.Unknown95.ToString();
            entryByte96!.Text = pokemon.Unknown96.ToString();
        }

        private void OnMegaEvolutionRelatedChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryMegaEvolutionRelated!.Text, out byte value))
            {
                pokemon.MegaRelatedProperty = value;
            }
            else if (!string.IsNullOrEmpty(entryMegaEvolutionRelated!.Text))
            {
                entryMegaEvolutionRelated!.Text = pokemon.MegaRelatedProperty.ToString();
            }
        }

        private void OnUnkMinLevelChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryUnkMinLevel!.Text, out byte value))
            {
                pokemon.SomeMinimumLevel = value;
            }
            else if (!string.IsNullOrEmpty(entryUnkMinLevel!.Text))
            {
                entryUnkMinLevel!.Text = pokemon.SomeMinimumLevel.ToString();
            }
        }

        private void OnUnkMaxLevelChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entryUnkMaxLevel!.Text, out byte value))
            {
                pokemon.SomeMaximumLevel = value;
            }
            else if (!string.IsNullOrEmpty(entryUnkMaxLevel!.Text))
            {
                entryUnkMaxLevel!.Text = pokemon.SomeMaximumLevel.ToString();
            }
        }

        private void OnShort62Changed(object sender, EventArgs args)
        {
            if (ushort.TryParse(entryShort62!.Text, out ushort value))
            {
                pokemon.Unknown62 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort62!.Text))
            {
                entryShort62!.Text = pokemon.Unknown62.ToString();
            }
        }

        private void OnShort66Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort66!.Text, out short value))
            {
                pokemon.Unknown66 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort66!.Text))
            {
                entryShort66!.Text = pokemon.Unknown66.ToString();
            }
        }

        private void OnShort6EChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort6E!.Text, out short value))
            {
                pokemon.Unknown6E = value;
            }
            else if (!string.IsNullOrEmpty(entryShort6E!.Text))
            {
                entryShort6E!.Text = pokemon.Unknown6E.ToString();
            }
        }

        private void OnShort70Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort70!.Text, out short value))
            {
                pokemon.Unknown70 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort70!.Text))
            {
                entryShort70!.Text = pokemon.Unknown70.ToString();
            }
        }

        private void OnShort72Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort72!.Text, out short value))
            {
                pokemon.Unknown72 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort72!.Text))
            {
                entryShort72!.Text = pokemon.Unknown72.ToString();
            }
        }

        private void OnShort80Changed(object sender, EventArgs args)
        {
            if (short.TryParse(entryShort80!.Text, out short value))
            {
                pokemon.Unknown80 = value;
            }
            else if (!string.IsNullOrEmpty(entryShort80!.Text))
            {
                entryShort80!.Text = pokemon.Unknown80.ToString();
            }
        }

        private void OnSByte8BChanged(object sender, EventArgs args)
        {
            if (sbyte.TryParse(entrySByte8B!.Text, out sbyte value))
            {
                pokemon.Unknown8B = value;
            }
            else if (!string.IsNullOrEmpty(entrySByte8B!.Text))
            {
                entrySByte8B!.Text = pokemon.Unknown8B.ToString();
            }
        }

        private void OnSByte8CChanged(object sender, EventArgs args)
        {
            if (sbyte.TryParse(entrySByte8C!.Text, out sbyte value))
            {
                pokemon.Unknown8C = value;
            }
            else if (!string.IsNullOrEmpty(entrySByte8C!.Text))
            {
                entrySByte8C!.Text = pokemon.Unknown8C.ToString();
            }
        }

        private void OnSByte8DChanged(object sender, EventArgs args)
        {
            if (sbyte.TryParse(entrySByte8D!.Text, out sbyte value))
            {
                pokemon.Unknown8D = value;
            }
            else if (!string.IsNullOrEmpty(entrySByte8D!.Text))
            {
                entrySByte8D!.Text = pokemon.Unknown8D.ToString();
            }
        }

        private void OnSByte8EChanged(object sender, EventArgs args)
        {
            if (sbyte.TryParse(entrySByte8E!.Text, out sbyte value))
            {
                pokemon.Unknown8E = value;
            }
            else if (!string.IsNullOrEmpty(entrySByte8E!.Text))
            {
                entrySByte8E!.Text = pokemon.Unknown8E.ToString();
            }
        }

        private void OnByte95Changed(object sender, EventArgs args)
        {
            if (byte.TryParse(entryByte95!.Text, out byte value))
            {
                pokemon.Unknown95 = value;
            }
            else if (!string.IsNullOrEmpty(entryByte95!.Text))
            {
                entryByte95!.Text = pokemon.Unknown95.ToString();
            }
        }

        private void OnByte96Changed(object sender, EventArgs args)
        {
            if (byte.TryParse(entryByte96!.Text, out byte value))
            {
                pokemon.Unknown96 = value;
            }
            else if (!string.IsNullOrEmpty(entryByte96!.Text))
            {
                entryByte96!.Text = pokemon.Unknown96.ToString();
            }
        }
    }
}
