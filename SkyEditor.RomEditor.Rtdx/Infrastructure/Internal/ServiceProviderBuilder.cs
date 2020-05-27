using Microsoft.Extensions.DependencyInjection;
using SkyEditor.RomEditor.Rtdx.Domain;
using SkyEditor.RomEditor.Rtdx.Domain.Automation.Lua;
using System;

namespace SkyEditor.RomEditor.Rtdx.Infrastructure.Internal
{
    internal static class ServiceProviderBuilder
    {
        internal static IServiceProvider CreateRtdxRomServiceProvider(IRtdxRom rom)
        {
            IServiceProvider? serviceProvider = null;
            var services = new ServiceCollection();
            services
                .AddSingleton(_ => serviceProvider ?? throw new Exception("Failed to register IServiceProvider within itself"))
                .AddSingleton<IRtdxRom>(rom)
                .AddSingleton<ICommonStrings>(_ => rom.GetCommonStrings())
                .AddSingleton<ILuaGenerator, LuaGenerator>()
                .AddCustomLuaValueGeneratorsAsSingleton();

            serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
