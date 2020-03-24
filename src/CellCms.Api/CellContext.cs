using CellCms.Api.Models;

using Microsoft.EntityFrameworkCore;

namespace CellCms.Api
{
    /// <summary>
    /// Context para a API do CellCMS.
    /// </summary>
    public class CellContext : DbContext
    {
        /// <summary>
        /// Chave para buscar a ConnectionString do Context.
        /// </summary>
        public const string ConnectionStringKey = "CellCmsContext";

        /// <summary>
        /// Novo context para o Cell CMS.
        /// </summary>
        /// <param name="options">Opções a serem definidas para o Context</param>
        public CellContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Tabela de Feeds.
        /// </summary>
        public DbSet<Feed> Feeds { get; protected set; }

        /// <summary>
        /// Tabela de Tags.
        /// </summary>
        public DbSet<Tag> Tags { get; protected set; }

        /// <summary>
        /// Tabela de Contents.
        /// </summary>
        public DbSet<Content> Contents { get; protected set; }

        /// <summary>
        /// Configuração da criação dos Models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

    }
}
