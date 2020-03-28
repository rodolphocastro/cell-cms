using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CellCms.Api.Features.Contents
{
    [Route("api/[controller]")]
    public class ContentsController : ControllerBase
    {
        private readonly ILogger<ContentsController> _logger;
        private readonly IMediator _mediator;

        public ContentsController(ILogger<ContentsController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateContent command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mediator.Send(command);
                return Created(string.Empty, result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar content: {@Command}", command);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListContent query)
        {
            try
            {
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar contents: {@Command}", query);
                throw;
            }
        }

        // TODO: Adicionar endpoint de Deletar Content
        // TODO: Adicionar endpoint de Atualizar Content
        // TODO: Adicionar endpoint de Alterar Tags do Content
    }
}
