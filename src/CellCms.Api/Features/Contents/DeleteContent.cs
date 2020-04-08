using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CellCms.Api.Features.Contents
{
    /// <summary>
    /// Command para deletar um content.
    /// </summary>
    public class DeleteContent : IRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }

    public class DeleteContentHandler : IRequestHandler<DeleteContent>
    {
        private readonly CellContext _context;

        public DeleteContentHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Unit> Handle(DeleteContent request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return DeleteContentInternalAsync(request.Id, cancellationToken);
        }

        private async Task<Unit> DeleteContentInternalAsync(int id, CancellationToken cancellationToken)
        {
            var existingModel = await _context
                .Contents
                .FindAsync(new object[] { id }, cancellationToken);
            if (existingModel is null)
            {
                throw new KeyNotFoundException();
            }

            _context.Contents.Remove(existingModel);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
