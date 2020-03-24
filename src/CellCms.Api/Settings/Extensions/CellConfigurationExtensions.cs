using System;

using CellCms.Api.Settings;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extensions para facilitar a busca de configurações do Cell CMS.
    /// </summary>
    public static class CellConfigurationExtensions
    {
        /// <summary>
        /// Recupera as configurações do Azure Active Directory.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>ao settings encontrados</returns>
        public static IAzureAdSettings GetAzureAdSettings(this IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var azureAdSettings = configuration.GetSection(AzureAdSettings.SettingsKey).Get<AzureAdSettings>();
            if (azureAdSettings is null)
            {
                throw new InvalidOperationException("As configurações do AzureAD são necessárias para a API");
            }

            return azureAdSettings;
        }
    }
}
