using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

using CellCms.Api.Models;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace CellCms.Api.Features.Feeds
{
    /// <summary>
    /// Query para listar
    /// </summary>
    public class ListAllFeeds : IRequest<IEnumerable<ListFeed>> { }

    /// <summary>
    /// Feed para exibição em lista.
    /// </summary>
    public class ListFeed
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        /// <summary>
        /// Lista dos nomes das tags.
        /// </summary>
        public IEnumerable<string> TagsNomes { get; set; }

        /// <summary>
        /// Lista dos títulos dos conteúdos.
        /// </summary>
        public IEnumerable<string> ContentsTitulos { get; set; }

    }

    /// <summary>
    /// Handler para listar feeds.
    /// </summary>
    public class ListAllFeedsHandler : IRequestHandler<ListAllFeeds, IEnumerable<ListFeed>>
    {
        private readonly CellContext _context;
        private readonly IMapper _mapper;

        public ListAllFeedsHandler(CellContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<IEnumerable<ListFeed>> Handle(ListAllFeeds request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return ListAllFeedsInternalAsync(cancellationToken);
        }

        private async Task<IEnumerable<ListFeed>> ListAllFeedsInternalAsync(CancellationToken cancellationToken)
        {
            var result = await _context
                .Feeds
                .AllFeeds()
                .AsNoTracking() // Indicando para o EF Core que vamos fazer apenas operações de leitura
                .Include(f => f.Contents)
                .Include(f => f.Tags)
                .ToListAsync(cancellationToken);

            // Futuramente vamos fazer o mapeamento automaticamente!
            return _mapper.Map<IEnumerable<ListFeed>>(result);
        }
    }
}
