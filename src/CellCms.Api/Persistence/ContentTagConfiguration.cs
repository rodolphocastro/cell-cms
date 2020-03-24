using CellCms.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CellCms.Api.Persistence
{
    /// <summary>
    /// Configurações para o N:N ContentTags
    /// </summary>
    public class ContentTagConfiguration : IEntityTypeConfiguration<ContentTag>
    {
        public void Configure(EntityTypeBuilder<ContentTag> builder)
        {
            // Definindo a tabela
            builder.ToTable("ContentTags");

            // Definindo a chave composta
            builder.HasKey(c => new { c.ContentId, c.TagId });

            // Relacionamentos
            // N:1 Content
            builder
                .HasOne(ct => ct.Content)
                .WithMany(c => c.ContentTags)
                .HasForeignKey(ct => ct.ContentId);

            // N:1 Tag
            builder
                .HasOne(ct => ct.Tag)
                .WithMany(t => t.ContentTags)
                .HasForeignKey(ct => ct.TagId);
        }
    }
}
