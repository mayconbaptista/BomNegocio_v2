using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BuildBlocks.WebApi.Extensions
{
    public static class LoggerExtension
    {
        public static IServiceCollection AddLoggerConfiguration (this IServiceCollection services, IConfiguration config)
        {


            return services;
        }
    }
}
