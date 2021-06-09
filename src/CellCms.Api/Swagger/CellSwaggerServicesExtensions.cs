using System;

using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions para a configuração de services relacionados ao Swagger.
    /// </summary>
    public static class CellSwaggerServicesExtensions
    {
        /// <summary>
        /// Configura e adiciona os serviços de geração do Swagger.Json do Cell CMS.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCellSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var aadSettings = configuration.GetAzureAdSettings();

            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Cell CMS",
                    Description = "API para o Cell CMS",
                    Version = "v1",
                    Contact = new OpenApiContact { Name = "Rodolpho Alves", Url = new Uri("https://github.com/rodolphocastro/cell-cms") }
                });

                cfg.AddSecurityDefinition("apiKey", new()
                {
                    In = ParameterLocation.Header,
                    Name = "ApiKey"
                });

                cfg.AddSecurityRequirement(new()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference() { Type = ReferenceType.SecurityScheme, Id = "apiKey" }
                        },
                        new[] { "readAccess, writeAccess" }
                    }
                });

            });

            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}
