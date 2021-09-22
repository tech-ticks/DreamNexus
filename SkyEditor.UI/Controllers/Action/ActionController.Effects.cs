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
using System.Linq;
using SkyEditorUI.Infrastructure;

namespace SkyEditorUI.Controllers
{
    partial class ActionController : Widget
    {
        [UI] private ComboBox? cbEffectType0;
        [UI] private ComboBox? cbEffectType1;
        [UI] private ComboBox? cbEffectType2;
        [UI] private ComboBox? cbEffectType3;
        [UI] private Grid? effect0Parameters;
        [UI] private Grid? effect1Parameters;
        [UI] private Grid? effect2Parameters;
        [UI] private Grid? effect3Parameters;

        [UI] private ListStore? effectTypesStore;
        [UI] private ListStore? statusStore;
        [UI] private ListStore? dungeonStatusStore;

        private List<EffectParameterType>[] effectParametersByIndex = new List<EffectParameterType>[8];

        private ILookup<EffectType, EffectParameterType[]>? paramTypesByEffect;

        public void LoadEffectsTab()
        {
            effectTypesStore!.Clear();
            for (EffectType i = (EffectType) 0; i < EffectType.Max; i++)
            {
                string? effectDescription = EffectTypeStrings.ResourceManager.GetString(i.ToString());
                if (string.IsNullOrEmpty(effectDescription))
                {
                    effectDescription = "Unknown effect";
                }
                effectTypesStore.AppendValues((int) i, $"#{((int) i).ToString("000")}: {effectDescription}");
            }

            for (int i = 0; i < effectParametersByIndex.Length; i++)
            {
                effectParametersByIndex[i] = new List<EffectParameterType>();
                var store = builder.GetObject("paramsStore" + i) as ListStore;
                store!.AppendValues(0, "None");
            }

            // Find parameter types by using the first set of types from an action
            // with the same type in the base ROM
            paramTypesByEffect = rom.GetActDataInfo().Entries
                .SelectMany(entry => entry.Effects)
                .ToLookup(effect => effect.Type, effect => effect.ParamTypes);

            var paramDataInfo = rom.GetActParamDataInfo();
            for (EffectParameterType i = (EffectParameterType) 1; i < EffectParameterType.Max; i++)
            {
                var expectedIndex = paramDataInfo.Entries[(int) i];
                var store = builder.GetObject("paramsStore" + expectedIndex) as ListStore;
                store!.AppendValues((int) i, i.GetDescription());
                effectParametersByIndex[expectedIndex].Add(i);
            }

            for (StatusIndex i = 0; i < StatusIndex.END; i++)
            {
                string? name = englishStrings.GetStatusName(i);
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = $"({i.ToString()})";
                }
                statusStore!.AppendValues((int) i, name);
            }

            for (DungeonStatusIndex i = 0; i < DungeonStatusIndex.END; i++)
            {
                string? name = englishStrings.GetDungeonStatusName(i);
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = $"({i.ToString()})";
                }
                dungeonStatusStore!.AppendValues((int) i, name);
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
            bool foundTypes = FindParameterTypes(effectIndex);
            var effect = action.Effects[effectIndex];
            for (int j = 0; j < effect.Parameters.Count; j++)
            {
                var param = effect.Parameters[j];
                var typeLabel = builder.GetObject($"labelEffect{effectIndex}Param{j}") as Label;
                if (typeLabel != null)
                {
                    typeLabel.Text = $"{param.Type.GetDescription()}:";
                    typeLabel.Visible = param.Type != EffectParameterType.None;
                }

                var displayType = param.Type.GetDisplayType();

                var valueStack = builder.GetObject($"stackEffect{effectIndex}Param{j}") as Stack;
                valueStack!.Visible = param.Type != EffectParameterType.None;

                if (displayType == typeof(ushort))
                {
                    var valueEntry = builder.GetObject($"entryEffect{effectIndex}Param{j}") as Entry;
                    if (valueEntry != null)
                    {
                        valueEntry.Text = param.Value.ToString();
                        valueStack!.VisibleChild = valueStack.Children[0];
                    }
                }
                else if (displayType == typeof(bool))
                {
                    var valueSwitch = builder.GetObject($"switchEffect{effectIndex}Param{j}") as Switch;
                    if (valueSwitch != null)
                    {
                        valueSwitch.Active = param.Value != 0;
                        valueStack!.VisibleChild = valueStack.Children[1];
                    }
                }
                else
                {
                    // Enum type
                    var valueComboBox = builder.GetObject($"cbEffect{effectIndex}Param{j}") as ComboBox;
                    valueStack!.VisibleChild = valueStack.Children[2];
                    
                    if (displayType == typeof(PokemonType))
                    {
                        valueComboBox!.Model = typesStore;
                    }
                    else if (displayType == typeof(StatusIndex))
                    {
                        valueComboBox!.Model = statusStore;
                    }
                    else if (displayType == typeof(DungeonStatusIndex))
                    {
                        valueComboBox!.Model = dungeonStatusStore;
                    }

                    valueComboBox!.Active = param.Value;
                }
            }

