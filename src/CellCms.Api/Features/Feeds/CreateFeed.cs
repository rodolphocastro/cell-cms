using System;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using CellCms.Api.Models;

using FluentValidation;

using MediatR;

namespace CellCms.Api.Features.Feeds
{
    /// <summary>
    /// Command para criar um novo Feed.
    /// </summary>
    public class CreateFeed : IRequest<Feed>
    {
        /// <summary>
        /// Nome do novo feed.
        /// </summary>
        public string Nome { get; set; }
    }

    /// <summary>
    /// Validator para o CreateFeed.
    /// </summary>
    public class CreateFeedValidator : AbstractValidator<CreateFeed>
    {
        public CreateFeedValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    /// <summary>
    /// Handler para a criação de feeds.
    /// </summary>
    public class CreateFeedHandler : IRequestHandler<CreateFeed, Feed>
    {
        private readonly CellContext _context;
        private readonly IMapper _mapper;

        public CreateFeedHandler(CellContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Feed> Handle(CreateFeed request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var feed = _mapper.Map<Feed>(request);

            // Separamos os métodos para que o compilador possa otimizar.
            // No método principal realizamos apenas operações síncronas
            // No método interno realizamos as operações assíncronas
            return CreateFeedInternalAsync(feed, cancellationToken);
        }

        private async Task<Feed> CreateFeedInternalAsync(Feed feed, CancellationToken cancellationToken)
        {
            _context.Feeds.Add(feed);
            await _context.SaveChangesAsync(cancellationToken);
            return feed;
        }
    }
}
