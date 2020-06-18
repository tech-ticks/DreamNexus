using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain.Library
{
    public interface ILibrary
    {
        /// <summary>
        /// Gets metadata about ROMs in the library
        /// </summary>
        IEnumerable<LibraryItem> GetItems();

        /// <summary>
        /// Gets the library item with the given name, or null if no such item exists
        /// </summary>
        /// <param name="name">Name of the desired library item</param>
        /// <returns>The library item with the given name, or null if no such item exists</returns>
        LibraryItem? GetItem(string name);

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
        /// Adds a file to the library
        /// </summary>
        /// <param name="sourceFilename">File to add</param>
        /// <param name="sourceFileSystem">File system on which the file is located</param>
        /// <param name="name">What the newly added file should be named once added to the library</param>
        void AddFile(string sourceFilename, IFileSystem sourceFileSystem, string name);

        /// <summary>
        /// Adds a file to the library
        /// </summary>
        /// <param name="sourceFilename">File to add</param>
        /// <param name="sourceFileSystem">File system on which the file is located</param>
        /// <param name="name">What the newly added file should be named once added to the library</param>
        Task AddFileAsync(string sourceFilename, IFileSystem sourceFileSystem, string name);

        /// <summary>
        /// Removes an item from the library
        /// </summary>
        /// <param name="name">Name of the item to remove</param>
        void Remove(string name);
    }

    public class Library : ILibrary
    {
        public Library(string directory, IFileSystem fileSystem)
        {
            this.directory = directory ?? throw new ArgumentNullException(nameof(directory));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        private readonly string directory;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Gets metadata about ROMs in the library
        /// </summary>
        public IEnumerable<LibraryItem> GetItems()
        {
            return fileSystem
                .GetDirectories(directory, true)
                .Concat(fileSystem.GetFiles(directory, "*", true))
                .Select(d => new LibraryItem(d));
        }

        /// <summary>
        /// Gets the library item with the given name, or null if no such item exists
        /// </summary>
        /// <param name="name">Name of the desired library item</param>
        /// <returns>The library item with the given name, or null if no such item exists</returns>
        public LibraryItem? GetItem(string name)
        {
            var path = Path.Combine(this.directory, name);
            if (this.fileSystem.DirectoryExists(path) || this.fileSystem.FileExists(path))
            {
                return new LibraryItem(path);
            }
            return null;
        }

        /// <summary>
        /// Adds a directory to the library
        /// </summary>
        /// <param name="sourceDirectory">Directory to add</param>
        /// <param name="sourceFileSystem">File system on which the directory is located</param>
        /// <param name="name">What the newly added directory should be named once added to the library</param>
        public void AddDirectory(string sourceDirectory, IFileSystem sourceFileSystem, string name)
        {
            var targetDirectory = Path.Combine(this.directory, name);
            sourceFileSystem.CopyDirectory(sourceDirectory, this.fileSystem, targetDirectory);
        }

        /// <summary>
        /// Adds a directory to the library
        /// </summary>
        /// <param name="sourceDirectory">Directory to add</param>
        /// <param name="sourceFileSystem">File system on which the directory is located</param>
        /// <param name="name">What the newly added directory should be named once added to the library</param>
        public async Task AddDirectoryAsync(string sourceDirectory, IFileSystem sourceFileSystem, string name)
        {
            var targetDirectory = Path.Combine(this.directory, name);
            await sourceFileSystem.CopyDirectoryAsync(sourceDirectory, this.fileSystem, targetDirectory).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a file to the library
        /// </summary>
        /// <param name="sourceFilename">File to add</param>
        /// <param name="sourceFileSystem">File system on which the file is located</param>
        /// <param name="name">What the newly added file should be named once added to the library</param>
        public void AddFile(string sourceFilename, IFileSystem sourceFileSystem, string name)
        {
            var targetPath = Path.Combine(this.directory, name);
            using var sourceFile = sourceFileSystem.OpenFileReadOnly(sourceFilename);
            using var destFile = this.fileSystem.OpenFileWriteOnly(targetPath);
            sourceFile.CopyTo(destFile);
        }

        /// <summary>
        /// Adds a file to the library
        /// </summary>
        /// <param name="sourceFilename">File to add</param>
        /// <param name="sourceFileSystem">File system on which the file is located</param>
        /// <param name="name">What the newly added file should be named once added to the library</param>
        public async Task AddFileAsync(string sourceFilename, IFileSystem sourceFileSystem, string name)
        {
            var targetPath = Path.Combine(this.directory, name);
            using var sourceFile = sourceFileSystem.OpenFileReadOnly(sourceFilename);
            using var destFile = this.fileSystem.OpenFileWriteOnly(targetPath);
            await sourceFile.CopyToAsync(destFile).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes an item from the library
        /// </summary>
        /// <param name="name">Name of the item to remove</param>
        public void Remove(string name)
        {
            var path = Path.Combine(this.directory, name);
            if (this.fileSystem.DirectoryExists(path))
            {
                this.fileSystem.DeleteDirectory(path);
            }
            if (this.fileSystem.FileExists(path))
            {
                this.fileSystem.DeleteFile(path);
            }
        }
    }
}
