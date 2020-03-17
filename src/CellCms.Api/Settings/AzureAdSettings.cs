namespace CellCms.Api.Settings
{
    /// <summary>
    /// Configurações do Azure AD.
    /// </summary>
    public class AzureAdSettings : IAzureAdSettings
    {
        /// <summary>
        /// Chave para buscar as configurações.
        /// </summary>
        public const string SettingsKey = "AzureAd";

        public string ClientId { get; set; }

        public string TokenEndpoint { get; set; }

        public string AuthorizeEndpoint { get; set; }

        public string MetadataEndpoint { get; set; }
    }
}
