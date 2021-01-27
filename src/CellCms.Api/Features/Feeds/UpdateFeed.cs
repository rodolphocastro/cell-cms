using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CellCms.Api.Models;
using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

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
        private readonly IMapper _mapper;

        public UpdateFeedHandler(CellContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public Task<Unit> Handle(UpdateFeed request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var updatedModel = _mapper.Map<Feed>(request);

            return UpdateFeedInternalAsync(updatedModel, cancellationToken);
        }

        private async Task<Unit> UpdateFeedInternalAsync(Feed updated, CancellationToken cancellationToken)
        {
            var existingFeed = await _context
                .Feeds
                .WithId(updated.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingFeed is null)
            {
                throw new KeyNotFoundException($"Não foi encontrado um feed com id {updated.Id}");
            }

            _mapper.Map(updated, existingFeed);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
