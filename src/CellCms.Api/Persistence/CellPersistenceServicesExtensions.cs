using System;

using CellCms.Api;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions para configurar a persistência do Cell CMS.
    /// </summary>
    public static class CellPersistenceServicesExtensions
    {
        /// <summary>
        /// Adiciona e configura o context para persistência.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCellPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            string connString = configuration.GetConnectionString(CellContext.ConnectionStringKey);
            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new InvalidOperationException($"Connection String {CellContext.ConnectionStringKey} não encontrada");
            }

            services.AddDbContext<CellContext>(opt => opt.UseSqlite(connString));

            return services;
        }
    }
}
