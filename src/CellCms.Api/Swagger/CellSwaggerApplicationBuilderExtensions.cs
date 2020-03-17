using CellCms.Api.Settings;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extensions para expor os middlewares do Swashbuckle.
    /// </summary>
    public static class CellSwaggerApplicationBuilderExtensions
    {
        /// <summary>
        /// Adiciona o middleware para o Swagger.json
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCellSwaggerJson(this IApplicationBuilder app)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseSwagger();

            return app;
        }

        /// <summary>
        /// Adiciona e configura o middleware para o SwaggerUI.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCellSwaggerUi(this IApplicationBuilder app, IConfiguration configuration)
        {
            if (app is null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var aadSettings = configuration.GetSection(AzureAdSettings.SettingsKey).Get<AzureAdSettings>();

            app.UseSwaggerUI(cfg =>
            {
                cfg.RoutePrefix = string.Empty;
                cfg.SwaggerEndpoint("/swagger/v1/swagger.json", "Cell CMS API");
                cfg.OAuthClientId(aadSettings.ClientId);
            });

            return app;
        }
    }
}
