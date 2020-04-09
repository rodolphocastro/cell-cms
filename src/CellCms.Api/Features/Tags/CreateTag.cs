using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using CellCms.Api.Models;

using FluentValidation;

using MediatR;

namespace CellCms.Api.Features.Tags
{
    /// <summary>
    /// Command para criar uma tag.
    /// </summary>
    public class CreateTag : IRequest<Tag>
    {
        public int FeedId { get; set; }

        public string Nome { get; set; }
    }

    public class CreateTagValidator : AbstractValidator<CreateTag>
    {
        public CreateTagValidator()
        {
            RuleFor(t => t.FeedId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(t => t.Nome)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    public class CreateTagHandler : IRequestHandler<CreateTag, Tag>
    {
        private readonly CellContext _context;
        private readonly IMapper _mapper;

        public CreateTagHandler(CellContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Tag> Handle(CreateTag request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = _mapper.Map<Tag>(request);

            return CreateTagInternalAsync(model, cancellationToken);
        }

        private async Task<Tag> CreateTagInternalAsync(Tag model, CancellationToken cancellationToken)
        {
            var existingFeed = await _context
                .Feeds
                .FindAsync(new object[] { model.FeedId }, cancellationToken);
            if (existingFeed is null)
            {
                throw new KeyNotFoundException($"Não foi encontrado um feed com id {model.Id}");
            }

            existingFeed.Tags.Add(model);
            await _context.SaveChangesAsync(cancellationToken);

            return model;
        }
    }
}
