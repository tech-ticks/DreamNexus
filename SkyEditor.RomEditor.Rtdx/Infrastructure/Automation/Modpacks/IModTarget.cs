namespace SkyEditor.RomEditor.Infrastructure.Automation.Modpacks
{
    public interface IModTarget
    {
        string RomDirectory { get; }

        void WriteFile(string relativePath, byte[] data);
    }
}
