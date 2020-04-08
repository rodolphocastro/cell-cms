using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CellCms.Api.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CellCms.Api.Features.Tags
{
    /// <summary>
    /// Query para listas as tags de um Feed.
    /// </summary>
    public class ListTags : IRequest<IEnumerable<Tag>>
    {
        [FromQuery(Name = "feedId")]
        public int? FeedId { get; set; } = null;
    }

    public class ListTagsHandler : IRequestHandler<ListTags, IEnumerable<Tag>>
    {
        private readonly CellContext _context;

        public ListTagsHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<IEnumerable<Tag>> Handle(ListTags request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return ListTagsInternalAsync(request.FeedId, cancellationToken);
        }

        private Task<IEnumerable<Tag>> ListTagsInternalAsync(int? feedId, CancellationToken cancellationToken)
        {
            if (feedId is int id)
            {
                return ListTagsInternalAsync(id, cancellationToken);
            }

            return ListTagsInternalAsync(cancellationToken);
        }

        private async Task<IEnumerable<Tag>> ListTagsInternalAsync(CancellationToken cancellationToken)
        {
            var result = await _context
                .Tags
                .AsNoTracking()
                .Include(t => t.Feed)
                .ToListAsync(cancellationToken);

            return result;
        }

        private async Task<IEnumerable<Tag>> ListTagsInternalAsync(int feedId, CancellationToken cancellationToken)
        {
            var existingFeed = await _context
                .Feeds
                .AsNoTracking()
                .Include(f => f.Tags)
                .SingleOrDefaultAsync(f => f.Id == feedId, cancellationToken);

            if (existingFeed is null)
            {
                throw new KeyNotFoundException($"Não foi encontrado nenhum feed com id {feedId}");
            }

            return existingFeed.Tags;
        }
    }
}
