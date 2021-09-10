using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditorUI.Infrastructure;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using System.Linq;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.ActDataInfo;
using Entry = Gtk.Entry;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Domain.Rtdx.Models;

namespace SkyEditorUI.Controllers
{
    partial class ActionController : Widget
    {
        [UI] private Entry? entryMessage1Hash;
        [UI] private Entry? entryMessage1;
        [UI] private Label? labelMessage1HashName;
        [UI] private Entry? entryMessage2Hash;
        [UI] private Label? labelMessage2HashName;
        [UI] private Entry? entryMessage2;

        public void LoadTextTab()
        {
            entryMessage1Hash!.Text = action.DungeonMessage1.ToString();
            entryMessage2Hash!.Text = action.DungeonMessage2.ToString();
        }

        private void OnMessage1HashChanged(object sender, EventArgs args)
        {
            action.DungeonMessage1 = (TextIDHash) entryMessage1Hash!.ParseInt((int) action.DungeonMessage1);
            entryMessage1!.Text = englishStrings.GetString(StringType.Dungeon, (int) action.DungeonMessage1)
                ?? "(invalid hash)";

            // Try to show the enum name of the hash
            string hashName = action.DungeonMessage1.ToString();
            if (!int.TryParse(hashName, out int _))
            {
                labelMessage1HashName!.Text = hashName;
            }
            else
            {
                labelMessage1HashName!.Text = "";
            }
        }

        private void OnMessage1Changed(object sender, EventArgs args)
        {
            englishStrings.SetString(StringType.Dungeon, (int) action.DungeonMessage1, entryMessage1!.Text);
        }

        private void OnMessage2HashChanged(object sender, EventArgs args)
        {
            action.DungeonMessage2 = (TextIDHash) entryMessage2Hash!.ParseInt((int) action.DungeonMessage2);
            entryMessage2!.Text = englishStrings.GetString(StringType.Dungeon, (int) action.DungeonMessage2)
                ?? "(invalid hash)";

            string hashName = action.DungeonMessage2.ToString();
            if (!int.TryParse(hashName, out int _))
            {
                labelMessage2HashName!.Text = hashName;
            }
            else
            {
                labelMessage2HashName!.Text = "";
            }
        }

        private void OnMessage2Changed(object sender, EventArgs args)
        {
            englishStrings.SetString(StringType.Dungeon, (int) action.DungeonMessage2, entryMessage2!.Text);
        }
    }
}
