using CellCms.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CellCms.Api.Persistence
{
    /// <summary>
    /// Configurações para Contents.
    /// </summary>
    public class ContentConfiguration : IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            // Tabela
            builder.ToTable("Contents");

            // Chave
            builder.HasKey(c => c.Id);

            // Propriedades
            builder.Property(c => c.DthCriacao);

            builder
                .Property(c => c.Titulo)
                .HasMaxLength(255)
                .IsRequired(true);

            builder
                .Property(c => c.Corpo)
                .HasMaxLength(3000)
                .IsRequired(true);

            // Importante: Os relacionamentos já foram definidos nos outros arquivos!
        }
    }
}
