using Microsoft.Extensions.DependencyInjection;
using SkyEditor.RomEditor.Rtdx.Domain.Automation.Lua;
using System;
using System.Linq;

namespace SkyEditor.RomEditor.Rtdx.Infrastructure.Internal
{
    internal static class ServiceCollectionExtensions
    {
        private static Type LuaValueGeneratorInterfaceType = typeof(ILuaExpressionGenerator);

        /// <summary>
        /// Registers implementations of <see cref="ILuaExpressionGenerator"/> that are located in the same assembly as the interace
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection AddCustomLuaValueGeneratorsAsSingleton(this IServiceCollection services)
        {
            var types = typeof(ILuaExpressionGenerator)
                .Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && LuaValueGeneratorInterfaceType.IsAssignableFrom(t));
            foreach (var type in types)
            {
                services.AddSingleton(type);
            }
            return services;
        }
    }
}
