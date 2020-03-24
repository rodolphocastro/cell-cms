using System;
using System.Linq;

using CellCms.Api;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.AspNetCore.Hosting
{
    /// <summary>
    /// Extensions de persistência para o Host.
    /// </summary>
    public static class CellPersistenceHostExtensions
    {
        /// <summary>
        /// Flag para controle das Migrations.
        /// </summary>
        public const string MigrateFlagKey = "MigrateOnStartup";

        /// <summary>
        /// Garante que a Database esteja atualizada (migrada).
        /// </summary>
        /// <param name="host"></param>
        public static IHost EnsureDatabaseIsMigrated(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Startup>>();
                var context = services.GetRequiredService<CellContext>();
                var pending = context.Database.GetPendingMigrations();
                if (pending.Any())
                {
                    logger.LogWarning("Existem {0} migrations pendentes: {1}", pending.Count(), string.Join(",", pending));
                    var configuration = services.GetRequiredService<IConfiguration>();
                    bool migrateFlag = configuration.GetValue<bool>(MigrateFlagKey);
                    if (migrateFlag)
                    {
                        logger.LogWarning("Aplicando migrations");
                        context.Database.Migrate();
                    }
                    else
                    {
                        logger.LogError("Existem migrations pendente. Altere a flag {0} para True ou atualize a base manualmente", MigrateFlagKey);
                        throw new InvalidOperationException("Não é possível executar a API com a base desatualizada");
                    }
                }
            }

            return host;
        }
    }
}
