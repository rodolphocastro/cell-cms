using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api.Features.Feeds;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Feeds
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public class FeedControllerTests
    {
        [Theory, CreateData]
        public async Task GetAll_ValidState_ReturnsOkObject(
            [Frozen] IMediator mediator,
            IEnumerable<ListFeed> feeds,
            [Greedy] FeedController subject
            )
        {
            // Arrange
            mediator
                .Send(Arg.Any<ListAllFeeds>())
                .Returns(feeds);

            // Act
            var result = await subject.GetAll();

            // Assert
            result.Should()
                .BeOfType<OkObjectResult>().Which
                .Value.Should().BeAssignableTo<IEnumerable<ListFeed>>().And
                .BeEquivalentTo(feeds);
        }

        [Theory, CreateData]
        public async Task Delete_ValidPayload_ReturnsNoContent(
            DeleteFeed payload,
            [Greedy] FeedController subject
            )
        {
            // Arrange

            // Act
            var result = await subject.Delete(payload);

            // Assert
            result.Should()
                .BeOfType<NoContentResult>();
        }

        [Theory, CreateData]
        public async Task Delete_NotFound_ReturnsNotFound(
            [Frozen] IMediator mediator,
            DeleteFeed payload,
            [Greedy] FeedController subject)
        {
            // Arrange
            mediator
                .Send(payload)
                .Throws<KeyNotFoundException>();

            // Act
            var result = await subject.Delete(payload);

            // Assert
            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Theory, CreateData]
        public async Task Update_ValidPayload_ReturnsNoContent(
            UpdateFeed payload,
            [Greedy] FeedController subject)
        {
            // Arrange

            // Act
            var result = await subject.Update(payload);

            // Assert
            result.Should()
                .BeOfType<NoContentResult>();
        }

        [Theory, CreateData]
        public async Task Update_NotFound_ReturnsNotFound(
            [Frozen] IMediator mediator,
            UpdateFeed payload,
            [Greedy] FeedController subject)
        {
            // Arrange
            mediator
                .Send(payload)
                .Throws<KeyNotFoundException>();

            // Act
            var result = await subject.Update(payload);

            // Assert
            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Theory, CreateData]
        public async Task Update_BadState_ReturnsBadRequest(
            UpdateFeed payload,
            [Greedy] FeedController subject)
        {
            // Arrange
            subject.ModelState
                .AddModelError(nameof(payload.Nome), "an error");

            // Act
            var result = await subject.Update(payload);

            // Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [Theory, CreateData]
        public async Task Create_ValidState_ReturnsCreated(
            [Frozen] IMediator mediator,
            CreateFeed payload,
            Feed created,
            [Greedy] FeedController subject
            )
        {
            // Arrange
            mediator
                .Send(payload)
                .Returns(created);

            // Act
            var result = await subject.Create(payload);

            // Assert
            result.Should()
                .BeOfType<CreatedResult>();
        }

        [Theory, CreateData]
        public async Task Create_BadModelState_ReturnsBadRequest(
            CreateFeed payload,
            [Greedy] FeedController subject
            )
        {
            // Arrange
            subject.ModelState
                .AddModelError(nameof(payload.Nome), "an error");

            // Act
            var result = await subject.Create(payload);

            // Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
        }
    }
}
