using System.Collections.Generic;

namespace CellCms.Api.Models
{
    /// <summary>
    /// Tag de um Feed.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Chave primária.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Feed ao qual a Tag pertence.
        /// </summary>
        public Feed Feed { get; set; }

        /// <summary>
        /// Chave Primária do Feed.
        /// </summary>
        public int FeedId { get; set; }

        /// <summary>
        /// Nome da Tag.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Conteúdos relacionados à tag.
        /// </summary>
        public ICollection<ContentTag> ContentTags { get; set; }
    }
}
