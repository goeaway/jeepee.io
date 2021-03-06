﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Jeepee.IO.Receiver.Application.Behaviours;
using Jeepee.IO.Receiver.Application.Commands;
using Jeepee.IO.Receiver.Presentation.API.Hubs;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pippin;
using Pippin.Adapters.Unosquare;
using Pippin.Core;
using Serilog;

namespace Jeepee.IO.Receiver.Presentation.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string AllowAllCorsPolicy = "allowall";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddLogger();
            services.AddMediatR(Assembly.GetAssembly(typeof(UpdateChannelHandler)));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggerBehaviour<,>));
            services.AddHardwareOptions(Configuration);
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllCorsPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost.app.com:30");
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                });
            });

            var env = Configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");

            services.AddPippin(options =>
            {
                if(env == "Development")
                {
                    options.AddAdapter(sp => new DebugAdapter(sp.GetRequiredService<ILogger>()));
                } 
                else
                {
                    options.AddSingletonAdapter(new UnosquarePinAdapter());
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(AllowAllCorsPolicy);
            app.UseExceptionHandler(ExceptionHandler);
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<JeepeeHub>("/jeepeehub");
            });
        }

        private void ExceptionHandler(IApplicationBuilder app) => app.Run(async ctx =>
        {
            ctx.Response.StatusCode = 500;
            var exHandlerPathFeature = ctx.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exHandlerPathFeature.Error;
            var uri = ctx.Request.Path;

            var logger = app.ApplicationServices.GetService<ILogger>();
            logger.Error(exception, "Error occurred when processing request {uri}", uri);

            await ctx.Response.WriteAsync($"Error Occurred: {exception.Message}. {(exception.InnerException?.Message)}");
        });
    }
}
