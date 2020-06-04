using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Library
{
    public interface IRomLibrary
    {
        /// <summary>
        /// Gets metadata about ROMs in the library
        /// </summary>
        IEnumerable<RomLibraryItem> GetItems();

        /// <summary>
        /// Gets the library item with the given name, or null if no such item exists
        /// </summary>
        /// <param name="name">Name of the desired library item</param>
        /// <returns>The library item with the given name, or null if no such item exists</returns>
        RomLibraryItem? GetItem(string name);

        /// <summary>
        /// Adds an extracted ROM to the library
        /// </summary>
        /// <param name="sourceDirectory">Directory of the ROM</param>
        /// <param name="sourceFileSystem">File system on which the ROM is located</param>
        /// <param name="name">What the newly added ROM should be named once added to the library</param>
        void AddDirectory(string sourceDirectory, IFileSystem sourceFileSystem, string name);

        /// <summary>
        /// Adds an extracted ROM to the library
        /// </summary>
        /// <param name="sourceDirectory">Directory of the ROM</param>
        /// <param name="fileSystem">File system on which the ROM is located</param>
        /// <param name="name">What the newly added ROM should be named once added to the library</param>
        Task AddDirectoryAsync(string sourceDirectory, IFileSystem sourceFileSystem, string name);

        /// <summary>
        /// Removes an item from the library
        /// </summary>
        /// <param name="name">Name of the item to remove</param>
        void Remove(string name);
    }

    public class RomLibrary : IRomLibrary
    {
        public RomLibrary(string directory, IFileSystem fileSystem)
        {
            this.directory = directory ?? throw new ArgumentNullException(nameof(directory));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        private readonly string directory;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Gets metadata about ROMs in the library
        /// </summary>
        public IEnumerable<RomLibraryItem> GetItems()
        {
            return fileSystem.GetDirectories(directory, true).Select(d => new RomLibraryItem(d));
        }

        /// <summary>
        /// Gets the library item with the given name, or null if no such item exists
        /// </summary>
        /// <param name="name">Name of the desired library item</param>
        /// <returns>The library item with the given name, or null if no such item exists</returns>
        public RomLibraryItem? GetItem(string name)
        {
            var directory = Path.Combine(this.directory, name);
            if (this.fileSystem.DirectoryExists(directory))
            {
                return new RomLibraryItem(directory);
            }
            return null;
        }

        /// <summary>
        /// Adds an extracted ROM to the library
        /// </summary>
        /// <param name="sourceDirectory">Directory of the ROM</param>
        /// <param name="sourceFileSystem">File system on which the ROM is located</param>
        /// <param name="name">What the newly added ROM should be named once added to the library</param>
        public void AddDirectory(string sourceDirectory, IFileSystem sourceFileSystem, string name)
        {
            var targetDirectory = Path.Combine(this.directory, name);
            sourceFileSystem.CopyDirectory(sourceDirectory, this.fileSystem, targetDirectory);
        }

        /// <summary>
        /// Adds an extracted ROM to the library
        /// </summary>
        /// <param name="sourceDirectory">Directory of the ROM</param>
        /// <param name="fileSystem">File system on which the ROM is located</param>
        /// <param name="name">What the newly added ROM should be named once added to the library</param>
        public async Task AddDirectoryAsync(string sourceDirectory, IFileSystem sourceFileSystem, string name)
        {
            var targetDirectory = Path.Combine(this.directory, name);
            await sourceFileSystem.CopyDirectoryAsync(sourceDirectory, this.fileSystem, targetDirectory).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes an item from the library
        /// </summary>
        /// <param name="name">Name of the item to remove</param>
        public void Remove(string name)
        {
            this.fileSystem.DeleteDirectory(Path.Combine(this.directory, name));
        }
    }
}
