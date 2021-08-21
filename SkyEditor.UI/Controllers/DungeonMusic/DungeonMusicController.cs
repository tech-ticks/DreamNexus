using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    class DungeonMusicController : Widget
    {
        [UI] private ListStore? musicStore;
        [UI] private TreeView? musicTree;

        private IDungeonMusicCollection dungeonMusic;

        private const int SymbolColumn = 1;

        public DungeonMusicController(IRtdxRom rom) : this(new Builder("DungeonMusic.glade"), rom)
        {
        }

        private DungeonMusicController(Builder builder, IRtdxRom rom) : base(builder.GetRawOwnedObject("main"))
        {
            builder.Autoconnect(this);

            this.dungeonMusic = rom.GetDungeonMusic();

            for (int i = 0; i < dungeonMusic.Music.Count; i++)
            {
                var symbol = dungeonMusic.Music[i];
                AddMusicToStore(symbol, i);
            }
        }

        private void AddMusicToStore(string symbol, int index)
        {
            musicStore!.AppendValues(index, symbol);
        }

        private void OnSymbolEdited(object sender, EditedArgs args)
        {
            var path = new TreePath(args.Path);
            if (musicStore!.GetIter(out var iter, path) && !string.IsNullOrWhiteSpace(args.NewText))
            {
                musicStore.SetValue(iter, SymbolColumn, args.NewText);
                dungeonMusic.Music[path.Indices[0]] = args.NewText;
            }
        }

        private void OnAddClicked(object sender, EventArgs args)
        {
            dungeonMusic.Music.Add("");
            AddMusicToStore("", dungeonMusic.Music.Count - 1);
        }

        private void OnRemoveClicked(object sender, EventArgs args)
        {
            if (musicTree!.Selection.GetSelected(out var model, out var iter))
            {
                var path = model.GetPath(iter);
                int index = path.Indices[0];
                dungeonMusic.Music.RemoveAt(index);
                (model as ListStore)!.Remove(ref iter);
            }
        }
    }
}
