using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CellCms.Api.Features.Feeds
{
    /// <summary>
    /// Command para deletar um feed.
    /// </summary>
    public class DeleteFeed : IRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }

    /// <summary>
    /// Handler para commands de delete.
    /// </summary>
    public class DeleteFeedHandler : IRequestHandler<DeleteFeed>
    {
        private readonly CellContext _context;

        public DeleteFeedHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Unit> Handle(DeleteFeed request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return DeleteFeedInternalAsync(request.Id, cancellationToken);
        }

        private async Task<Unit> DeleteFeedInternalAsync(int id, CancellationToken cancellationToken)
        {
            var existingFeed = await _context
                .Feeds
                .FindAsync(new object[] { id }, cancellationToken: cancellationToken);
            if (existingFeed is null)
            {
                throw new KeyNotFoundException($"Não foi encontrado um feed com id {id}");
            }

            _context.Feeds.Remove(existingFeed);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;

        }
    }
}
