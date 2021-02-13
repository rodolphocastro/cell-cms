using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Management
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public class ManagementControllerTests
    {
        [Theory, CreateData]
        public async Task ListFeatures_ValidQuery_ReturnsOkObject(
            ListFeatures query,
            IEnumerable<ListFeaturesResponse> queryResult,
            [Frozen] IMediator mediator,
            [Greedy] ManagementController subject)
        {
            // Arrange
            mediator
                .Send(query, Arg.Any<CancellationToken>())
                .Returns(queryResult);

            // Act
            IActionResult result = await subject.ListFeatures(query);

            // Assert
            result.Should()
                .NotBeNull().And
                .BeOfType<OkObjectResult>().Which
                .Value.Should().BeEquivalentTo(queryResult);
        }

        [Theory, CreateData]
        public async Task ListFeatures_Cancelled_ReturnsNoContent(
            ListFeatures query,
            TaskCanceledException ex,
            [Frozen] IMediator mediator,
            [Greedy] ManagementController subject)
        {
            // Arrange
            mediator
                .Send(query, Arg.Any<CancellationToken>())
                .Throws(ex);

            // Act
            IActionResult result = await subject.ListFeatures(query);

            // Assert
            result.Should()
                .NotBeNull().And
                .BeOfType<NoContentResult>();
        }
    }

    /// <summary>
    /// Controller for management 
    /// </summary>
    public class ManagementController
    {
        public Task<IActionResult> ListFeatures(ListFeatures query) => throw new NotImplementedException();
    }
}
