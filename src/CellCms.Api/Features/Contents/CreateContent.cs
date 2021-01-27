using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using CellCms.Api.Models;

using FluentValidation;

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

        public IEnumerable<int> TagsId = new List<int>();
    }

    public class CreateContentValidator : AbstractValidator<CreateContent>
    {
        public CreateContentValidator()
        {
            RuleFor(c => c.FeedId)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(c => c.Titulo)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(c => c.Corpo)
                .NotEmpty()
                .MaximumLength(3000);

            RuleForEach(c => c.TagsId)
                .GreaterThan(0);

        }
    }

    public class CreateContentHandler : IRequestHandler<CreateContent, Content>
    {
        private readonly CellContext _context;
        private readonly IMapper _mapper;

        public CreateContentHandler(CellContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Content> Handle(CreateContent request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = _mapper.Map<Content>(request);

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