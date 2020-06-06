﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pippin;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddLogger();
            services.AddMediatR(Assembly.GetAssembly(typeof(UpdateChannelHandler)));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggerBehaviour<,>));
            services.AddConfiguration(Configuration);

            services.AddPippin(options =>
            {
                options.AddAdapter(() => new ActionPinAdapter((number, on) =>
                {
                    
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(ExceptionHandler);
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapHub<MonitorHub>("/monitor");
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
