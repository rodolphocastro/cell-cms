using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using CellCms.Api.Models;

using FluentValidation;

using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CellCms.Api.Features.Contents
{
    /// <summary>
    /// Command para atualizar um Content.
    /// </summary>
    public class UpdateContent : IRequest<Content>
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Corpo { get; set; }

        public IEnumerable<int> TagsId = new List<int>();
    }

    public class UpdateContentValidator : AbstractValidator<UpdateContent>
    {
        public UpdateContentValidator()
        {
            RuleFor(c => c.Id)
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

    public class UpdateContentHandler : IRequestHandler<UpdateContent, Content>
    {
        private readonly CellContext _context;
        private readonly IMapper _mapper;

        public UpdateContentHandler(CellContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<Content> Handle(UpdateContent request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var model = _mapper.Map<Content>(request);

            return UpdateFeedInternalAsync(model, cancellationToken);
        }

        private async Task<Content> UpdateFeedInternalAsync(Content updatedContent, CancellationToken cancellationToken)
        {
            var existingContent = await _context
                .Contents
                .Include(c => c.ContentTags)
                .SingleOrDefaultAsync(c => c.Id == updatedContent.Id, cancellationToken);

            if (existingContent is null)
            {
                throw new KeyNotFoundException($"Não foi encontrado um Content com id {updatedContent.Id}");
            }

            _mapper.Map(updatedContent, existingContent);

            await _context.SaveChangesAsync(cancellationToken);
            return existingContent;
        }
    }
}
