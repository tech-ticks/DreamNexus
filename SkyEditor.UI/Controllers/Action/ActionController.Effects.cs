using Gtk;
using SkyEditor.RomEditor.Infrastructure;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.ActDataInfo;
using UI = Gtk.Builder.ObjectAttribute;
using EffectTypeStrings = SkyEditor.RomEditor.Resources.Strings.EffectType;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System;
using System.Collections.Generic;
using Entry = Gtk.Entry;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    partial class ActionController : Widget
    {
        [UI] private ComboBox? cbEffectType0;
        [UI] private ComboBox? cbEffectType1;
        [UI] private ComboBox? cbEffectType2;
        [UI] private ComboBox? cbEffectType3;
        [UI] private Label? labelEffect0Parameters;
        [UI] private Label? labelEffect1Parameters;
        [UI] private Label? labelEffect2Parameters;
        [UI] private Label? labelEffect3Parameters;
        [UI] private Grid? effect0Parameters;
        [UI] private Grid? effect1Parameters;
        [UI] private Grid? effect2Parameters;
        [UI] private Grid? effect3Parameters;

        [UI] private ListStore? effectTypesStore;

        private List<EffectParameterType>[] effectParametersByIndex = new List<EffectParameterType>[8];

        public void LoadEffectsTab()
        {
            effectTypesStore!.Clear();
            for (EffectType i = (EffectType) 0; i < EffectType.Max; i++)
            {
                string? effectDescription = EffectTypeStrings.ResourceManager.GetString(i.ToString());
                if (string.IsNullOrEmpty(effectDescription))
                {
                    effectDescription = $"Unknown effect {i}";
                }
                effectTypesStore.AppendValues((int) i, effectDescription);
            }

            for (int i = 0; i < effectParametersByIndex.Length; i++)
            {
                effectParametersByIndex[i] = new List<EffectParameterType>();
                var store = builder.GetObject("paramsStore" + i) as ListStore;
                store!.AppendValues(0, "None");
            }

            var paramDataInfo = rom.GetActParamDataInfo();
            for (EffectParameterType i = (EffectParameterType) 1; i < EffectParameterType.Max; i++)
            {
                var expectedIndex = paramDataInfo.Entries[(int) i];
                var store = builder.GetObject("paramsStore" + expectedIndex) as ListStore;
                store!.AppendValues((int) i, i.GetDescription());
                effectParametersByIndex[expectedIndex].Add(i);
            }

            cbEffectType0!.Active = (int) action.Effects[0].Type;
            cbEffectType1!.Active = (int) action.Effects[1].Type;
            cbEffectType2!.Active = (int) action.Effects[2].Type;
            cbEffectType3!.Active = (int) action.Effects[3].Type;

            for (int i = 0; i < 4; i++)
            {
                ShowParameters(i);
            }
        }

        private void ShowParameters(int effectIndex)
        {
            var effect = action.Effects[effectIndex];
            for (int j = 0; j < effect.Parameters.Count; j++)
            {
                var param = effect.Parameters[j];
                var typeComboBox = builder.GetObject($"cbEffect{effectIndex}Param{j}") as ComboBox;
                if (typeComboBox != null)
                {
                    typeComboBox.Active = effectParametersByIndex[j].IndexOf(param.Type) + 1;
                }
                var valueEntry = builder.GetObject($"entryEffect{effectIndex}Param{j}") as Entry;
                if (valueEntry != null)
                {
                    valueEntry.Text = param.Value.ToString();
                }
            }
        }

        private void OnEffectType0Changed(object sender, EventArgs args)
        {
            action.Effects[0].Type = (EffectType) cbEffectType0!.Active;
            bool visible = action.Effects[0].Type != EffectType.None;
            effect0Parameters!.Visible = visible;
            labelEffect0Parameters!.Visible = visible;
            ShowParameters(0);
        }

        private void OnEffectType1Changed(object sender, EventArgs args)
        {
            action.Effects[1].Type = (EffectType) cbEffectType1!.Active;
            bool visible = action.Effects[1].Type != EffectType.None;
            effect1Parameters!.Visible = visible;
            labelEffect1Parameters!.Visible = visible;
            ShowParameters(1);
        }

        private void OnEffectType2Changed(object sender, EventArgs args)
        {
            action.Effects[2].Type = (EffectType) cbEffectType2!.Active;
            bool visible = action.Effects[2].Type != EffectType.None;
            effect2Parameters!.Visible = visible;
            labelEffect2Parameters!.Visible = visible;
            ShowParameters(2);
        }

        private void OnEffectType3Changed(object sender, EventArgs args)
        {
            action.Effects[3].Type = (EffectType) cbEffectType3!.Active;
            bool visible = action.Effects[3].Type != EffectType.None;
            effect3Parameters!.Visible = visible;
            labelEffect3Parameters!.Visible = visible;
            ShowParameters(3);
        }

        private void OnEffect0ParamTypeChanged(object sender, EventArgs args)
        {
        }

        private void OnEffect1ParamTypeChanged(object sender, EventArgs args)
        {
        }

        private void OnEffect2ParamTypeChanged(object sender, EventArgs args)
        {
        }

        private void OnEffect3ParamTypeChanged(object sender, EventArgs args)
        {
        }

        private void OnEffect0ParamValueChanged(object sender, EventArgs args)
        {
        }

        private void OnEffect1ParamValueChanged(object sender, EventArgs args)
        {
        }

        private void OnEffect2ParamValueChanged(object sender, EventArgs args)
        {
        }

        private void OnEffect3ParamValueChanged(object sender, EventArgs args)
        {
        }
    }
}
