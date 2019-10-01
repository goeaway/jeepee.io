using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Behaviours;
using Jeepee.IO.Receiver.Application.Providers;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddLogger();
            services.AddMediatR(Assembly.GetAssembly(typeof(ISystem)));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionWrapperBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestLoggerBehaviour<,>));
            services.AddTransient<IChannelProvider, ChannelProvider>();
            services.AddSystem();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime, ISystem system)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseExceptionHandler(ExceptionHandler);

            //app.UseHttpsRedirection();
            //app.UseAuthentication();
            app.UseMvc();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            // register stopping event (dispose of pi camera stream)
            lifetime.ApplicationStopping.Register(OnAppStopping, system);
        }

        private void ExceptionHandler(IApplicationBuilder app)
        {
            app.Run(ctx =>
            {
                return Task.Run(async () =>
                {
                    ctx.Response.StatusCode = 500;
                    var exHandlerPathFeature = ctx.Features.Get<IExceptionHandlerPathFeature>();

                    var exception = exHandlerPathFeature.Error;
                    var uri = ctx.Request.Path;

                    var logger = app.ApplicationServices.GetService<ILogger>();
                    logger.Error(exception, "Error occured when processing request {uri}", uri);

                    await ctx.Response.WriteAsync($"Error Ocurred: {exception.Message}. {(exception.InnerException?.Message)}");
                });
            });
        }

        private void OnAppStopping(object system)
        {
            ((IDisposable)system).Dispose();
        }
    }
}
