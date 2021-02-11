using DevIO.Api.Extensions;
using Elmah.Io.AspNetCore;
using Elmah.Io.AspNetCore.HealthChecks;
using Elmah.Io.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configurations
{
    public static class LoggerConfig
    {
        public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Aqui ele pega exceções, erros, pega tudo de mais grave por si mesmo
            services.AddElmahIo(o =>
            {
                o.ApiKey = "93bc121cd08b47f4bb5820bf372d13f4";
                o.LogId = new Guid("5b41f017-bc60-4a17-ad63-a9c0b2a4081d");
            });

            // Para habilitar o elmah.io com o logging do asp net core
            // Colocando o Elmah como um provider
            /*services.AddLogging(builder =>
            {
                builder.AddElmahIo(o =>
                {
                    o.ApiKey = "93bc121cd08b47f4bb5820bf372d13f4";
                    o.LogId = new Guid("5b41f017-bc60-4a17-ad63-a9c0b2a4081d");
                });

                // No caso seria o Warning pra cima (Error e Critical Error)
                builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
            });*/

            services.AddHealthChecks()
                .AddElmahIoPublisher(options =>
                {
                    options.ApiKey = "93bc121cd08b47f4bb5820bf372d13f4";
                    options.LogId = new Guid("5b41f017-bc60-4a17-ad63-a9c0b2a4081d");
                    options.HeartbeatId = "3fd2412e57d143819215adad43386ee3";
                })
                .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
                .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "BancoSQL");

            services
                .AddHealthChecksUI(opt =>
                {
                    opt.SetEvaluationTimeInSeconds(15); //time in seconds between check
                    opt.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                    //opt.SetApiMaxActiveRequests(1); //api requests concurrency

                    opt.AddHealthCheckEndpoint("default api", "/healthz"); //map health check api
                })
                .AddInMemoryStorage();

            return services;
        }

        public static IApplicationBuilder UseLoggingConfiguration(this IApplicationBuilder app)
        {
            app.UseElmahIo();

            return app;
        }
    }
}
