using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using MediatR;

namespace CellCms.Api.Features.Feeds
{
    /// <summary>
    /// Command para editar um feed.
    /// </summary>
    public class UpdateFeed : IRequest
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }

    /// <summary>
    /// Validator para o UpdateFeed.
    /// </summary>
    public class UpdateFeedValidator : AbstractValidator<UpdateFeed>
    {
        public UpdateFeedValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(c => c.Nome)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    /// <summary>
    /// Handler para updates.
    /// </summary>
    public class UpdateFeedHandler : IRequestHandler<UpdateFeed>
    {
        private readonly CellContext _context;

        public UpdateFeedHandler(CellContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Task<Unit> Handle(UpdateFeed request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return UpdateFeedInternalAsync(request.Id, request.Nome, cancellationToken);
        }

        private async Task<Unit> UpdateFeedInternalAsync(int id, string nome, CancellationToken cancellationToken)
        {
            var existingFeed = await _context
                .Feeds
                .FindAsync(new object[] { id }, cancellationToken);

            if (existingFeed is null)
            {
                throw new KeyNotFoundException($"Não foi encontrado um feed com id {id}");
            }

            existingFeed.Nome = nome;

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
