using System.Collections.Generic;
using System.Linq;

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

    /// <summary>
    /// Queries relacionadas a Feeds.
    /// </summary>
    public static class FeedQueries
    {
        /// <summary>
        /// Obtem todos os feeds.
        /// </summary>
        /// <param name="feeds"></param>
        /// <returns></returns>
        public static IQueryable<Feed> AllFeeds(this IQueryable<Feed> feeds) => feeds;

        /// <summary>
        /// Filtra feeds com base no ID.
        /// </summary>
        /// <param name="feeds"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IQueryable<Feed> WithId(this IQueryable<Feed> feeds, int id)
        {
            return feeds
                .Where(f => f.Id.Equals(id));
        }

        /// <summary>
        /// Filtra feeds com base no Nome.
        /// </summary>
        /// <param name="feeds"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IQueryable<Feed> FilterByNome(this IQueryable<Feed> feeds, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return feeds;
            }

            return feeds
                .Where(f => f.Nome.Contains(name, System.StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
