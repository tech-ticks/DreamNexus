namespace SkyEditor.RomEditor.Domain.Automation
{
    public interface IScriptGenerator
    {
        /// <summary>
        /// Generates a simple script to modify a simple object
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="source">The unmodified object to be used as a baseline</param>
        /// <param name="modified">The modified object</param>
        /// <param name="variableName">Name of the variable</param>
        /// <param name="indentLevel">Level of indentation to apply to each line</param>
        /// <returns>A script segment</returns>
        string GenerateSimpleObjectDiff<T>(T source, T modified, string variableName, int indentLevel);

        /// <summary>
        /// Represent the given value as an expression
        /// </summary>
        string GenerateExpression(object? value);
    }
}
