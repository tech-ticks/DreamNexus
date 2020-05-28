using Autofac.Features.Metadata;
using SkyEditor.IO.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.RomEditor.Rtdx.Domain.Automation.Modpacks
{
    public class Mod
    {
        public Mod(ModMetadata metadata, string directory, IFileSystem fileSystem)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }
            if (fileSystem == null)
            {
                throw new ArgumentNullException(nameof(fileSystem));
            }

            this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            this.Enabled = metadata.Enabled;

            var scripts = new List<Script>();
            foreach (var scriptRelativePath in metadata.Scripts ?? Enumerable.Empty<string>())
            {
                var scriptAbsolutePath = Path.Combine(directory, scriptRelativePath);
                if (!fileSystem.FileExists(scriptAbsolutePath))
                {
                    throw new FileNotFoundException("Could not find a script at the path specified by the mod metadata", scriptAbsolutePath);
                }

                ScriptType scriptType;
                var extension = Path.GetExtension(scriptAbsolutePath);
                if (string.Equals(extension, ".csx", StringComparison.OrdinalIgnoreCase))
                {
                    scriptType = ScriptType.CSharp;
                }
                else if (string.Equals(extension, ".lua", StringComparison.OrdinalIgnoreCase))
                {
                    scriptType = ScriptType.Lua;
                }
                else
                {
                    throw new UnsupportedScriptTypeException(extension);
                }

                scripts.Add(new Script(scriptType, fileSystem.ReadAllText(scriptAbsolutePath)));
            }

            this.Scripts = scripts;
        }

        public ModMetadata Metadata { get; }
        public IReadOnlyList<Script> Scripts { get; }

        public bool Enabled { get; set; }

        public async Task Apply(SkyEditorScriptContext context)
        {
            foreach (var script in Scripts)
            {
                switch (script.Type) 
                {
                    case ScriptType.CSharp:
                        await context.ExecuteCSharp(script.Value).ConfigureAwait(false);
                        break;
                    case ScriptType.Lua:
                        context.ExecuteLua(script.Value);
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported script type: " + script.Type.ToString("f"));
                }
            }
        }
    }
}
