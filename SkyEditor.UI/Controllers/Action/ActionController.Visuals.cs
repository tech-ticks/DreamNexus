using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using Entry = Gtk.Entry;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using System.Linq;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    partial class ActionController : Widget
    {
        [UI] private SpinButton? sbAllyInvokeGfxIndex;
        [UI] private SpinButton? sbEnemyInvokeGfxIndex;
        [UI] private SpinButton? sbUserGfxIndex;
        [UI] private SpinButton? sbAreaGfxIndex;
        [UI] private SpinButton? sbImpactGfxIndex;
        [UI] private SpinButton? sbProjectileGfxIndex;
        [UI] private SpinButton? sbProjectileImpactGfxIndex;
        [UI] private SpinButton? sbEffectShort1EIndex;
        [UI] private SpinButton? sbAllyInvokeSfxIndex;
        [UI] private SpinButton? sbEnemyInvokeSfxIndex;
        [UI] private SpinButton? sbInitiateSfxIndex;
        [UI] private SpinButton? sbImpactSfxIndex;
        [UI] private SpinButton? sbFireProjectileSfxIndex;

        [UI] private Entry? entryAllyInvokeGfxString;
        [UI] private Entry? entryEnemyInvokeGfxString;
        [UI] private Entry? entryUserGfxString;
        [UI] private Entry? entryAreaGfxString;
        [UI] private Entry? entryImpactGfxString;
        [UI] private Entry? entryProjectileGfxString;
        [UI] private Entry? entryProjectileImpactGfxString;
        [UI] private Entry? entryEffectShort1EString;
        [UI] private Entry? entryAllyInvokeSfxString;
        [UI] private Entry? entryEnemyInvokeSfxString;
        [UI] private Entry? entryInitiateSfxString;
        [UI] private Entry? entryImpactSfxString;
        [UI] private Entry? entryFireProjectileSfxString;

        [UI] private Entry? entryEffectByte00;
        [UI] private Entry? entryEffectByte01;
        [UI] private Entry? entryEffectShort02;
        [UI] private Entry? entryEffectFloat04;
        [UI] private Entry? entryEffectFloat08;
        [UI] private Entry? entryEffectInt0C;
        [UI] private Entry? entryEffectShort10;
        [UI] private Entry? entryEffectShort12;
        [UI] private Entry? entryEffectShort14;
        [UI] private Entry? entryEffectShort16;
        [UI] private Entry? entryEffectShort32;
        [UI] private Entry? entryEffectShort34;
        [UI] private Entry? entryEffectShort36;
        [UI] private Entry? entryEffectShort38;
        [UI] private Entry? entryEffectInt3C;

        private IEffectSymbolCollection? effectSymbols;
        private ISoundEffectSymbolCollection? soundEffectSymbols;

        public void LoadVisualsTab()
        {
            effectSymbols = rom.GetEffectSymbolCollection();
            soundEffectSymbols = rom.GetSoundEffectSymbolCollection();
            int maxEffectSymbolIndex = effectSymbols.Entries.Count - 1;
            int maxSoundEffectSymbolIndex = soundEffectSymbols.Entries.Count - 1;

            sbAllyInvokeGfxIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbAllyInvokeGfxIndex!.Value = action.AllyInvokeGfxSymbol;

            sbEnemyInvokeGfxIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbEnemyInvokeGfxIndex!.Value = action.EnemyInvokeGfxSymbol;

            sbUserGfxIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbUserGfxIndex!.Value = action.UserGfxSymbol;

            sbAreaGfxIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbAreaGfxIndex!.Value = action.AreaGfxSymbol;

            sbImpactGfxIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbImpactGfxIndex!.Value = action.ImpactGfxSymbol;

            sbProjectileGfxIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbProjectileGfxIndex!.Value = action.ProjectileGfxSymbol;

            sbProjectileImpactGfxIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbProjectileImpactGfxIndex!.Value = action.ProjectileImpactGfxSymbol;

            sbEffectShort1EIndex!.Adjustment!.Upper = maxEffectSymbolIndex;
            sbEffectShort1EIndex!.Value = action.ActEffectDataInfoShort1E;

            sbAllyInvokeSfxIndex!.Adjustment!.Upper = maxSoundEffectSymbolIndex;
            sbAllyInvokeSfxIndex!.Value = action.AllyInvokeSfxSymbol;

            sbEnemyInvokeSfxIndex!.Adjustment!.Upper = maxSoundEffectSymbolIndex;
            sbEnemyInvokeSfxIndex!.Value = action.EnemyInvokeSfxSymbol;

            sbInitiateSfxIndex!.Adjustment!.Upper = maxSoundEffectSymbolIndex;
            sbInitiateSfxIndex!.Value = action.InitiateSfxSymbol;

            sbImpactSfxIndex!.Adjustment!.Upper = maxSoundEffectSymbolIndex;
            sbImpactSfxIndex!.Value = action.ImpactSfxSymbol;

            sbFireProjectileSfxIndex!.Adjustment!.Upper = maxSoundEffectSymbolIndex;
            sbFireProjectileSfxIndex!.Value = action.FireProjectileSfxSymbol;

            entryEffectByte00!.Text = action.ActEffectDataInfoByte00.ToString();
            entryEffectByte01!.Text = action.ActEffectDataInfoByte01.ToString();
            entryEffectShort02!.Text = action.ActEffectDataInfoShort02.ToString();
            entryEffectFloat04!.Text = action.ActEffectDataInfoFloat04.ToString();
            entryEffectFloat08!.Text = action.ActEffectDataInfoFloat08.ToString();
            entryEffectInt0C!.Text = action.ActEffectDataInfoInt0C.ToString();
            entryEffectShort10!.Text = action.ActEffectDataInfoShort10.ToString();
            entryEffectShort12!.Text = action.ActEffectDataInfoShort12.ToString();
            entryEffectShort14!.Text = action.ActEffectDataInfoShort14.ToString();
            entryEffectShort16!.Text = action.ActEffectDataInfoShort16.ToString();
            entryEffectShort32!.Text = action.ActEffectDataInfoShort32.ToString();
            entryEffectShort34!.Text = action.ActEffectDataInfoShort34.ToString();
            entryEffectShort36!.Text = action.ActEffectDataInfoShort36.ToString();
            entryEffectShort38!.Text = action.ActEffectDataInfoShort38.ToString();
            entryEffectInt3C!.Text = action.ActEffectDataInfoInt3C.ToString();
        }

        private void OnAllyInvokeGfxIndexChanged(object sender, EventArgs args)
        {
            action.AllyInvokeGfxSymbol = (ushort)sbAllyInvokeGfxIndex!.ValueAsInt;
            entryAllyInvokeGfxString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbAllyInvokeGfxIndex!.ValueAsInt);
            entryAllyInvokeGfxString.Sensitive = sbAllyInvokeGfxIndex!.ValueAsInt > 0;
        }

        private void OnEnemyInvokeGfxIndexChanged(object sender, EventArgs args)
        {
            action.EnemyInvokeGfxSymbol = (ushort)sbEnemyInvokeGfxIndex!.ValueAsInt;
            entryEnemyInvokeGfxString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbEnemyInvokeGfxIndex!.ValueAsInt);
            entryEnemyInvokeGfxString.Sensitive = sbEnemyInvokeGfxIndex!.ValueAsInt > 0;
        }

        private void OnUserGfxIndexChanged(object sender, EventArgs args)
        {
            action.UserGfxSymbol = (ushort)sbUserGfxIndex!.ValueAsInt;
            entryUserGfxString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbUserGfxIndex!.ValueAsInt);
            entryUserGfxString.Sensitive = sbUserGfxIndex!.ValueAsInt > 0;
        }

        private void OnAreaGfxIndexChanged(object sender, EventArgs args)
        {
            action.AreaGfxSymbol = (ushort)sbAreaGfxIndex!.ValueAsInt;
            entryAreaGfxString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbAreaGfxIndex!.ValueAsInt);
            entryAreaGfxString.Sensitive = sbAreaGfxIndex!.ValueAsInt > 0;
        }

        private void OnImpactGfxIndexChanged(object sender, EventArgs args)
        {
            action.ImpactGfxSymbol = (ushort)sbImpactGfxIndex!.ValueAsInt;
            entryImpactGfxString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbImpactGfxIndex!.ValueAsInt);
            entryImpactGfxString.Sensitive = sbImpactGfxIndex!.ValueAsInt > 0;
        }

        private void OnProjectileGfxIndexChanged(object sender, EventArgs args)
        {
            action.ProjectileGfxSymbol = (ushort)sbProjectileGfxIndex!.ValueAsInt;
            entryProjectileGfxString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbProjectileGfxIndex!.ValueAsInt);
            entryProjectileGfxString.Sensitive = sbProjectileGfxIndex!.ValueAsInt > 0;
        }

        private void OnProjectileImpactGfxIndexChanged(object sender, EventArgs args)
        {
            action.ProjectileImpactGfxSymbol = (ushort)sbProjectileImpactGfxIndex!.ValueAsInt;
            entryProjectileImpactGfxString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbProjectileImpactGfxIndex!.ValueAsInt);
            entryProjectileImpactGfxString.Sensitive = sbProjectileImpactGfxIndex!.ValueAsInt > 0;
        }

        private void OnEffectShort1EIndexChanged(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort1E = (ushort)sbEffectShort1EIndex!.ValueAsInt;
            entryEffectShort1EString!.Text = effectSymbols!.Entries.ElementAtOrDefault(sbEffectShort1EIndex!.ValueAsInt);
            entryEffectShort1EString.Sensitive = sbEffectShort1EIndex!.ValueAsInt > 0;
        }

        private void OnAllyInvokeSfxIndexChanged(object sender, EventArgs args)
        {
            action.AllyInvokeSfxSymbol = (ushort)sbAllyInvokeSfxIndex!.ValueAsInt;
            entryAllyInvokeSfxString!.Text = soundEffectSymbols!.Entries.ElementAtOrDefault(sbAllyInvokeSfxIndex!.ValueAsInt);
            entryAllyInvokeSfxString.Sensitive = sbAllyInvokeSfxIndex!.ValueAsInt > 0;
        }

        private void OnEnemyInvokeSfxIndexChanged(object sender, EventArgs args)
        {
            action.EnemyInvokeSfxSymbol = (ushort)sbEnemyInvokeSfxIndex!.ValueAsInt;
            entryEnemyInvokeSfxString!.Text = soundEffectSymbols!.Entries.ElementAtOrDefault(sbEnemyInvokeSfxIndex!.ValueAsInt);
            entryEnemyInvokeSfxString.Sensitive = sbEnemyInvokeSfxIndex!.ValueAsInt > 0;
        }

        private void OnInitiateSfxIndexChanged(object sender, EventArgs args)
        {
            action.InitiateSfxSymbol = (ushort)sbInitiateSfxIndex!.ValueAsInt;
            entryInitiateSfxString!.Text = soundEffectSymbols!.Entries.ElementAtOrDefault(sbInitiateSfxIndex!.ValueAsInt);
            entryInitiateSfxString.Sensitive = sbInitiateSfxIndex!.ValueAsInt > 0;
        }

        private void OnImpactSfxIndexChanged(object sender, EventArgs args)
        {
            action.ImpactSfxSymbol = (ushort)sbImpactSfxIndex!.ValueAsInt;
            entryImpactSfxString!.Text = soundEffectSymbols!.Entries.ElementAtOrDefault(sbImpactSfxIndex!.ValueAsInt);
            entryImpactSfxString.Sensitive = sbImpactSfxIndex!.ValueAsInt > 0;
        }

        private void OnFireProjectileSfxIndexChanged(object sender, EventArgs args)
        {
            action.FireProjectileSfxSymbol = (ushort)sbFireProjectileSfxIndex!.ValueAsInt;
            entryFireProjectileSfxString!.Text = soundEffectSymbols!.Entries.ElementAtOrDefault(sbFireProjectileSfxIndex!.ValueAsInt);
            entryFireProjectileSfxString.Sensitive = sbFireProjectileSfxIndex!.ValueAsInt > 0;
        }

        private void OnAllyInvokeGfxStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbAllyInvokeGfxIndex!.ValueAsInt] = entryAllyInvokeGfxString!.Text;
        }

        private void OnEnemyInvokeGfxStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbEnemyInvokeGfxIndex!.ValueAsInt] = entryEnemyInvokeGfxString!.Text;
        }

        private void OnUserGfxStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbUserGfxIndex!.ValueAsInt] = entryUserGfxString!.Text;
        }

        private void OnAreaGfxStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbAreaGfxIndex!.ValueAsInt] = entryAreaGfxString!.Text;
        }

        private void OnImpactGfxStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbImpactGfxIndex!.ValueAsInt] = entryImpactGfxString!.Text;
        }

        private void OnProjectileGfxStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbProjectileGfxIndex!.ValueAsInt] = entryProjectileGfxString!.Text;
        }

        private void OnProjectileImpactGfxStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbProjectileImpactGfxIndex!.ValueAsInt] = entryProjectileImpactGfxString!.Text;
        }

        private void OnEffectShort1EStringChanged(object sender, EventArgs args)
        {
            effectSymbols!.Entries[sbEffectShort1EIndex!.ValueAsInt] = entryEffectShort1EString!.Text;
        }

        private void OnAllyInvokeSfxStringChanged(object sender, EventArgs args)
        {
            soundEffectSymbols!.Entries[sbAllyInvokeSfxIndex!.ValueAsInt] = entryAllyInvokeSfxString!.Text;
        }

        private void OnEnemyInvokeSfxStringChanged(object sender, EventArgs args)
        {
            soundEffectSymbols!.Entries[sbEnemyInvokeSfxIndex!.ValueAsInt] = entryEnemyInvokeSfxString!.Text;
        }

        private void OnInitiateSfxStringChanged(object sender, EventArgs args)
        {
            soundEffectSymbols!.Entries[sbInitiateSfxIndex!.ValueAsInt] = entryInitiateSfxString!.Text;
        }

        private void OnImpactSfxStringChanged(object sender, EventArgs args)
        {
            soundEffectSymbols!.Entries[sbImpactSfxIndex!.ValueAsInt] = entryImpactSfxString!.Text;
        }

        private void OnFireProjectileSfxStringChanged(object sender, EventArgs args)
        {
            soundEffectSymbols!.Entries[sbFireProjectileSfxIndex!.ValueAsInt] = entryFireProjectileSfxString!.Text;
        }

        private void OnEffectByte00Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoByte00 = entryEffectByte00!.ParseByte(action.ActEffectDataInfoByte00);
        }

        private void OnEffectByte01Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoByte01 = entryEffectByte01!.ParseByte(action.ActEffectDataInfoByte01);
        }

        private void OnEffectShort02Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort02 = entryEffectShort02!.ParseUShort(action.ActEffectDataInfoShort02);
        }

        private void OnEffectFloat04Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoFloat04 = entryEffectFloat04!.ParseFloat(action.ActEffectDataInfoFloat04);
        }

        private void OnEffectFloat08Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoFloat08 = entryEffectFloat08!.ParseFloat(action.ActEffectDataInfoFloat08);
        }

        private void OnEffectInt0CChanged(object sender, EventArgs args)
        {
            action.ActEffectDataInfoInt0C = entryEffectInt0C!.ParseInt(action.ActEffectDataInfoInt0C);
        }

        private void OnEffectShort10Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort10 = entryEffectShort10!.ParseUShort(action.ActEffectDataInfoShort10);
        }

        private void OnEffectShort12Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort12 = entryEffectShort12!.ParseUShort(action.ActEffectDataInfoShort12);
        }

        private void OnEffectShort14Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort14 = entryEffectShort14!.ParseUShort(action.ActEffectDataInfoShort14);
        }

        private void OnEffectShort16Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort16 = entryEffectShort16!.ParseUShort(action.ActEffectDataInfoShort16);
        }

        private void OnEffectShort32Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort32 = entryEffectShort32!.ParseUShort(action.ActEffectDataInfoShort32);
        }

        private void OnEffectShort34Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort34 = entryEffectShort34!.ParseUShort(action.ActEffectDataInfoShort34);
        }

        private void OnEffectShort36Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort36 = entryEffectShort36!.ParseUShort(action.ActEffectDataInfoShort36);
        }

        private void OnEffectShort38Changed(object sender, EventArgs args)
        {
            action.ActEffectDataInfoShort38 = entryEffectShort38!.ParseUShort(action.ActEffectDataInfoShort38);
        }

        private void OnEffectInt3CChanged(object sender, EventArgs args)
        {
            action.ActEffectDataInfoInt3C = entryEffectInt3C!.ParseInt(action.ActEffectDataInfoInt3C);
        }
    }
}
