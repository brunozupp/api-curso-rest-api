using AutoMapper;
using DevIO.Api.Configurations;
using DevIO.Api.Extensions;
using DevIO.Data.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace DevIO.Api
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
            services.AddDbContext<MeuDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentityConfiguration(Configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.WebApiConfig(); // método estático

            services.AddSwaggerConfig();

            services.AddLoggingConfiguration(Configuration);

            // Ficava aqui o serviço do HealthCheck. Agora dentro de AddLoggingConfiguration

            services.ResolveDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                // Sempre chamar o cors antes do UseMvcConfiguration que está
                // O Cors é implementado pelo browser, então seu eu acessar pelo postman
                // aquele mais restritivo (Production), vai funcionar
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseCors("Production");
                app.UseHsts();
            }

            // Vir antes da config do Mvc
            app.UseAuthentication();

            // Vir antes da config do Mvc
            // Talvez não precise desse Middleware pq estava funcionando os erros com throw
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseMvcConfiguration(); // método estático

            app.UseSwaggerConfig(provider);

            app.UseLoggingConfiguration();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //adding endpoint of health check for the health check ui in UI format
                endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                //map healthcheck ui endpoing - default is /healthchecks-ui
                endpoints.MapHealthChecksUI();

                endpoints.MapControllers();
            });
        }
    }
}
