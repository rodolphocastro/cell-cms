using System;
using System.Threading.Tasks;

using CellCms.Tests.Unit.Features.Management;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CellCms.Api.Features.Management
{
    /// <summary>
    /// Controller for management 
    /// </summary>
    [Route(ControllerBaseExtensions.DefaultRoute)]
    public class ManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ManagementController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("features")]
        public async Task<IActionResult> ListFeatures([FromQuery] ListFeatures query)
        {
            try
            {
                var result = await _mediator.Send(query, this.GetRequestCancellationToken());
                return Ok(result);
            }
            catch (TaskCanceledException)
            {
                return NoContent();
            }
        }
    }
}
