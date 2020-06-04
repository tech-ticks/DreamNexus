using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkyEditor.RomEditor.Infrastructure
{
    public static class ServiceProviderExtensions
    {
        public static T GetRequiredService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetRequiredService(typeof(T));
        }

        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            return (T)serviceProvider.GetService(typeof(T));
        }
    }
}
