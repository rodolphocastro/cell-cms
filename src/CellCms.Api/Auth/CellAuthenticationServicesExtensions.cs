using System;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions para configuração dos services de autenticação.
    /// </summary>
    public static class CellAuthenticationServicesExtensions
    {
        /// <summary>
        /// Adiciona e configura os serviços para autenticação do usuário.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCellAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var aadSettings = configuration.GetAzureAdSettings();

            services
                .AddAuthentication(c =>
                {
                    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;   // Definindo que os JWTs serão nosso schema de autenticação padrão
                    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Definindo que os JWTs serão nosso schema padrão para challenges de autenticação
                })
                .AddJwtBearer(c =>
                {
                    c.IncludeErrorDetails = true;   // Garantindo que o Middleware nos retorno erros detalhados, em produção pode ser melhor desabilitar
                    c.SaveToken = true; // Indica que o Token deve ser salvo
                    // As linhas abaixo configuram a validação do Token, com base nas nossas configurações
                    c.MetadataAddress = aadSettings.MetadataEndpoint;
                    c.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = aadSettings.ClientId,
                        ValidateAudience = true,
                        // Na documentação (https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-convert-app-to-be-multi-tenant#update-your-code-to-handle-multiple-issuer-values)
                        // recomenda-se validar Guid-a-Guid os issuers para cada tenant que puder utilizar nosso App.
                        // Como nossa ideia aqui é fazer apenas o App, não vamos fazer este passo
                        ValidateIssuer = false
                    };
                });

            return services;
        }
    }
}
