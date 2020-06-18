using Avalonia;
using Avalonia.Controls;
using DynamicData;
using ReactiveUI;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Avalonia.Infrastructure;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Avalonia.ViewModels.Automation
{
    public class ModpackCreatorViewModel : OpenFileViewModel
    {
        public ModpackCreatorViewModel()
        {
            metadata = new ModpackMetadata
            {
                Id = "MyModpack",
                Version = "1.0.0",
                Target = "RTDX",
                Name = "My Modpack",
                Description = "A modpack that does a thing",
                Author = "Me"
            };

            Mods = new ObservableCollection<ModViewModel>();
            AddModFromDirectoryCommand = ReactiveCommand.Create(AddModFromDirectory);
            AddModFromFileCommand = ReactiveCommand.Create(AddModFromFile);
            SaveCommand = ReactiveCommand.Create(Save);
        }

        private readonly ModpackMetadata metadata;

        public override string Name => throw new System.NotImplementedException();

        /// <summary>
        /// ID of the modpack. This should remain unchanged across versions, and is required to be unique across all modpacks.
        /// </summary>
        public string? Id
        {
            get => metadata.Id;
            set { metadata.Id = value; this.RaisePropertyChanged(nameof(Id)); }
        }

        /// <summary>
        /// Version of the modpack, preferably a SemVer (e.g. 1.0.0, 2.0.5-hotfix2, etc)
        /// </summary>
        public string? Version
        {
            get => metadata.Version;
            set { metadata.Version = value; this.RaisePropertyChanged(nameof(Version)); }
        }

        /// <summary>
        /// Which ROM this mod targets
        /// </summary>
        public string? Target
        {
            get => metadata.Target;
            set { metadata.Target = value; this.RaisePropertyChanged(nameof(Target)); }
        }

        /// <summary>
        /// Name of the modpack
        /// </summary>
        public string? ModpackName
        {
            get => metadata.Name;
            set { metadata.Name = value; this.RaisePropertyChanged(nameof(ModpackName)); }
        }

        /// <summary>
        /// Description of the modpack
        /// </summary>
        public string? Description
        {
            get => metadata.Description;
            set { metadata.Description = value; this.RaisePropertyChanged(nameof(Description)); }
        }

        /// <summary>
        /// Author of the mod
        /// </summary>
        public string? Author
        {
            get => metadata.Author;
            set { metadata.Author = value; this.RaisePropertyChanged(nameof(Author)); }
        }

        public ObservableCollection<ModViewModel> Mods { get; }
        public ModViewModel? SelectedMod
        {
            get => _selectedMod;
            set
            {
                _selectedMod = value;
                this.RaisePropertyChanged(nameof(SelectedMod));
                this.RaisePropertyChanged(nameof(IsModSelected));
            }
        }
        private ModViewModel? _selectedMod;

        public bool IsModSelected => SelectedMod != null;

        public ReactiveCommand<Unit, Task> AddModFromDirectoryCommand { get; }
        public ReactiveCommand<Unit, Task> AddModFromFileCommand { get; }
        public ReactiveCommand<Unit, Task> SaveCommand { get; }

        private async Task AddModFromDirectory()
        {
            var dialog = new OpenFolderDialog();
            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                var modpack = new Modpack(path, PhysicalFileSystem.Instance);
                Mods.AddRange(modpack.Mods?.Select(m => new ModViewModel(m)) ?? Enumerable.Empty<ModViewModel>());
            }
        }

        private async Task AddModFromFile()
        {
            var dialog = new OpenFileDialog
            {
                AllowMultiple = true,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_ModFiles, Extensions = new List<string> { "zip" }},
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_CsFiles, Extensions = new List<string> { "csx" }},
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_LuaFiles, Extensions = new List<string> { "lua" }},
                }
            };

            var paths = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            foreach (var path in paths)
            {
                var modpack = new Modpack(path, PhysicalFileSystem.Instance);
                Mods.AddRange(modpack.Mods?.Select(m => new ModViewModel(m)) ?? Enumerable.Empty<ModViewModel>());
            }
        }

        private async Task Save()
        {
            var dialog = new SaveFileDialog
            {
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = Properties.Resources.FileDialogFilter_ModpackFiles, Extensions = new List<string> { "zip" }}
                }
            };

            var path = await dialog.ShowAsync(Application.Current.GetMainWindowOrThrow());
            if (!string.IsNullOrEmpty(path))
            {
                var builder = new ModpackBuilder
                {
                    Metadata = this.metadata
                };
                foreach (var mod in Mods)
                {
                    builder.AddMod(mod.Mod);
                }

                await builder.Build(path);
            }
        }
    }
}
