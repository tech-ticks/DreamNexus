namespace SkyEditor.RomEditor.Domain.Automation
{
    public interface IScriptExpressionGenerator
    {
        string Generate(object? value);
    }
}
