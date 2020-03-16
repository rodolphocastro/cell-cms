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
    }
}
