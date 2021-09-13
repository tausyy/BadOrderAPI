using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Settings
{
    public static class ConfigureMongoDbExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection service, IConfiguration mongoConfiguration)
        {
            return service;
        }
    }
}
