using Gtk;
using SkyEditor.RomEditor.Infrastructure;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.ActDataInfo;

namespace SkyEditorUI.Controllers
{
    partial class ActionController : Widget
    {
        public void LoadFlagsTab()
        {
            for (int i = 0; i <= 63; i++)
            {
                var flagSwitch = (Switch) builder.GetObject($"switchFlag{i}");
                var flag = (ActionFlags) (1ul << i);
                flagSwitch.Active = action.Flags.HasFlag(flag);
            }
        }

        [GLib.ConnectBefore]
        private void OnFlagStateSet(object sender, StateSetArgs args)
        {
            var flagSwitch = (Switch) sender;

            // Extract flag from switch name
            var flagIndex = int.Parse(flagSwitch.Name.Replace("switchFlag", ""));
            var flag = (ActionFlags) (1ul << flagIndex);
            action.Flags = action.Flags.SetFlag(flag, flagSwitch.Active);
        }
    }
}
