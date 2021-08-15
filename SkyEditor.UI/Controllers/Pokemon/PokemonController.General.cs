using System;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;
using UI = Gtk.Builder.ObjectAttribute;

namespace SkyEditorUI.Controllers
{
    partial class PokemonController : Widget
    {
        [UI] private ComboBox? cbAbility1;
        [UI] private ComboBox? cbAbility2;
        [UI] private ComboBox? cbHiddenAbility;
        [UI] private Entry? entryBaseRecruitRate;
        [UI] private Entry? entryBoostedRecruitRate;
        [UI] private Entry? entryRecruitPrereq;
        [UI] private Entry? entrySize;

        [UI] private ListStore? abilitiesStore;

        private void LoadGeneralTab()
        {
            for (AbilityIndex i = 0; i < AbilityIndex.ABILITY_END; i++)
            {
                abilitiesStore!.AppendValues((int) i, englishStrings.GetAbilityName(i) ?? $"({i.ToString()})");
            }

            cbAbility1!.Active = (int) pokemon.Ability1;
            cbAbility2!.Active = (int) pokemon.Ability2;
            cbHiddenAbility!.Active = (int) pokemon.HiddenAbility;
            entryBaseRecruitRate!.Text = pokemon.BaseRecruitRate.ToString();
            entryBoostedRecruitRate!.Text = pokemon.BoostedRecruitRate.ToString();
            entryRecruitPrereq!.Text = pokemon.RecruitPrereq;
            entrySize!.Text = pokemon.Size.ToString();

            for (int i = 0; i <= 15; i++)
            {
                var flagSwitch = (Switch) builder.GetObject($"switchFlag{i}");
                var flag = (PokemonDataInfo.FeatureFlags) (1 << i);
                flagSwitch.Active = pokemon.Features.HasFlag(flag);
            }
        }

        [GLib.ConnectBefore]
        private void OnFlagSet(object sender, StateSetArgs args)
        {
            var flagSwitch = (Switch) sender;

            // Extract flag from switch name
            var flagIndex = int.Parse(flagSwitch.Name.Replace("switchFlag", ""));
            var flag = (PokemonDataInfo.FeatureFlags) (1 << flagIndex);
            pokemon.Features = pokemon.Features.SetFlag(flag, flagSwitch.Active);
        }

        private void OnAbility1Changed(object sender, EventArgs args)
        {
            pokemon.Ability1 = (AbilityIndex) cbAbility1!.Active;
        }

        private void OnAbility2Changed(object sender, EventArgs args)
        {
            pokemon.Ability2 = (AbilityIndex) cbAbility2!.Active;
        }

        private void OnHiddenAbilityChanged(object sender, EventArgs args)
        {
            pokemon.HiddenAbility = (AbilityIndex) cbHiddenAbility!.Active;
        }

        private void OnBaseRecruitRateChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBaseRecruitRate!.Text, out short value))
            {
                pokemon.BaseRecruitRate = value;
            }
            else if (!string.IsNullOrEmpty(entryBaseRecruitRate!.Text))
            {
                entryBaseRecruitRate!.Text = pokemon.BaseRecruitRate.ToString();
            }
        }

        private void OnBoostedRecruitRateChanged(object sender, EventArgs args)
        {
            if (short.TryParse(entryBoostedRecruitRate!.Text, out short value))
            {
                pokemon.BoostedRecruitRate = value;
            }
            else if (!string.IsNullOrEmpty(entryBoostedRecruitRate!.Text))
            {
                entryBoostedRecruitRate!.Text = pokemon.BoostedRecruitRate.ToString();
            }
        }

        private void OnRecruitPrereqChanged(object sender, EventArgs args)
        {
            pokemon.RecruitPrereq = entryRecruitPrereq!.Text;
        }

        private void OnSizeChanged(object sender, EventArgs args)
        {
            if (byte.TryParse(entrySize!.Text, out byte value))
            {
                pokemon.Size = value;
            }
            else if (!string.IsNullOrEmpty(entrySize!.Text))
            {
                entrySize!.Text = pokemon.Size.ToString();
            }
        }

        private void OnHelpRecruitRateClicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Ok, "The values are actually percentages.\n"
                    + "10 -> 1.0%,\n"
                    + "100 -> 10.0%,\n"
                    + "1000 -> 100%, etc.\n\n"
                    + "The second recruit rate is used if the Pokémon was already recruited, the first recruit rate "
                    + "is used otherwise.");
            dialog.Title = "Recruit Rate";
            dialog.Run();
            dialog.Destroy();
        }
    }
}
