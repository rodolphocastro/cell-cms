using System.Collections.Generic;

namespace CellCms.Api.Models
{
    /// <summary>
    /// Feed para conteúdos do Cell CMS.
    /// </summary>
    public class Feed
    {
        /// <summary>
        /// Chave primária do Feed.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do Feed.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tags pertencentes ao Feed
        /// </summary>
        public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        /// <summary>
        /// Conteúdos do Feed.
        /// </summary>
        public ICollection<Content> Contents { get; set; } = new HashSet<Content>();
    }
}
