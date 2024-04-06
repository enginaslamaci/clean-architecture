using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Helpers
{
    public class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public static void CreateServiceProvider(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
