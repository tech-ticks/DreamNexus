using System;
using System.Collections.Generic;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditor.RomEditor.Domain.Rtdx.Models
{
    public interface IMoveCollection
    {
        IDictionary<WazaIndex, WazaDataInfo.Entry> LoadedMoves { get; }
        int Count { get; }
        void SetMove(WazaIndex id, WazaDataInfo.Entry model);
        bool IsMoveDirty(WazaIndex id);
        WazaDataInfo.Entry? GetMoveById(WazaIndex id, bool markAsDirty = true);
        void Flush(IRtdxRom rom);
    }

    public class MoveCollection : IMoveCollection
    {
        public IDictionary<WazaIndex, WazaDataInfo.Entry> LoadedMoves { get; } = new Dictionary<WazaIndex, WazaDataInfo.Entry>();
        public HashSet<WazaIndex> DirtyMoves { get; } = new HashSet<WazaIndex>();
        public int Count { get; private set; }

        private IRtdxRom rom;

        public MoveCollection(IRtdxRom rom)
        {
            this.rom = rom;
            Count = rom.GetWazaDataInfo().Entries.Count;
        }

        public WazaDataInfo.Entry LoadMove(WazaIndex index)
        {
            var data = rom.GetWazaDataInfo().Entries[(int) index];
            return data.Clone();
        }

        public WazaDataInfo.Entry GetMoveById(WazaIndex id, bool markAsDirty = true)
        {
            if (!LoadedMoves.ContainsKey(id))
            {
                LoadedMoves.Add(id, LoadMove(id));
            }
            if (markAsDirty)
            {
                DirtyMoves.Add(id);
            }
            return LoadedMoves[id];
        }

        public void SetMove(WazaIndex id, WazaDataInfo.Entry model)
        {
            LoadedMoves[id] = model;
        }

        public bool IsMoveDirty(WazaIndex id)
        {
            return DirtyMoves.Contains(id);
        }

        public void Flush(IRtdxRom rom)
        {
            var romMoves = rom.GetWazaDataInfo().Entries;
            foreach (var move in LoadedMoves)
            {
                romMoves[(int) move.Key] = move.Value.Clone();
            }
        }
    }    
}
