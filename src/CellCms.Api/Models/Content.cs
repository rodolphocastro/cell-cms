using System;
using System.Collections.Generic;

namespace CellCms.Api.Models
{
    /// <summary>
    /// Conteúdo do CellCMS.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Chave primária.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Momento de criação do conteúdo.
        /// </summary>
        public DateTimeOffset DthCriacao { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Feed ao qual pertence.
        /// </summary>
        public Feed Feed { get; set; }

        /// <summary>
        /// Chave do Feed.
        /// </summary>
        public int FeedId { get; set; }

        /// <summary>
        /// Título do Conteúdo.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Conteúdo em si.
        /// </summary>
        public string Corpo { get; set; }

        /// <summary>
        /// Tags relacionadas ao conteúdo.
        /// </summary>
        public ICollection<ContentTag> ContentTags { get; set; }
    }
}
