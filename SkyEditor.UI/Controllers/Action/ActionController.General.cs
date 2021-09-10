using Gtk;
using UI = Gtk.Builder.ObjectAttribute;
using System;
using SkyEditorUI.Infrastructure;
using System.Text;
using SkyEditor.RomEditor.Domain.Rtdx.Structures;
using System.Linq;
using static SkyEditor.RomEditor.Domain.Rtdx.Structures.ActDataInfo;
using Entry = Gtk.Entry;

namespace SkyEditorUI.Controllers
{
    partial class ActionController : Widget
    {
        [UI] private Entry? entryBasePower;
        [UI] private Entry? entryMaxPower;
        [UI] private Entry? entryBasePP;
        [UI] private Entry? entryMaxPP;
        [UI] private Entry? entryBaseAccuracy;
        [UI] private Entry? entryMaxAccuracy;
        [UI] private ComboBox? cbChainedHits;
        [UI] private Entry? entryRange;
        [UI] private ComboBoxText? cbArea;
        [UI] private ComboBoxText? cbTarget;
        [UI] private Entry? entryByte83;
        [UI] private Entry? entryByte84;
        [UI] private Entry? entryByte85;
        [UI] private Entry? entryByte86;
        [UI] private Entry? entryByte87;
        [UI] private Entry? entryByte89;
        [UI] private Entry? entryByte8A;
        [UI] private Entry? entryByte8B;
        [UI] private Entry? entryByte8E;
        [UI] private Entry? entryByte8F;
        [UI] private Entry? entryByte90;
        [UI] private Entry? entryByte91;
        [UI] private Entry? entryByte92;
        [UI] private Entry? entryByte94;
        [UI] private Entry? entryByte95;
        [UI] private Entry? entryByte96;
        [UI] private Entry? entryByte97;
        [UI] private Entry? entryByte98;
        [UI] private Entry? entryByte99;
        [UI] private Entry? entryByte9A;
        [UI] private Entry? entryByte9B;
        [UI] private Entry? entryByte9C;
        [UI] private Entry? entryByte9D;
        [UI] private Entry? entryByte9E;
        [UI] private Entry? entryByte9F;

        [UI] private ListStore? chainedHitsStore;

        public void LoadGeneralTab()
        {
            var chainedHitDescriptions = rom.GetActHitCountTableDataInfo().Entries.Select(FormatHits);
            chainedHitsStore!.AppendAll(chainedHitDescriptions);

            entryBasePower!.Text = action.MinPower.ToString();
            entryMaxPower!.Text = action.MaxPower.ToString();
            entryBasePP!.Text = action.MinPP.ToString();
            entryMaxPP!.Text = action.MaxPP.ToString();
            entryBaseAccuracy!.Text = action.MinAccuracy.ToString();
            entryMaxAccuracy!.Text = action.MaxAccuracy.ToString();
            cbChainedHits!.Active = action.ActHitCountIndex;
            entryRange!.Text = action.Range.ToString();
            cbArea!.Active = (int) action.Area;
            cbTarget!.Active = (int) action.Target;

            entryByte83!.Text = action.ActDataInfoByte83.ToString();
            entryByte84!.Text = action.ActDataInfoByte84.ToString();
            entryByte85!.Text = action.ActDataInfoByte85.ToString();
            entryByte86!.Text = action.ActDataInfoByte86.ToString();
            entryByte87!.Text = action.ActDataInfoByte87.ToString();
            entryByte89!.Text = action.ActDataInfoByte89.ToString();
            entryByte8A!.Text = action.ActDataInfoByte8A.ToString();
            entryByte8B!.Text = action.ActDataInfoByte8B.ToString();
            entryByte8E!.Text = action.ActDataInfoByte8E.ToString();
            entryByte8F!.Text = action.ActDataInfoByte8F.ToString();
            entryByte90!.Text = action.ActDataInfoByte90.ToString();
            entryByte91!.Text = action.ActDataInfoByte91.ToString();
            entryByte92!.Text = action.ActDataInfoByte92.ToString();
            entryByte94!.Text = action.ActDataInfoByte94.ToString();
            entryByte95!.Text = action.ActDataInfoByte95.ToString();
            entryByte96!.Text = action.ActDataInfoByte96.ToString();
            entryByte97!.Text = action.ActDataInfoByte97.ToString();
            entryByte98!.Text = action.ActDataInfoByte98.ToString();
            entryByte99!.Text = action.ActDataInfoByte99.ToString();
            entryByte9A!.Text = action.ActDataInfoByte9A.ToString();
            entryByte9B!.Text = action.ActDataInfoByte9B.ToString();
            entryByte9C!.Text = action.ActDataInfoByte9C.ToString();
            entryByte9D!.Text = action.ActDataInfoByte9D.ToString();
            entryByte9E!.Text = action.ActDataInfoByte9E.ToString();
            entryByte9F!.Text = action.ActDataInfoByte9F.ToString();
        }

        private void OnBasePowerChanged(object sender, EventArgs args)
        {
            action.MinPower = entryBasePower!.ParseByte(action.MinPower);
        }

        private void OnMaxPowerChanged(object sender, EventArgs args)
        {
            action.MaxPower = entryMaxPower!.ParseByte(action.MaxPower);
        }

        private void OnBasePPChanged(object sender, EventArgs args)
        {
            action.MinPP = entryBasePP!.ParseByte(action.MinPP);
        }

        private void OnMaxPPChanged(object sender, EventArgs args)
        {
            action.MaxPP = entryMaxPP!.ParseByte(action.MaxPP);
        }

