using Microsoft.Extensions.DependencyInjection;
using SkyEditor.RomEditor.Domain.Automation.CSharp;
using SkyEditor.RomEditor.Domain.Automation.Lua;
using SkyEditor.RomEditor.Domain.Rtdx;
using System;

namespace SkyEditor.RomEditor.Infrastructure.Internal
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
                .AddSingleton<ICSharpGenerator, CSharpGenerator>()
                .AddCustomScriptExpressionGeneratorsAsSingleton();

            serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
