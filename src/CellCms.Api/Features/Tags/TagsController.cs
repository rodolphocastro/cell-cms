using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CellCms.Api.Features.Tags
{
    [Route(ControllerBaseExtensions.DefaultRoute)]
    public class TagsController : ControllerBase
    {
        private readonly ILogger<TagsController> _logger;
        private readonly IMediator _mediator;

        public TagsController(ILogger<TagsController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateTag command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mediator.Send(command, this.GetRequestCancellationToken());
                return Created(string.Empty, result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (TaskCanceledException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar uma nova tag: {@Command}", command);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListTags query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mediator.Send(query, this.GetRequestCancellationToken());
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (TaskCanceledException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tags: {@Command}", query);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteTag command)
        {
            try
            {
                await _mediator.Send(command, this.GetRequestCancellationToken());
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (TaskCanceledException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar uma tag: {@Command}", command);
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateTag command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _mediator.Send(command, this.GetRequestCancellationToken());
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (TaskCanceledException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar uma tag: {@Command}", command);
                throw;
            }
        }
    }
}
