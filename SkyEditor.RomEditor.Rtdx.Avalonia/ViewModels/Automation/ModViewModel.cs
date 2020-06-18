using ReactiveUI;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Automation
{
    public class ModViewModel : ViewModelBase
    {
        public ModViewModel(Mod mod)
        {
            this.Mod = mod ?? throw new ArgumentNullException(nameof(mod));
        }

        public Mod Mod { get; }

        /// <summary>
        /// ID of the mod. This should remain unchanged across versions, and is required to be unique across all mods.
        /// </summary>
        public string? Id
        {
            get => Mod.Metadata.Id;
            set { Mod.Metadata.Id = value; this.RaisePropertyChanged(nameof(Id)); }
        }

        /// <summary>
        /// Version of the mod, preferably a SemVer (e.g. 1.0.0, 2.0.5-hotfix2, etc)
        /// </summary>
        public string? Version
        {
            get => Mod.Metadata.Version;
            set { Mod.Metadata.Version = value; this.RaisePropertyChanged(nameof(Version)); }
        }

        /// <summary>
        /// Which ROM this mod targets
        /// </summary>
        public string? Target
        {
            get => Mod.Metadata.Target;
            set { Mod.Metadata.Target = value; this.RaisePropertyChanged(nameof(Target)); }
        }

        /// <summary>
        /// Name of the mod
        /// </summary>
        public string? Name
        {
            get => Mod.Metadata.Name;
            set { Mod.Metadata.Name = value; this.RaisePropertyChanged(nameof(Name)); }
        }

        /// <summary>
        /// Description of the mod
        /// </summary>
        public string? Description
        {
            get => Mod.Metadata.Description;
            set { Mod.Metadata.Description = value; this.RaisePropertyChanged(nameof(Description)); }
        }

        /// <summary>
        /// Author of the mod
        /// </summary>
        public string? Author
        {
            get => Mod.Metadata.Author;
            set { Mod.Metadata.Author = value; this.RaisePropertyChanged(nameof(Author)); }
        }

        /// <summary>
        /// Whether the mod should be applied by default. The user can override this value.
        /// </summary>
        public bool Enabled
        {
            get => Mod.Metadata.Enabled;
            set { Mod.Metadata.Enabled = value; this.RaisePropertyChanged(nameof(Enabled)); }
        }
    }
}
