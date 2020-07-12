using Jeepee.IO.Receiver.Application;
using Jeepee.IO.Receiver.Application.Options;
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

        public static IServiceCollection AddHardwareOptions(this IServiceCollection collection, IConfiguration configuration)
        {
            var options = new HardwareOptions();
            configuration.GetSection(HardwareOptions.Key).Bind(options);
            return collection.AddSingleton(options);
        }
    }
}
