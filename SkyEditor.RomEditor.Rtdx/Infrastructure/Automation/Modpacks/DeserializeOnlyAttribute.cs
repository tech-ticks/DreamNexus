using System;

namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class DeserializeOnlyAttribute : Attribute
    {
    }
}
