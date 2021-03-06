﻿using System;
using System.Collections.Generic;

using CellCms.Api.Constants;
using CellCms.Api.Swagger;

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

                cfg.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Azure AD",
                    OpenIdConnectUrl = new Uri(aadSettings.MetadataEndpoint),
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(aadSettings.AuthorizeEndpoint),
                            TokenUrl = new Uri(aadSettings.TokenEndpoint),
                            Scopes = new Dictionary<string, string>
                            {
                                { CellScopes.AcessoSwagger, "Permite que o usuário faça login no SwaggerUi" }
                            }
                        }
                    }
                });

                cfg.OperationFilter<SecurityRequirementsOperationFilter>();

            });

            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }
    }
}
