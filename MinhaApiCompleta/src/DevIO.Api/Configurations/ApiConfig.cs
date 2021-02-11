using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configurations
{
    public static class ApiConfig
    {
        public static IServiceCollection WebApiConfig(this IServiceCollection services)
        {

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2, 0); // 2.0
                options.ReportApiVersions = true; // ao usar essa api, vai passar no header se essa versão está ok ou obsoleta
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // VVV - Major - Minor - Patch
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Retirando a validação do ModelState automatico
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );

                options.AddPolicy("Production",
                    builder => builder
                    .WithMethods("GET")
                    .WithOrigins("http://meudominio.com")
                    .SetIsOriginAllowedToAllowWildcardSubdomains() // Meus subdominos vão ter total acesso
                    .WithHeaders(HeaderNames.ContentType, "x-custom-header")
                    .AllowAnyHeader()
                );
            });

            return services;
        }

        public static IApplicationBuilder UseMvcConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            // Para trabalhar só com um ambiente, posso colocar aqui a configuração
            // Mais de uma configuração do cors para diferentes ambientes precisa colocar na Startup
            //app.UseCors("Development");

            return app;
        }
    }
}
