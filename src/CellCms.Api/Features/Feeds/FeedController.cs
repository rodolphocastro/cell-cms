using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CellCms.Api.Features.Feeds
{

    [Route(ControllerBaseExtensions.DefaultRoute)]
    public class FeedController : ControllerBase
    {
        private readonly ILogger<FeedController> _logger;
        private readonly IMediator _mediator;

        public FeedController(ILogger<FeedController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeed command)
        {
            // TODO: Futuramente vamos implementar estas validações
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mediator.Send(command, this.GetRequestCancellationToken());
                return Created(string.Empty, result);
            }
            catch (TaskCanceledException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                // Caso seja algum erro que não esperávamos, vamos fazer um log decente deste erro.
                _logger.LogError(ex, "Erro ao tentar criar um novo Feed: {@Command}", command);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new ListAllFeeds();
            try
            {
                var result = await _mediator.Send(query, this.GetRequestCancellationToken());
                return Ok(result);
            }
            catch (TaskCanceledException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar listar todos os feeds");
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFeed command)
        {
            // TODO: Futuramente vamos implementar estas validações
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
                _logger.LogError(ex, "Erro ao tentar atualizar um Feed: {@Command}", command);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteFeed command)
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
                _logger.LogError(ex, "Erro ao tentar deletar um Feed: {@Command}", command);
                throw;
            }
        }
    }
}
