namespace SkyEditor.RomEditor.Domain.Automation.CSharp
{
    public static class CSharpSnippets
    {
        /// <summary>
        /// A guard to be placed at the top of the script to ensure the script is run within the context of Sky Editor
        /// </summary>
        public static readonly string RequireSkyEditor = @"
if (Rom == null)
    throw new System.Exception(""Script must be run in the context of Sky Editor"");
";
    }
}
