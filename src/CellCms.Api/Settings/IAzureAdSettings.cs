namespace CellCms.Api.Settings
{
    /// <summary>
    /// Descreve as propriedades para configurar autenticação através do Azure AD.
    /// </summary>
    public interface IAzureAdSettings
    {
        /// <summary>
        /// ClientId esperado.
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Endpoint para obter access tokens.
        /// </summary>
        string TokenEndpoint { get; }

        /// <summary>
        /// Endpoint para obter autorização dos usuários.
        /// </summary>
        string AuthorizeEndpoint { get; }

        /// <summary>
        /// Endpoint para buscar o well-known document.
        /// </summary>
        string MetadataEndpoint { get; }
    }
}
