using CellCms.Api.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CellCms.Api.Persistence
{
    /// <summary>
    /// Configuração do Feed.
    /// </summary>
    public class FeedConfiguration : IEntityTypeConfiguration<Feed>
    {
        public void Configure(EntityTypeBuilder<Feed> builder)
        {
            // Definindo o nome da tabela que será gerada
            builder.ToTable("Feeds");

            // Mapeando a chave primária
            builder.HasKey(f => f.Id);

            // Mapeando as propriedades do Feed
            builder
                .Property(f => f.Nome)
                .HasMaxLength(250)
                .IsRequired(true);

            // Mapeando os relacionamentos
            // Importante notar: Precisamos mapear apenas UM dos lados do relacionamento!
            builder
                .HasMany(f => f.Tags)
                .WithOne(t => t.Feed)
                    .HasForeignKey(t => t.FeedId);

            builder
                .HasMany(f => f.Contents)
                .WithOne(c => c.Feed)
                    .HasForeignKey(c => c.FeedId);
        }
    }
}
