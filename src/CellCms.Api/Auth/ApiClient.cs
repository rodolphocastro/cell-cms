using System;
using System.Linq;

namespace CellCms.Api.Auth
{
    /// <summary>
    /// Um cliente autorizado a chamar a API.
    /// </summary>
    public record ApiClient(string Name, string Key) : IEquatable<string>
    {
        public virtual bool Equals(string other) => Key.Equals(other, StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Queries para consultar ApiClients.
    /// </summary>
    public static class ApiClientQueries
    {
        /// <summary>
        /// Filtra clients baseados em uma key.
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IQueryable<ApiClient> WithKey(this IQueryable<ApiClient> clients, string key) => clients.Where(c => c.Equals(key));

        /// <summary>
        /// Query para verificar se existe um client para uma key específica.
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsAuthorized(this IQueryable<ApiClient> clients, string key) => clients.WithKey(key).Any();
    }
}
