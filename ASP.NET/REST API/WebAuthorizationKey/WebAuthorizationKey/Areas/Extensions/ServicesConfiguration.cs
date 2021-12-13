using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using WebAuthorizationKey.Properties;

namespace WebAuthorizationKey.Areas.Extensions
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Startup.ASSEMBLY_NAME, Version = "v1" });

                var xmlFileName = $"{Startup.ASSEMBLY_NAME}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

                var jwtSecurityScheme = new OpenApiSecurityScheme()
                {
                    Name = HeaderNames.Authorization,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    // Nagłówek autoryzacji JWT przy użyciu schematu Bearer. Przykład: "{token}".
                    Description = Resources.Swagger_AuthorizationDescription,
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme , Array.Empty<string>() }
                });
            });
            return services;
        }
    }
}
