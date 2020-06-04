using Microsoft.Extensions.DependencyInjection;
using SkyEditor.RomEditor.Domain.Automation;
using System;
using System.Linq;

namespace SkyEditor.RomEditor.Infrastructure.Internal
{
    internal static class ServiceCollectionExtensions
    {
        private static readonly Type ScriptValueGeneratorInterfaceType = typeof(IScriptExpressionGenerator);

        /// <summary>
        /// Registers implementations of <see cref="ILuaExpressionGenerator"/> that are located in the same assembly as the interace
        /// </summary>
        internal static IServiceCollection AddCustomScriptExpressionGeneratorsAsSingleton(this IServiceCollection services)
        {
            var types = ScriptValueGeneratorInterfaceType
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && ScriptValueGeneratorInterfaceType.IsAssignableFrom(t));
            foreach (var type in types)
            {
                services.AddSingleton(type);
            }
            return services;
        }
    }
}