            UpdateStatChangeDescription(effectIndex);
        }

        private void UpdateStatChangeDescription(int effectIndex)
        {
            var effect = action.Effects[effectIndex];
            var statChangeParam = effect.Parameters.FirstOrDefault(param =>
                param.Type == EffectParameterType.StatChangeIndex
                || param.Type == EffectParameterType.StatMultiplierIndex);

            var explainationLabel = builder.GetObject($"labelEffect{effectIndex}StatExplanation") as Label;
            if (statChangeParam != null)
            {
                var description = DescribeStatChanges(statChangeParam.Value,
                    statChangeParam.Type == EffectParameterType.StatMultiplierIndex);
                explainationLabel!.Visible = true;
                explainationLabel.Text = $"Stat change: {description}";
            }
            else
            {
                explainationLabel!.Visible = false;
            }
        }

        private void OnEffectType0Changed(object sender, EventArgs args)
        {
            var effect = action.Effects[0];
            effect.Type = (EffectType) cbEffectType0!.Active;
            ShowParameters(0);
            bool visible = effect.Type != EffectType.None
                && effect.Parameters.Any(param => param.Type != EffectParameterType.None);
            effect0Parameters!.Visible = visible;
        }

        private void OnEffectType1Changed(object sender, EventArgs args)
        {
            var effect = action.Effects[1];
            effect.Type = (EffectType) cbEffectType1!.Active;
            ShowParameters(1);
            bool visible = effect.Type != EffectType.None
                && effect.Parameters.Any(param => param.Type != EffectParameterType.None);
            effect1Parameters!.Visible = visible;
        }

        private void OnEffectType2Changed(object sender, EventArgs args)
        {
            var effect = action.Effects[2];
            effect.Type = (EffectType) cbEffectType2!.Active;
            ShowParameters(2);
            bool visible = effect.Type != EffectType.None
                && effect.Parameters.Any(param => param.Type != EffectParameterType.None);
            effect2Parameters!.Visible = visible;
        }

        private void OnEffectType3Changed(object sender, EventArgs args)
        {
            var effect = action.Effects[3];
            effect.Type = (EffectType) cbEffectType3!.Active;
            ShowParameters(3);
            bool visible = effect.Type != EffectType.None
                && effect.Parameters.Any(param => param.Type != EffectParameterType.None);
            effect3Parameters!.Visible = visible;
        }

        private void OnEffect0ParamTypeChanged(object sender, EventArgs args)
        {
            var combobox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(combobox.Name.Last().ToString());
            action.Effects[0].Parameters[parameterIndex].Type = (EffectParameterType) combobox.Active;
        }

        private void OnEffect1ParamTypeChanged(object sender, EventArgs args)
        {
            var combobox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(combobox.Name.Last().ToString());
            action.Effects[1].Parameters[parameterIndex].Type = (EffectParameterType) combobox.Active;
        }

        private void OnEffect2ParamTypeChanged(object sender, EventArgs args)
        {
            var combobox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(combobox.Name.Last().ToString());
            action.Effects[2].Parameters[parameterIndex].Type = (EffectParameterType) combobox.Active;
        }

        private void OnEffect3ParamTypeChanged(object sender, EventArgs args)
        {
            var combobox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(combobox.Name.Last().ToString());
            action.Effects[3].Parameters[parameterIndex].Type = (EffectParameterType) combobox.Active;
        }

        private void OnEffect0ParamValueChanged(object sender, EventArgs args)
        {
            var entry = (Entry) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(entry.Name.Last().ToString());
            var param = action.Effects[0].Parameters[parameterIndex];
            param.Value = entry.ParseUShort(param.Value);
            UpdateStatChangeDescription(0);
        }

        private void OnEffect1ParamValueChanged(object sender, EventArgs args)
        {
            var entry = (Entry) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(entry.Name.Last().ToString());
            var param = action.Effects[1].Parameters[parameterIndex];
            param.Value = entry.ParseUShort(param.Value);
            UpdateStatChangeDescription(1);
        }

        private void OnEffect2ParamValueChanged(object sender, EventArgs args)
        {
            var entry = (Entry) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(entry.Name.Last().ToString());
            var param = action.Effects[2].Parameters[parameterIndex];
            param.Value = entry.ParseUShort(param.Value);
            UpdateStatChangeDescription(2);
        }

