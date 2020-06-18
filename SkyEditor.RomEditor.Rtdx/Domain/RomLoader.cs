using DotNet3dsToolkit;
using SkyEditor.IO.Binary;
using SkyEditor.IO.FileSystem;
using SkyEditor.RomEditor.Domain.Psmd;
using SkyEditor.RomEditor.Domain.Rtdx;
using SkyEditor.RomEditor.Infrastructure.Automation.Modpacks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Domain
{
    public static class RomLoader
    {
        /// <summary>
        /// Loads a ROM from the given path if supported
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileSystem"></param>
        /// <returns>The ROM that was loaded, or null if the given path is not supported</returns>
        public static async Task<IModTarget?> LoadRom(string path, IFileSystem fileSystem)
        {
            if (fileSystem.FileExists(path))
            {
                bool is3dsRom;

                BinaryFile? file = null;
                bool disposeFile = true;
                try
                {
                    file = new BinaryFile(path, fileSystem);
                    is3dsRom = await ThreeDsRom.IsThreeDsRom(file).ConfigureAwait(false);

                    if (is3dsRom)
                    {
                        var rom = await ThreeDsRom.Load(file).ConfigureAwait(false);
                        // To-do: check title ID to ensure it's a PSMD ROM
                        // For now, just assume it's supported by our class
                        
                        disposeFile = false;
                        return new PsmdRom(rom);
                    }
                    else
                    {
                        return null;
                    }
                }
                finally
                {
                    if (disposeFile && file != null)
                    {
                        file.Dispose();
                    }
                }                
            }
            else if (fileSystem.DirectoryExists(path))
            {
                // Is it a Switch ROM or a 3DS ROM?
                // To-do: check title ID to ensure it's the correct type of ROM
                if (fileSystem.FileExists(Path.Combine(path, "ExeFS", "code.bin")))
                {
                    // It's a 3DS ROM
                    return new PsmdRom(path, fileSystem);
                }
                else if (fileSystem.FileExists(Path.Combine(path, "exefs", "main")))
                {
                    // It's a Switch ROM
                    return new RtdxRom(path, fileSystem);
                }
                else
                {
                    // We couldn't find the executable and can make no conclusion
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
