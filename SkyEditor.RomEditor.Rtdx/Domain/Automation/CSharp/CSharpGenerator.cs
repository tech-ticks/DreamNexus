using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SkyEditor.RomEditor.Domain.Automation.CSharp
{
    public interface ICSharpGenerator : IScriptGenerator
    {
    }

    public class CSharpGenerator : ICSharpGenerator
    {
        public CSharpGenerator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Generates a simple script to modify a simple object
        /// </summary>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <param name="source">The unmodified object to be used as a baseline</param>
        /// <param name="modified">The modified object</param>
        /// <param name="variableName">Name of the C# variable</param>
        /// <param name="indentLevel">Level of indentation to apply to each line</param>
        /// <returns>A C# script segment</returns>
        public string GenerateSimpleObjectDiff<T>(T source, T modified, string variableName, int indentLevel)
        {
            var script = new StringBuilder();
            var indent = GenerateIndentation(indentLevel);

            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                var sourceValue = property.GetValue(source);
                var targetValue = property.GetValue(modified);
                if (sourceValue.Equals(targetValue))
                {
                    continue;
                }

                script.Append(indent);
                script.Append(variableName);
                script.Append(".");
                script.Append(property.Name);
                script.Append(" = ");
                script.Append(GenerateExpression(targetValue));
                script.Append(";");
                script.AppendLine();
            }
            return script.ToString();
        }

        /// <summary>
        /// Represent the given value as a C# expression
        /// </summary>
        public string GenerateExpression(object? value)
        {
            if (value is null)
            {
                return "null";
            }
            else if (value is bool valueBool)
            {
                return valueBool ? "true" : "false";
            }
            else if (value is string)
            {
                return $"\"{value}\"";
            }
            else if (value is byte)
            {
                return $"{value}";
            }
            else if (value is short)
            {
                return $"{value}";
            }
            else if (value is ushort)
            {
                return $"{value}u";
            }
            else if (value is int)
            {
                return $"{value}";
            }
            else if (value is uint)
            {
                return $"{value}u";
            }
            else if (value is long)
            {
                return $"{value}l";
            }
            else if (value is ulong)
            {
                return $"{value}ul";
            }
            else if (value is float)
            {
                // This is not the best way to do it, but it's the safest.
                // 1.1 is a float
                // 1 is not
                // 1.1f is legal
                // 1f is not
                return $"(float){value}";
            }
            else if (value is double)
            {
                return $"(double){value}";
            }
            else
            {
                var type = value.GetType();
                if (type.IsGenericType && type.GetGenericTypeDefinition() == NullableType)
                {
                    var hasValue = (bool)type.GetProperty("HasValue").GetValue(value);
                    if (hasValue)
                    {
                        return GenerateExpression(type.GetProperty("Value").GetValue(value));
                    }
                    else
                    {
                        return GenerateExpression(null);
                    }
                }
                var converterAttribute = type.GetCustomAttribute<CSharpExpressionGeneratorAttribute>();
                if (converterAttribute != null)
                {
                    var generatorType = converterAttribute.Generator;
                    var converter = (ICSharpExpressionGenerator)serviceProvider.GetRequiredService(generatorType);
                    return converter.Generate(value);
                }
            }

            throw new ArgumentException($"Unsupported variable type: {value.GetType().FullName}");
        }
        private static readonly Type NullableType = typeof(Nullable<>);

        public static string GenerateIndentation(int indentLevel)
        {
            // Spaces only (sorry tabs gang)
            // Probably best to add support for tabs here later to avoid flame wars
            var data = new byte[4 * indentLevel];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0x20;
            }
            return Encoding.ASCII.GetString(data);
        }
    }
}
