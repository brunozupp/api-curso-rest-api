using DevIO.Api.Data;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DevIO.Api.Extensions;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DevIO.Api.Configurations
{
    public static class IdentityConfig
    {
        public static  IServiceCollection AddIdentityConfiguration(
            this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                 .AddRoles<IdentityRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddErrorDescriber<IdentityMensagensPortugues>()
                 .AddDefaultTokenProviders();

            // JWT

            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                // Toda vez que autenticar alguém, o padrão de autenticação é pra gerar um token
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // E toda vez que for validar o token
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                // Se ter 100% de certeza que só vai trabalhar com https, pode usar true
                x.RequireHttpsMetadata = true;
                // Se o token deve ser guardado no HttpAuthenticationProperties após sucesso
                x.SaveToken = true;

                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // Vai validar se quem está emitindo é o mesmo de quem recebeu
                    ValidateIssuerSigningKey = true,
                    // Configurando a chave para criptografia
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // Validar quem emitiu o token
                    ValidateIssuer = true,
                    // Onde o token é válido, em qual url
                    ValidateAudience = true,
                    // localhost ou a url da minha aplicação
                    ValidAudience = appSettings.ValidoEm,
                    // nome da minha aplicação, quem emite esse token
                    ValidIssuer = appSettings.Emissor,
                };
            });

            return services;
        }
    }
}