        private void OnEffect3ParamValueChanged(object sender, EventArgs args)
        {
            var entry = (Entry) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(entry.Name.Last().ToString());
            var param = action.Effects[3].Parameters[parameterIndex];
            param.Value = entry.ParseUShort(param.Value);
            UpdateStatChangeDescription(3);
        }

        [GLib.ConnectBefore]
        private void OnEffect0ParamValueStateSet(object sender, StateSetArgs args)
        {
            var switchWidget = (Switch) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(switchWidget.Name.Last().ToString());
            var param = action.Effects[0].Parameters[parameterIndex];
            param.Value = args.State ? (ushort) 1 : (ushort) 0;
        }

        [GLib.ConnectBefore]
        private void OnEffect1ParamValueStateSet(object sender, StateSetArgs args)
        {
            var switchWidget = (Switch) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(switchWidget.Name.Last().ToString());
            var param = action.Effects[1].Parameters[parameterIndex];
            param.Value = args.State ? (ushort) 1 : (ushort) 0;
        }

        [GLib.ConnectBefore]
        private void OnEffect2ParamValueStateSet(object sender, StateSetArgs args)
        {
            var switchWidget = (Switch) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(switchWidget.Name.Last().ToString());
            var param = action.Effects[2].Parameters[parameterIndex];
            param.Value = args.State ? (ushort) 1 : (ushort) 0;
        }

        [GLib.ConnectBefore]
        private void OnEffect3ParamValueStateSet(object sender, StateSetArgs args)
        {
            var switchWidget = (Switch) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(switchWidget.Name.Last().ToString());
            var param = action.Effects[3].Parameters[parameterIndex];
            param.Value = args.State ? (ushort) 1 : (ushort) 0;
        }

        private void OnEffect0ParamValueEnumChanged(object sender, EventArgs args)
        {
            var comboBox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(comboBox.Name.Last().ToString());
            var param = action.Effects[0].Parameters[parameterIndex];
            param.Value = (ushort) comboBox.Active;
        }

        private void OnEffect1ParamValueEnumChanged(object sender, EventArgs args)
        {
            var comboBox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(comboBox.Name.Last().ToString());
            var param = action.Effects[1].Parameters[parameterIndex];
            param.Value = (ushort) comboBox.Active;
        }

        private void OnEffect2ParamValueEnumChanged(object sender, EventArgs args)
        {
            var comboBox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(comboBox.Name.Last().ToString());
            var param = action.Effects[2].Parameters[parameterIndex];
            param.Value = (ushort) comboBox.Active;
        }

        private void OnEffect3ParamValueEnumChanged(object sender, EventArgs args)
        {
            var comboBox = (ComboBox) sender; 
            // The last digit of the widget's name is the parameter index
            int parameterIndex = int.Parse(comboBox.Name.Last().ToString());
            var param = action.Effects[3].Parameters[parameterIndex];
            param.Value = (ushort) comboBox.Active;
        }

        private bool FindParameterTypes(int effectIndex)
        {
            if (paramTypesByEffect == null)
            {
                return false;
            }
            var effect = action.Effects[effectIndex];
            var paramTypes = paramTypesByEffect[effect.Type].FirstOrDefault();
            if (paramTypes != null)
            {
                if (effect.Parameters.Count < 8)
                {
                    effect.Parameters.Clear();
                    for (int i = 0; i < 8; i++)
                    {
                        effect.Parameters.Add(new ParameterModel());
                    }
                }
                for (int i = 0; i < 8; i++)
                {
                    effect.Parameters[i].Type = paramTypes[i]!;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private string DescribeStatChanges(ushort index, bool percentage)
        {
            var statChangeData = rom.GetActStatusTableDataInfo().Entries;
            if (index < statChangeData.Count)
            {
                var entry = statChangeData[index];
                var changes = new List<string>();

                void addEntry(short mod, string name)
                {
                    if (mod != 0)
                    {
                        if (percentage)
                        {
                            changes.Add($"{mod}% {name}");
                        }
                        else
                        {
                            changes.Add($"{mod:+#;-#} {name}");
                        }
                    }
                }

                addEntry(entry.AttackMod, "Attack");
                addEntry(entry.SpecialAttackMod, "Special Attack");
                addEntry(entry.DefenseMod, "Defense");
                addEntry(entry.SpecialDefenseMod, "Special Defense");
                addEntry(entry.SpeedMod, "Speed");
                addEntry(entry.AccuracyMod, "Accuracy");
                addEntry(entry.EvasionMod, "Evasion");
                return changes.Count > 0 ? string.Join(", ", changes): "None";
            }
            else
            {
                return $"{index} (out of range)";
            }
        }
    }
}
