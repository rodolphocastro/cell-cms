using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CellCms.Api.Models;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CellCms.Api.Features.Contents
{
    public class ListContent : IRequest<IEnumerable<Content>>
    {
        [FromQuery(Name = "feedId")]
        public int? FeedId { get; set; } = null;

        [FromQuery(Name = "tagId")]
        public int? TagId { get; set; } = null;
    }

    public class ListContentHandler : IRequestHandler<ListContent, IEnumerable<Content>>
    {
        private readonly CellContext _context;

        public ListContentHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<IEnumerable<Content>> Handle(ListContent request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return ListContentInternalAsync(request.FeedId, request.TagId, cancellationToken);
        }

        private Task<IEnumerable<Content>> ListContentInternalAsync(int? feedId, int? tagId, CancellationToken cancellationToken)
        {
            if (feedId is int f)
            {
                return ListContentByFeedInternalAsync(f, cancellationToken);
            }
            else if (tagId is int t)
            {
                return ListContentByTagInternalAsync(t, cancellationToken);
            }

            return ListContentInternalAsync(cancellationToken);
        }

        private async Task<IEnumerable<Content>> ListContentInternalAsync(CancellationToken cancellationToken)
        {
            var result = await _context
                .Contents
                .AsNoTracking()
                .Include(c => c.Feed)
                .Include(c => c.ContentTags)
                    .ThenInclude(ct => ct.Tag)
                .ToListAsync(cancellationToken);

            return result;
        }

        private async Task<IEnumerable<Content>> ListContentByTagInternalAsync(int tagId, CancellationToken cancellationToken)
        {
            var existingTag = await _context
                .Tags
                .AsNoTracking()
                .Include(t => t.ContentTags)
                    .ThenInclude(ct => ct.Content)
                .SingleOrDefaultAsync(t => t.Id == tagId, cancellationToken);

            if (existingTag is null)
            {
                throw new KeyNotFoundException($"A tag com id {tagId} não foi encontrada");
            }

            return existingTag
                .ContentTags
                .Select(ct => ct.Content);
        }
        private async Task<IEnumerable<Content>> ListContentByFeedInternalAsync(int feedId, CancellationToken cancellationToken)
        {
            var existingFeed = await _context
                .Feeds
                .AsNoTracking()
                .Include(f => f.Contents)
                    .ThenInclude(c => c.ContentTags)
                        .ThenInclude(ct => ct.Tag)
                .SingleOrDefaultAsync(f => f.Id == feedId, cancellationToken);

            if (existingFeed is null)
            {
                throw new KeyNotFoundException($"O feed com id {feedId} não foi encontrado");
            }

            return existingFeed.Contents;
        }
    }
}