using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CellCms.Api.Features.Tags
{
    /// <summary>
    /// Command para deletar uma tag.
    /// </summary>
    public class DeleteTag : IRequest
    {
        [FromRoute(Name = "id")]
        public int Id { get; set; }
    }

    public class DeleteTagHandler : IRequestHandler<DeleteTag>
    {
        private readonly CellContext _context;

        public DeleteTagHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Unit> Handle(DeleteTag request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return DeleteTagInternalAsync(request.Id, cancellationToken);
        }

        private async Task<Unit> DeleteTagInternalAsync(int id, CancellationToken cancellationToken)
        {
            var existingTag = await _context
                .Tags
                .FindAsync(new object[] { id }, cancellationToken);

            if (existingTag is null)
            {
                throw new KeyNotFoundException($"Uma tag com id {id} não foi encontrada");
            }

            _context.Tags.Remove(existingTag);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
