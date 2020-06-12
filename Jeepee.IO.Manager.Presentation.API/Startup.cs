using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Jeepee.IO.Core.Exceptions;
using Jeepee.IO.Manager.Application;
using Jeepee.IO.Manager.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

namespace Jeepee.IO.Manager.Presentation.API
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
            services.AddControllers()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<IJeepeeControlStore>();
                });
            services.AddConfiguration(Configuration);
            services.AddMediatR(typeof(IJeepeeControlStore));
            services.AddSingleton<IJeepeeControlStore>(new JeepeeControlStore());
            services.AddTransient(serviceProvider => Log.Logger);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(ExceptionHandler);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ExceptionHandler(IApplicationBuilder app) => app.Run(async ctx =>
        {
            ctx.Response.StatusCode = 500;
            ctx.Response.ContentType = "application/json";
            var exHandlerPathFeature = ctx.Features.Get<IExceptionHandlerFeature>();
            var exception = exHandlerPathFeature.Error;
            var uri = ctx.Request.Path;

            var logger = app.ApplicationServices.GetService<ILogger>();
            logger.Error(exception, "Error occurred when processing request {uri}", uri);

            var errorList = new List<string> { exception.Message, exception.StackTrace };

            if (exception.InnerException != null)
            {
                errorList.Add(exception.InnerException.Message);
            }

            if (exception is JeepeeHTTPException)
            {
                var requestFailedException = exception as JeepeeHTTPException;
                ctx.Response.StatusCode = (int)requestFailedException.StatusCode;
            }

            await ctx.Response.WriteAsync(JsonConvert.SerializeObject(errorList));
        });
    }
}
