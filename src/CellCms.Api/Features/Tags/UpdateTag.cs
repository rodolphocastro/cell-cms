using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CellCms.Api.Models;

using MediatR;

namespace CellCms.Api.Features.Tags
{
    /// <summary>
    /// Command para atualizar uma Tag.
    /// </summary>
    public class UpdateTag : IRequest
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }

    public class UpdateTagHandler : IRequestHandler<UpdateTag>
    {
        private readonly CellContext _context;

        public UpdateTagHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Task<Unit> Handle(UpdateTag request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = new Tag
            {
                Id = request.Id,
                Nome = request.Nome
            };

            return UpdateTagInternalAsync(model, cancellationToken);
        }

        private async Task<Unit> UpdateTagInternalAsync(Tag model, CancellationToken cancellationToken)
        {
            var existingModel = await _context
                .Tags
                .FindAsync(new object[] { model.Id }, cancellationToken);

            if (existingModel is null)
            {
                throw new KeyNotFoundException($"Nenhum tag encontrada para o id {model.Id}");
            }

            existingModel.Nome = model.Nome;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
