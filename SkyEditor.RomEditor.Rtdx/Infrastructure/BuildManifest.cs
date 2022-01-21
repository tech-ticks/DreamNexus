using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace SkyEditor.RomEditor.Infrastructure
{
    public class BuildManifest
    {
        public Dictionary<string, string> HashToFilename { get; set; } = new Dictionary<string, string>();
    }
}
