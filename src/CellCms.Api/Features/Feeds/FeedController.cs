using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CellCms.Api.Features.Feeds
{
    [Route("api/[controller]")]
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
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateFeed command)
        {
            // TODO: Futuramente vamos implementar estas validações
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mediator.Send(command);
                return Created(string.Empty, result);
            }
            catch (ArgumentException ex)
            {
                // Futuramente será tratado pelo ModelState.
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Caso seja algum erro que não esperavamos, vamos fazer um log decente deste erro.
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
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar listar todos os feeds");
                throw;
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateFeed command)
        {
            // TODO: Futuramente vamos implementar estas validações
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar atualizar um Feed: {@Command}", command);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] DeleteFeed command)
        {
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar deletar um Feed: {@Command}", command);
                throw;
            }
        }
    }
}
