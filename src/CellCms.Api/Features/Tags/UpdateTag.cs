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
    /// Command para atualizar uma Tag.
    /// </summary>
    public class UpdateTag : IRequest
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }

    public class UpdateTagValidator : AbstractValidator<UpdateTag>
    {
        public UpdateTagValidator()
        {
            RuleFor(t => t.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(t => t.Nome)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    public class UpdateTagHandler : IRequestHandler<UpdateTag>
    {
        private readonly CellContext _context;
        private readonly IMapper _mapper;

        public UpdateTagHandler(CellContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Unit> Handle(UpdateTag request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = _mapper.Map<Tag>(request);

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

            _mapper.Map(model, existingModel);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
