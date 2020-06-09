﻿using SkyEditor.IO.FileSystem;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Infrastructure.Interfaces
{
    public interface ISaveableToDirectory
    {
        Task Save(string directory, IFileSystem fileSystem);
    }
}
