using System;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using UI = Gtk.Builder.ObjectAttribute;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private Entry? entryBaseHp;
        [UI] private Entry? entryBaseAtk;
        [UI] private Entry? entryBaseSpAtk;
        [UI] private Entry? entryBaseDef;
        [UI] private Entry? entryBaseSpDef;
        [UI] private Entry? entryBaseSpeed;

        private void LoadBaseStatsTab()
        {
            entryBaseHp!.Text = pokemon.BaseHitPoints.ToString();
            entryBaseAtk!.Text = pokemon.BaseAttack.ToString();
            entryBaseSpAtk!.Text = pokemon.BaseSpecialAttack.ToString();
            entryBaseDef!.Text = pokemon.BaseDefense.ToString();
            entryBaseSpDef!.Text = pokemon.BaseSpecialDefense.ToString();
            entryBaseSpeed!.Text = pokemon.BaseSpeed.ToString();
        }

        private void OnBaseHpChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBaseHp!.Text, out short value))
            {
                pokemon.BaseHitPoints = value;
            }
            else if (!string.IsNullOrEmpty(entryBaseHp!.Text))
            {
                entryBaseHp!.Text = pokemon.BaseHitPoints.ToString();
            }
        }

        private void OnBaseAtkChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBaseAtk!.Text, out short value))
            {
                pokemon.BaseAttack = value;
            }
            else if (!string.IsNullOrEmpty(entryBaseAtk!.Text))
            {
                entryBaseAtk!.Text = pokemon.BaseAttack.ToString();
            }
        }

        private void OnBaseSpAtkChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBaseSpAtk!.Text, out short value))
            {
                pokemon.BaseSpecialAttack = value;
            }
            else if (!string.IsNullOrEmpty(entryBaseSpAtk!.Text))
            {
                entryBaseSpAtk!.Text = pokemon.BaseSpecialAttack.ToString();
            }
        }

        private void OnBaseDefChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBaseDef!.Text, out short value))
            {
                pokemon.BaseDefense = value;
            }
            else if (!string.IsNullOrEmpty(entryBaseDef!.Text))
            {
                entryBaseDef!.Text = pokemon.BaseDefense.ToString();
            }
        }

        private void OnBaseSpDefChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBaseSpDef!.Text, out short value))
            {
                pokemon.BaseSpecialDefense = value;
            }
            else if (!string.IsNullOrEmpty(entryBaseSpDef!.Text))
            {
                entryBaseSpDef!.Text = pokemon.BaseSpecialDefense.ToString();
            }
        }

        private void OnBaseSpeedChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBaseSpeed!.Text, out short value))
            {
                pokemon.BaseSpeed = value;
            }
            else if (!string.IsNullOrEmpty(entryBaseSpeed!.Text))
            {
                entryBaseSpeed!.Text = pokemon.BaseSpeed.ToString();
            }
        }
    }
}
