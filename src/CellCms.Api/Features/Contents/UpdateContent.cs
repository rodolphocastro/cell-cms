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
    /// Command para atualizar um Content.
    /// </summary>
    public class UpdateContent : IRequest<Content>
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Corpo { get; set; }

        public IEnumerable<UpdateContentTag> ContentTags { get; set; } = new HashSet<UpdateContentTag>();

        public class UpdateContentTag
        {
            public int TagId { get; set; }
        }
    }

    public class UpdateContentHandler : IRequestHandler<UpdateContent, Content>
    {
        private readonly CellContext _context;

        public UpdateContentHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Content> Handle(UpdateContent request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return UpdateFeedInternalAsync(request.Id, request.Titulo, request.Corpo, request.ContentTags.Select(c => new ContentTag { TagId = c.TagId }), cancellationToken);
        }

        private async Task<Content> UpdateFeedInternalAsync(int id, string titulo, string corpo, IEnumerable<ContentTag> contentTags, CancellationToken cancellationToken)
        {
            var existingContent = await _context
                .Contents
                .FindAsync(new object[] { id }, cancellationToken);

            if (existingContent is null)
            {
                throw new KeyNotFoundException($"Não foi encontrado um Content com id {id}");
            }

            existingContent.Titulo = titulo;
            existingContent.Corpo = corpo;
            existingContent.ContentTags = contentTags.ToHashSet();
            
            await _context.SaveChangesAsync(cancellationToken);
            return existingContent;
        }
    }
}
