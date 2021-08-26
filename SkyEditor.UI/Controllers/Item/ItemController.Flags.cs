using System;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx.Constants;
using SkyEditor.RomEditor.Infrastructure;
using SkyEditorUI.Infrastructure;
using UI = Gtk.Builder.ObjectAttribute;

namespace SkyEditorUI.Controllers
{
    partial class ItemController : Widget
    {
        [UI] private Switch? switchThink;
        [UI] private Switch? switchThrowParty;
        [UI] private Switch? switchThrowEnemy;
        [UI] private Switch? switchImplement;
        [UI] private Switch? switchExchange;
        [UI] private Switch? switchDestroyed;
        [UI] private Switch? switchMegumiEat;
        [UI] private Switch? switchMegumiBurn;
        [UI] private Switch? switchEat;
        [UI] private Switch? switchSecure;
        [UI] private Switch? switchDlc;
        [UI] private Switch? switchAutoLog;
        [UI] private Switch? switchHighClass;

        private void LoadFlagsTab()
        {
            switchThink!.Active = item.Flags.HasFlag(ItemFlags.THINK);
            switchThrowParty!.Active = item.Flags.HasFlag(ItemFlags.THROW_PARTY);
            switchThrowEnemy!.Active = item.Flags.HasFlag(ItemFlags.THROW_ENEMY);
            switchImplement!.Active = item.Flags.HasFlag(ItemFlags.IMPLEMENT);
            switchExchange!.Active = item.Flags.HasFlag(ItemFlags.EXCHANGE);
            switchDestroyed!.Active = item.Flags.HasFlag(ItemFlags.DESTROYED);
            switchMegumiEat!.Active = item.Flags.HasFlag(ItemFlags.MEGUMI_EAT);
            switchMegumiBurn!.Active = item.Flags.HasFlag(ItemFlags.MEGUMI_BURN);
            switchEat!.Active = item.Flags.HasFlag(ItemFlags.EAT);
            switchSecure!.Active = item.Flags.HasFlag(ItemFlags.SECURE);
            switchDlc!.Active = item.Flags.HasFlag(ItemFlags.DLC);
            switchAutoLog!.Active = item.Flags.HasFlag(ItemFlags.AUTO_LOG);
            switchHighClass!.Active = item.Flags.HasFlag(ItemFlags.HIGH_CLASS);
        }

        [GLib.ConnectBefore]
        private void OnThinkStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.THINK, args.State);
        }

        [GLib.ConnectBefore]
        private void OnThrowPartyStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.THROW_PARTY, args.State);
        }

        [GLib.ConnectBefore]
        private void OnThrowEnemyStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.THROW_ENEMY, args.State);
        }

        [GLib.ConnectBefore]
        private void OnImplementStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.IMPLEMENT, args.State);
        }

        [GLib.ConnectBefore]
        private void OnExchangeStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.EXCHANGE, args.State);
        }

        [GLib.ConnectBefore]
        private void OnDestroyedStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.DESTROYED, args.State);
        }

        [GLib.ConnectBefore]
        private void OnMegumiEatStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.MEGUMI_EAT, args.State);
        }

        [GLib.ConnectBefore]
        private void OnMegumiBurnStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.MEGUMI_BURN, args.State);
        }

        [GLib.ConnectBefore]
        private void OnEatStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.EAT, args.State);
        }

        [GLib.ConnectBefore]
        private void OnSecureStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.SECURE, args.State);
        }

        [GLib.ConnectBefore]
        private void OnDlcStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.DLC, args.State);
        }

        [GLib.ConnectBefore]
        private void OnAutoLogStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.AUTO_LOG, args.State);
        }

        [GLib.ConnectBefore]
        private void OnHighClassStateSet(object sender, StateSetArgs args)
        {
            item.Flags = item.Flags.SetFlag(ItemFlags.HIGH_CLASS, args.State);
        }
    }
}
