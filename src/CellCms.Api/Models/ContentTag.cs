namespace CellCms.Api.Models
{
    /// <summary>
    /// Relacionamento entre Conteúdos e Tags.
    /// </summary>
    public class ContentTag
    {
        /// <summary>
        /// Conteúdo.
        /// </summary>
        public Content Content { get; set; }

        /// <summary>
        /// Chave do Conteúdo.
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Tag.
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// Chave da tag.
        /// </summary>
        public int TagId { get; set; }
    }
}
