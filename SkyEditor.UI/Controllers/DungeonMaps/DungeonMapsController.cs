using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditorUI.Infrastructure;
using SkyEditor.RomEditor.Domain.Rtdx.Models;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using System.Linq;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;

namespace SkyEditorUI.Controllers
{
    class DungeonMapsController : Widget
    {
        [UI] private ListStore? mapsStore;
        [UI] private TreeView? mapsTree;

        private IDungeonMapCollection dungeonMaps;
        private ushort fixedMapEndIndex;

        private const int SymbolColumn = 1;
        private const int FixedMapIndexColumn = 2;
        private const int Byte06Column = 3;
        private const int Byte07Column = 4;
        private const int BgmSymbolIndexColumn = 5;
        private const int Byte09Column = 6;
        private const int Byte0AColumn = 7;
        private const int Byte0BColumn = 8;

        public DungeonMapsController(IRtdxRom rom) : this(new Builder("DungeonMaps.glade"), rom)
        {
        }

        private DungeonMapsController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.dungeonMaps = rom.GetDungeonMaps();
            this.fixedMapEndIndex = (ushort) rom.GetFixedMap().Entries.Count;

            for (int i = 0; i < dungeonMaps.Maps.Count; i++)
            {
                var map = dungeonMaps.Maps[i];
                AddMapToStore(map, i);
            }
        }

        private void AddMapToStore(DungeonMapModel map, int index)
        {
            mapsStore!.AppendValues(index,
                map.Symbol,
                FormatFixedMapIndex(map.FixedMapIndex),
                (int) map.Byte06,
                (int) map.Byte07,
                (int) map.DungeonBgmSymbolIndex,
                (int) map.Byte09,
                (int) map.Byte0A,
                (int) map.Byte0B
            );
        }

        private void OnSymbolEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path) && !string.IsNullOrWhiteSpace(args.NewText))
            {
                mapsStore.SetValue(iter, SymbolColumn, args.NewText);
                dungeonMaps.Maps[path.Indices[0]].Symbol = args.NewText;
            }
        }

        private void OnFixedMapIndexEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path))
            {
                var map = dungeonMaps.Maps[path.Indices[0]];
                if (string.IsNullOrWhiteSpace(args.NewText)
                    || args.NewText.ToLower() == "none" || args.NewText.ToLower() == "null")
                {
                    // A fixed map index of (fixed map entries).length means that no fixed map is used
                    map.FixedMapIndex = fixedMapEndIndex;
                }
                if (ushort.TryParse(args.NewText, out ushort value))
                {
                    map.FixedMapIndex = value;
                }
                mapsStore.SetValue(iter, FixedMapIndexColumn, FormatFixedMapIndex(map.FixedMapIndex));
            }
        }

        private void OnBgmIndexEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path))
            {
                var map = dungeonMaps.Maps[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    map.DungeonBgmSymbolIndex = value;
                }
                mapsStore.SetValue(iter, BgmSymbolIndexColumn, (int) map.DungeonBgmSymbolIndex);
            }
        }

        private void OnByte06Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path))
            {
                var map = dungeonMaps.Maps[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    map.Byte06 = value;
                }
                mapsStore.SetValue(iter, Byte06Column, (int) map.Byte06);
            }
        }

        private void OnByte07Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path))
            {
                var map = dungeonMaps.Maps[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    map.Byte07 = value;
                }
                mapsStore.SetValue(iter, Byte07Column, (int) map.Byte07);
            }
        }

        private void OnByte09Edited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path))
            {
                var map = dungeonMaps.Maps[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    map.Byte09 = value;
                }
                mapsStore.SetValue(iter, Byte09Column, (int) map.Byte09);
            }
        }

        private void OnByte0AEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path))
            {
                var map = dungeonMaps.Maps[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    map.Byte0A = value;
                }
                mapsStore.SetValue(iter, Byte0AColumn, (int) map.Byte0A);
            }
        }

        private void OnByte0BEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (mapsStore!.GetIter(out var iter, path))
            {
                var map = dungeonMaps.Maps[path.Indices[0]];
                if (byte.TryParse(args.NewText, out byte value))
                {
                    map.Byte0B = value;
                }
                mapsStore.SetValue(iter, Byte0BColumn, (int) map.Byte0B);
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            var map = new DungeonMapModel { FixedMapIndex = fixedMapEndIndex };
            dungeonMaps.Maps.Add(map);
            AddMapToStore(map, dungeonMaps.Maps.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (mapsTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                dungeonMaps.Maps.RemoveAt(index);
                (model as ListStore)!.Remove(ref iter);
            }
        }

        private string FormatFixedMapIndex(ushort index)
        {
            return index == fixedMapEndIndex ? "None" : index.ToString();
        }
    }
}