        private void OnBaseAccuracyChanged(object sender, EventArgs args)
        {
            action.MinAccuracy = entryBaseAccuracy!.ParseUShort(action.MinAccuracy);
        }

        private void OnMaxAccuracyChanged(object sender, EventArgs args)
        {
            action.MaxAccuracy = entryMaxAccuracy!.ParseUShort(action.MaxAccuracy);
        }

        private void OnChainedHitsChanged(object sender, EventArgs args)
        {
            action.ActHitCountIndex = (byte) cbChainedHits!.Active;
        }

        private void OnRangeChanged(object sender, EventArgs args)
        {
            action.Range = entryRange!.ParseByte(action.Range);
        }

        private void OnHelpRangeClicked(object sender, EventArgs args)
        {
            var dialog = new MessageDialog(MainWindow.Instance, DialogFlags.DestroyWithParent, MessageType.Info,
                ButtonsType.Ok, "Maximum distance for moves: 2 tiles, 4 tiles, 10 tiles, etc.\n"
                + "For charged moves, the first turn has a range of 0, while the second turn has the correct range.");
            dialog.Title = "Action range";
            dialog.Run();
            dialog.Destroy();
        }

        private void OnAreaChanged(object sender, EventArgs args)
        {
            action.Area = (ActionArea) cbArea!.Active;
        }

        private void OnTargetChanged(object sender, EventArgs args)
        {
            action.Target = (ActionTarget) cbTarget!.Active;
        }

        private void OnByte83Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte83 = entryByte83!.ParseByte(action.ActDataInfoByte83);
        }

        private void OnByte84Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte84 = entryByte84!.ParseByte(action.ActDataInfoByte84);
        }

        private void OnByte85Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte85 = entryByte85!.ParseByte(action.ActDataInfoByte85);
        }

        private void OnByte86Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte86 = entryByte86!.ParseByte(action.ActDataInfoByte86);
        }

        private void OnByte87Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte87 = entryByte87!.ParseByte(action.ActDataInfoByte87);
        }

        private void OnByte89Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte89 = entryByte89!.ParseByte(action.ActDataInfoByte89);
        }

        private void OnByte8AChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte8A = entryByte8A!.ParseByte(action.ActDataInfoByte8A);
        }

        private void OnByte8BChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte8B = entryByte8B!.ParseByte(action.ActDataInfoByte8B);
        }

        private void OnByte8EChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte8E = entryByte8E!.ParseByte(action.ActDataInfoByte8E);
        }

        private void OnByte8FChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte8F = entryByte8F!.ParseByte(action.ActDataInfoByte8F);
        }

        private void OnByte90Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte90 = entryByte90!.ParseByte(action.ActDataInfoByte90);
        }

        private void OnByte91Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte91 = entryByte91!.ParseByte(action.ActDataInfoByte91);
        }

        private void OnByte92Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte92 = entryByte92!.ParseByte(action.ActDataInfoByte92);
        }

        private void OnByte94Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte94 = entryByte94!.ParseByte(action.ActDataInfoByte94);
        }

        private void OnByte95Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte95 = entryByte95!.ParseByte(action.ActDataInfoByte95);
        }

        private void OnByte96Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte96 = entryByte96!.ParseByte(action.ActDataInfoByte96);
        }

        private void OnByte97Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte97 = entryByte97!.ParseByte(action.ActDataInfoByte97);
        }

        private void OnByte98Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte98 = entryByte98!.ParseByte(action.ActDataInfoByte98);
        }

        private void OnByte99Changed(object sender, EventArgs args)
        {
            action.ActDataInfoByte99 = entryByte99!.ParseByte(action.ActDataInfoByte99);
        }

        private void OnByte9AChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte9A = entryByte9A!.ParseByte(action.ActDataInfoByte9A);
        }

        private void OnByte9BChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte9B = entryByte9B!.ParseByte(action.ActDataInfoByte9B);
        }

        private void OnByte9CChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte9C = entryByte9C!.ParseByte(action.ActDataInfoByte9C);
        }

        private void OnByte9DChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte9D = entryByte9D!.ParseByte(action.ActDataInfoByte9D);
        }

        private void OnByte9EChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte9E = entryByte9E!.ParseByte(action.ActDataInfoByte9E);
        }

        private void OnByte9FChanged(object sender, EventArgs args)
        {
            action.ActDataInfoByte9F = entryByte9F!.ParseByte(action.ActDataInfoByte9F);
        }

        // Format a hit count entry
        private string FormatHits(ActHitCountTableDataInfo.Entry hitCountEntry)
        {
            var str = new StringBuilder();
            if (hitCountEntry.MinHits == hitCountEntry.MaxHits)
            {
                str.Append(hitCountEntry.MaxHits == 1 ? "1 hit" : $"{hitCountEntry.MaxHits} hits");
            }
            else
            {
                double weightSum = 0;
                for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
                {
                    weightSum += hitCountEntry.Weights[i - hitCountEntry.MinHits];
                }

                str.Append($"{hitCountEntry.MinHits} to {hitCountEntry.MaxHits} hits (");
                for (var i = hitCountEntry.MinHits; i <= hitCountEntry.MaxHits; i++)
                {
                    var weight = hitCountEntry.Weights[i - hitCountEntry.MinHits];
                    double chance = weight / weightSum * 100.0;
                    if (i > hitCountEntry.MinHits)
                    {
                        str.Append(" ");
                    }
                    str.Append($"{chance:f1}%");
                }
                str.Append(")");
            }

            if (hitCountEntry.StopOnMiss != 0)
            {
                str.Append(", stop on miss");
            }

            return str.ToString();
        }
    }
}
