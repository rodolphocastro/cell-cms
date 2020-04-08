using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CellCms.Api.Models;

using MediatR;

namespace CellCms.Api.Features.Contents
{
    /// <summary>
    /// Command para criar um content.
    /// </summary>
    public class CreateContent : IRequest<Content>
    {
        public int FeedId { get; set; }

        public string Titulo { get; set; }

        public string Corpo { get; set; }

        public IEnumerable<CreateContentTag> ContentTags { get; set; } = new HashSet<CreateContentTag>();

        public class CreateContentTag
        {
            public int TagId { get; set; }
        }
    }

    public class CreateContentHandler : IRequestHandler<CreateContent, Content>
    {
        private readonly CellContext _context;

        public CreateContentHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Content> Handle(CreateContent request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = new Content
            {
                FeedId = request.FeedId,
                Titulo = request.Titulo,
                Corpo = request.Corpo,
                ContentTags = request.ContentTags.Select(c => new ContentTag { TagId = c.TagId }).ToHashSet()
            };            

            return CreateContentInternalAsync(model, cancellationToken);
        }

        private async Task<Content> CreateContentInternalAsync(Content model, CancellationToken cancellationToken)
        {
            var existingFeed = await _context
                .Feeds
                .FindAsync(new object[] { model.FeedId }, cancellationToken);

            if (existingFeed is null)
            {
                throw new KeyNotFoundException($"Feed com id {model.FeedId} não foi encontrado");
            }

            existingFeed.Contents.Add(model);
            await _context.SaveChangesAsync(cancellationToken);

            return model;
        }
    }
}