using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkyEditor.RomEditor.Domain.Library
{
    public class LibraryItem
    {
        public LibraryItem(string fullPath)
        {
            this.FullPath = fullPath ?? throw new ArgumentNullException();
            this.Name = Path.GetFileName(fullPath);
        }
        public string Name { get; }
        public string FullPath { get; }
    }
}
