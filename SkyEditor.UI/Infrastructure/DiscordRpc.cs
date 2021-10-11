using System;
using System.Collections.Generic;
using System.Reflection;
using DiscordRPC;
using Gtk;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using SkyEditorUI.Controllers;

namespace SkyEditorUI.Infrastructure
{
    public class DiscordRpc
    {
        private const string ClientId = "897109434893991976";
        private const uint IdleTimeoutSeconds = 5 * 60;
        
        private readonly DiscordRpcClient? client;
        private readonly bool loaded;
        private DateTime start;
        private bool idle;
        private string? controllerState;
        private string? controllerInfo;
        private Modpack? modpack;
        private string? modpackName;
        private uint? idleTimeoutId;
        private readonly string version;
        private IRtdxRom? rom;

        public DiscordRpc()
        {
            version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "dev";
            loaded = false;
            idle = false;
            start = DateTime.UtcNow;
            try
            {
                client = new DiscordRpcClient(ClientId);
                //client.Logger = new ConsoleLogger() { Level = LogLevel.Info };
                client.Initialize();
                loaded = true;
                Console.WriteLine(@"Loaded Discord integration.");
                UpdateCurrentPresence();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Failed to load Discord integration: {0}.", ex);
            }
        }

        public void OnWindowHasFocus()
        {
            if (idleTimeoutId != null) GLib.Timeout.Remove((uint)idleTimeoutId);
            if (!idle) return;
            ResetPlaytime();
            idle = false;
            UpdateCurrentPresence();
        }

        public void OnWindowLostFocus()
        {
            if (idleTimeoutId == null) GLib.Timeout.AddSeconds(IdleTimeoutSeconds, OnIdle);
        }

        
        public void OnModpackLoaded(IRtdxRom inRom, Modpack inModpack, string? inModpackName)
        {
            rom = inRom;
            modpack = inModpack;
            modpackName = inModpackName;
            UpdateCurrentPresence();
        }

        public void OnViewLoaded(Widget view, List<string> breadcrumbs)
        {
            try 
            {
                Console.WriteLine(view);
                switch (view)
                {
                    case ActionController v:
                        controllerInfo = "Editing Actions";
                        controllerState = breadcrumbs[0];
                        break;
                    case ActionStatModifiersController v:
                        controllerInfo = "Editing Actions";
                        controllerState = "Stat Modifiers";
                        break;
                    case ActorListController v:
                        controllerInfo = "Editing Actor List";
                        controllerState = modpackName;
                        break;
                    case ChargedMovesController v:
                        controllerInfo = "Editing Moves";
                        controllerState = "Charged Moves";
                        break;
                    case DungeonController v:
                        controllerInfo = "Editing Dungeons";
                        controllerState = breadcrumbs[0];
                        break;
                    case DungeonFloorController v:
                        controllerInfo = $"Editing Dungeon '{breadcrumbs[1]}'";
                        controllerState = breadcrumbs[0];
                        break;
                    case DungeonMapsController v:
                        controllerInfo = "Editing Dungeons";
                        controllerState = "Dungeon Maps";
                        break;
                    case DungeonMusicController v:
                        controllerInfo = "Editing Dungeons";
                        controllerState = "Dungeon Music";
                        break;
                    case ExtraLargeMovesController v:
                        controllerInfo = "Editing Moves";
                        controllerState = "Extra Big Moves";
                        break;
                    case ItemController v:
                        controllerInfo = "Editing Items";
                        controllerState = breadcrumbs[0];
                        break;
                    case ModpackScriptsController v:
                        controllerInfo = "Editing Automation Scripts";
                        controllerState = modpackName;
                        break;
                    case ModpackSettingsController v:
                        controllerInfo = "Editing Modpack";
                        controllerState = modpackName;
                        break;
                    case ModsController v:
                        controllerInfo = "Editing Mods for Modpack";
                        controllerState = modpackName;
                        break;
                    case MoveController v:
                        controllerInfo = "Editing Moves";
                        controllerState = breadcrumbs[0];
                        break;
                    case PokemonController v:
                        controllerInfo = "Editing Pokémon";
                        controllerState = breadcrumbs[0];
                        break;
                    case SourceFileController v:
                        controllerInfo = "Editing Scripts";
                        controllerState = v.FileNameLabelValue.Replace("\\", "/")
                            // Get rid of the common path prefix
                            .Replace("/romfs/Data/StreamingAssets/native_data/script/", "");
                        break;
                    case StartersController v:
                        controllerInfo = "Editing Starters";
                        controllerState = modpackName;
                        break;
                    case StringsController v:
                        controllerInfo = "Editing Strings";
                        controllerState = breadcrumbs[0];
                        break;
                    default:
                        controllerInfo = $"Editing modpack '{modpackName}'";
                        controllerState = view.GetType().Name;
                        break;
                }
                UpdateCurrentPresence();
            } catch (Exception ex)
            {
                Console.WriteLine(@"Failed loading view information for Discord: {0}.", ex);
            }
        }

        private bool OnIdle()
        {
            idleTimeoutId = null;
            idle = true;
            UpdateCurrentPresence();
            return false;
        }

        private void UpdateCurrentPresence()
        {
            string suffix = !string.IsNullOrWhiteSpace(modpackName) ? $" | {modpackName}" : "";
            if (!idle)
            {
                UpdatePresence(controllerState, controllerInfo, start, version + suffix);
            }
            else
            {
                UpdatePresence(null, "Idle", null, version + suffix);
            }
        }

        private void UpdatePresence(
            string? state, string? details, DateTime? start, string? largeText, string? largeImage = "dreamnexus",
            string? smallImage = null, string? smallText = null
        ) {
            if (!loaded) return;
            try
            {
                //Console.WriteLine(@"Updated presence.");
                client?.SetPresence(new RichPresence
                {
                    Details = details,
                    State = state,
                    Timestamps = new Timestamps
                    {
                        Start = start
                    },
                    Assets = new Assets
                    {
                        LargeImageKey = largeImage,
                        LargeImageText = largeText,
                        SmallImageKey = smallImage,
                        SmallImageText = smallText
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Failed updating Discord presence: {0}.", ex);
            }
        }

        private void ResetPlaytime()
        {
            start = DateTime.UtcNow;
        }
    }
}
