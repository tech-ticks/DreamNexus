using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using SkyEditor.RomEditor.Infrastructure;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IActionCollection
    {
        IDictionary<int, ActionModel> LoadedActions { get; }
        int ActionCount { get; }
        void SetAction(int id, ActionModel model);
        bool IsActionDirty(int id);
        ActionModel? GetActionById(int id);
        void Flush(IRtdxRom rom);
        string GetUsedByString(int actionId);
    }

    public class ActionCollection : IActionCollection
    {
        public IDictionary<int, ActionModel> LoadedActions { get; } = new Dictionary<int, ActionModel>();
        public int ActionCount => actDataInfoEntries.Count;
        public HashSet<int> DirtyActions { get; } = new HashSet<int>();
        private Dictionary<int, IList<WazaDataInfo.Entry>> actionsToMoves = new Dictionary<int, IList<WazaDataInfo.Entry>>();
        private Dictionary<int, IList<ItemDataInfo.Entry>> actionsToItems = new Dictionary<int, IList<ItemDataInfo.Entry>>();

        private IRtdxRom rom;
        private Dictionary<int, ActDataInfo.Entry> actDataInfoEntries;

        public ActionCollection(IRtdxRom rom)
        {
            this.rom = rom;
            this.actDataInfoEntries = rom.GetActDataInfo().Entries
                .Select((entry, i) => new { Entry = entry, Index = i })
                .ToDictionary(pair => pair.Index, pair => pair.Entry);

            // Build lookup tables of action indices to moves and items for fast lookups
            for (int i = 0; i < rom.GetMoves().Count; i++)
            {
                var move = rom.GetMoves().GetMoveById((WazaIndex) i, false);
                if (move != null && move.ActIndex != 0)
                {
                    actionsToMoves.AddToList(move.ActIndex, move);
                }
            }

            for (int i = 0; i < rom.GetItems().Count; i++)
            {
                var item = rom.GetItems().GetItemById((ItemIndex) i, false);
                if (item != null)
                {
                    if (item.PrimaryActIndex != 0)
                    {
                        actionsToItems.AddToList(item.PrimaryActIndex, item);
                    }
                    if (item.ReviveActIndex != 0)
                    {
                        actionsToItems.AddToList(item.ReviveActIndex, item);
                    }
                    if (item.ThrowActIndex != 0)
                    {
                        actionsToItems.AddToList(item.ThrowActIndex, item);
                    }
                }
            }
        }

        public ActionModel LoadAction(int index)
        {
            var data = actDataInfoEntries[index];

            var effectModels = new EffectModel[4];
            for (int i = 0; i < data.Effects.Length; i++)
            {
                var effect = data.Effects[i];
                var model = new EffectModel();
                model.Type = effect.Type;

                for (int j = 0; j < effect.Params.Length && j < effect.ParamTypes.Length; j++)
                {
                    if (model.Type != EffectType.None)
                    {
                        model.Parameters.Add(new ParameterModel
                        {
                            Type = effect.ParamTypes[j],
                            Value = effect.Params[j],
                        });
                    }
                }

                effectModels[i] = model;
            }

            return new ActionModel
            {
                Id = index,
                Flags = data.Flags,
                DungeonMessage1 = data.DungeonMessage1,
                DungeonMessage2 = data.DungeonMessage2,
                Effects = effectModels,
                MinAccuracy = data.MinAccuracy,
                MaxAccuracy = data.MaxAccuracy,
                Kind = data.Kind,
                MoveType = data.MoveType,
                MoveCategory = data.MoveCategory,
                MinPower = data.MinPower,
                MaxPower = data.MaxPower,
                MinPP = data.MinPP,
                MaxPP = data.MaxPP,
                ActDataInfoByte83 = data.Byte83,
                ActDataInfoByte84 = data.Byte84,
                ActDataInfoByte85 = data.Byte85,
                ActDataInfoByte86 = data.Byte86,
                ActDataInfoByte87 = data.Byte87,
                Range = data.Range,
                ActDataInfoByte89 = data.Byte89,
                ActDataInfoByte8A = data.Byte8A,
                ActDataInfoByte8B = data.Byte8B,
                Area = data.Area,
                Target = data.Target,
                ActDataInfoByte8E = data.Byte8E,
                ActDataInfoByte8F = data.Byte8F,
                ActDataInfoByte90 = data.Byte90,
                ActDataInfoByte91 = data.Byte91,
                ActDataInfoByte92 = data.Byte92,
                ActHitCountIndex = data.ActHitCountIndex,
                ActDataInfoByte94 = data.Byte94,
                ActDataInfoByte95 = data.Byte95,
                ActDataInfoByte96 = data.Byte96,
                ActDataInfoByte97 = data.Byte97,
                ActDataInfoByte98 = data.Byte98,
                ActDataInfoByte99 = data.Byte99,
                ActDataInfoByte9A = data.Byte9A,
                ActDataInfoByte9B = data.Byte9B,
                ActDataInfoByte9C = data.Byte9C,
                ActDataInfoByte9D = data.Byte9D,
                ActDataInfoByte9E = data.Byte9E,
                ActDataInfoByte9F = data.Byte9F,
            };
        }

        public ActionModel GetActionById(int id)
        {
            DirtyActions.Add(id);
            if (!LoadedActions.ContainsKey(id))
            {
                LoadedActions.Add(id, LoadAction(id));
            }
            return LoadedActions[id];
        }

        public void SetAction(int id, ActionModel model)
        {
            LoadedActions[id] = model;
        }

        public string GetUsedByString(int actionId)
        {
            var englishStrings = rom.GetStrings().English;
            var moveNames = (actionsToMoves.ContainsKey(actionId)
                ? actionsToMoves[actionId] : Enumerable.Empty<WazaDataInfo.Entry>())
                .Select(move => englishStrings.GetMoveName(move.Index)).ToArray();
            var itemNames = (actionsToItems.ContainsKey(actionId)
                ? actionsToItems[actionId] : Enumerable.Empty<ItemDataInfo.Entry>())
                .Select(item => englishStrings.GetItemName(item.Index)).Distinct().ToArray();
            
            if (moveNames.Length == 0 && itemNames.Length == 0) {
                return "unused";
            }
            return string.Join(", ", moveNames.Concat(itemNames));
        }

        public bool IsActionDirty(int id)
        {
            return DirtyActions.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            foreach (var pair in LoadedActions)
            {
                var data = actDataInfoEntries[pair.Key];
                var model = pair.Value;

                data.Flags = model.Flags;
                data.DungeonMessage1 = model.DungeonMessage1;
                data.DungeonMessage2 = model.DungeonMessage2;

                for (int i = 0; i < model.Effects.Length; i++)
                {
                    var effect = data.Effects[i];
                    var effectModel = model.Effects[i];

                    effect.Type = effectModel.Type;
                    if (effect.Type != EffectType.None)
                    {
                        for (int j = 0; j < effectModel.Parameters.Count; j++)
                        {
                            effect.ParamTypes[j] = effectModel.Parameters[j].Type;
                            effect.Params[j] = effectModel.Parameters[j].Value;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < effectModel.Parameters.Count; j++)
                        {
                            effect.ParamTypes[j] = EffectParameterType.None;
                            effect.Params[j] = 0;
                        }
                    }
                }

                data.MinAccuracy = model.MinAccuracy;
                data.MaxAccuracy = model.MaxAccuracy;
                data.Kind = model.Kind;
                data.MoveType = model.MoveType;
                data.MoveCategory = model.MoveCategory;
                data.MinPower = model.MinPower;
                data.MaxPower = model.MaxPower;
                data.MinPP = model.MinPP;
                data.MaxPP = model.MaxPP;
                data.Byte83 = model.ActDataInfoByte83;
                data.Byte84 = model.ActDataInfoByte84;
                data.Byte85 = model.ActDataInfoByte85;
                data.Byte86 = model.ActDataInfoByte86;
                data.Byte87 = model.ActDataInfoByte87;
                data.Range = model.Range;
                data.Byte89 = model.ActDataInfoByte89;
                data.Byte8A = model.ActDataInfoByte8A;
                data.Byte8B = model.ActDataInfoByte8B;
                data.Area = model.Area;
                data.Target = model.Target;
                data.Byte8E = model.ActDataInfoByte8E;
                data.Byte8F = model.ActDataInfoByte8F;
                data.Byte90 = model.ActDataInfoByte90;
                data.Byte91 = model.ActDataInfoByte91;
                data.Byte92 = model.ActDataInfoByte92;
                data.ActHitCountIndex = model.ActHitCountIndex;
                data.Byte94 = model.ActDataInfoByte94;
                data.Byte95 = model.ActDataInfoByte95;
                data.Byte96 = model.ActDataInfoByte96;
                data.Byte97 = model.ActDataInfoByte97;
                data.Byte98 = model.ActDataInfoByte98;
                data.Byte99 = model.ActDataInfoByte99;
                data.Byte9A = model.ActDataInfoByte9A;
                data.Byte9B = model.ActDataInfoByte9B;
                data.Byte9C = model.ActDataInfoByte9C;
                data.Byte9D = model.ActDataInfoByte9D;
                data.Byte9E = model.ActDataInfoByte9E;
                data.Byte9F = model.ActDataInfoByte9F;
            }
        }
    }    
}
