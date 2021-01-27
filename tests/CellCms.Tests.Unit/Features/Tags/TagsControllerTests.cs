using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api.Features.Tags;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Tags
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public class TagsControllerTests
    {
        [Theory, CreateData]
        public async Task Create_ValidPayload_ReturnsCreatedObject(
            CreateTag payload,
            Tag payloadResult,
            [Frozen] IMediator mediator,
            [Greedy] TagsController subject)
        {
            // Arrange
            mediator
                .Send(payload)
                .Returns(payloadResult);

            // Act
            var result = await subject.Create(payload);

            // Assert
            result.Should()
                .BeOfType<CreatedResult>().Which
                .Value.Should().BeEquivalentTo(payloadResult);
        }

        [Theory, CreateData]
        public async Task Create_InvalidPayload_ReturnsBadRequest(
            CreateTag payload,
            [Greedy] TagsController subject)
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

        [Theory, CreateData]
        public async Task List_ValidPayload_ReturnsOkObject(
            ListTags payload,
            IEnumerable<Tag> payloadResults,
            [Frozen] IMediator mediator,
            [Greedy] TagsController subject)
        {
            // Arrange
            mediator
                .Send(payload)
                .Returns(payloadResults);

            // Act
            var result = await subject.List(payload);

            // Assert
            result.Should()
                .BeOfType<OkObjectResult>().Which
                .Value.Should().BeEquivalentTo(payloadResults);
        }

        [Theory, CreateData]
        public async Task List_NotFound_ReturnsNotFound(
            ListTags payload,
            [Frozen] IMediator mediator,
            [Greedy] TagsController subject)
        {
            // Arrange
            mediator
                .Send(payload)
                .Throws<KeyNotFoundException>();

            // Act
            var result = await subject.List(payload);

            // Assert
            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Theory, CreateData]
        public async Task List_InvalidPayload_ReturnsBadRequest(
            ListTags payload,
            [Greedy] TagsController subject)
        {
            // Arrange
            subject.ModelState
                .AddModelError(nameof(payload.FeedId), "an error");

            // Act
            var result = await subject.List(payload);

            // Assert
            result.Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [Theory, CreateData]
        public async Task Delete_ValidPayload_ReturnsNoContent(
            DeleteTag payload,
            [Greedy] TagsController subject
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
            DeleteTag payload,
            [Frozen] IMediator mediator,
            [Greedy] TagsController subject
            )
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
        public async Task Update_InvalidPayload_ReturnsBadRequest(
            UpdateTag payload,
            [Greedy] TagsController subject
            )
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
        public async Task Update_ValidPayload_ReturnsNoContent(
            UpdateTag payload,
            [Greedy] TagsController subject
            )
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
            UpdateTag payload,
            [Frozen] IMediator mediator,
            [Greedy] TagsController subject)
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
    }
}
