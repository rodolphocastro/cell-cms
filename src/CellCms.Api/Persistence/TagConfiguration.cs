using CellCms.Api.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CellCms.Api.Persistence
{
    /// <summary>
    /// Configurações para Tags.
    /// </summary>
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            // Definindo a Tabela
            builder.ToTable("Tags");

            // Chave Primária
            builder.HasKey(t => t.Id);

            // Propriedades
            builder
                .Property(t => t.Nome)
                .HasMaxLength(250)
                .IsRequired(true);

            // Relacionamentos
            // Importante: Note que o relacionamento com Feed foi configurado pelo FeedConfiguration
            // e o Relacionamento entre Tags e Content está no ContentTag configuration
            // então aqui não precisamos fazer nada ;)

        }
    }
}
