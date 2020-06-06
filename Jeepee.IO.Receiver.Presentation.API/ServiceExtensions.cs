using Jeepee.IO.Receiver.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Presentation.API
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddLogger(this IServiceCollection collection)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "log-{Date}.txt");
            var logger = new LoggerConfiguration()
                .WriteTo.RollingFile(path)
                .CreateLogger();

            collection.AddSingleton<ILogger>(logger);

            return collection;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection collection, IConfiguration configuration)
        {
            return collection.AddSingleton(configuration);
        }
    }
}
