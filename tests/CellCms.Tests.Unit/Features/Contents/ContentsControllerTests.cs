using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api.Features.Contents;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Contents
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public class ContentsControllerTests
    {
        private const string _errorMessage = "an error";

        [Theory, CreateData]
        public async Task Create_InvalidState_ReturnsBadRequest(
            CreateContent command,
            [Greedy] ContentsController subject)
        {
            // Arrange
            subject.ModelState
                .AddModelError(nameof(command.Corpo), _errorMessage);

            // Act
            var result = await subject.Create(command);

            // Assert
            result.Should()
                .NotBeNull().And
                .BeOfType<BadRequestObjectResult>();
        }

        [Theory, CreateData]
        public async Task Create_Valid_ReturnsCreated(
            CreateContent command,
            Content commandResult,
            [Frozen] IMediator mediator,
            [Greedy] ContentsController subject
            )
        {
            // Arrange
            mediator
                .Send(command)
                .Returns(commandResult);

            // Act
            var result = await subject.Create(command);

            // Assert
            result.Should()
                .BeOfType<CreatedResult>().Which
                .Value.Should().BeEquivalentTo(commandResult);
        }

        [Theory, CreateData]
        public async Task Create_NotFound_ReturnsNotFound(
            CreateContent command,
            KeyNotFoundException ex,
            [Frozen] IMediator mediator,
            [Greedy] ContentsController subject)
        {
            // Arrange
            mediator
                .Send(command)
                .Throws(ex);

            // Act
            var result = await subject.Create(command);

            // Assert
            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Theory, CreateData]
        public async Task List_Valid_ReturnsOkObject(
            ListContent query,
            IEnumerable<Content> queryResult,
            [Frozen] IMediator mediator,
            [Greedy] ContentsController subject)
        {
            // Arrange
            mediator
                .Send(query)
                .Returns(queryResult);

            // Act
            var result = await subject.List(query);

            // Assert
            result.Should()
                .BeOfType<OkObjectResult>().Which
                .Value.Should().BeEquivalentTo(queryResult);
        }

        [Theory, CreateData]
        public async Task List_NotFound_ReturnsNotFound(
            ListContent query,
            KeyNotFoundException ex,
            [Frozen] IMediator mediator,
            [Greedy] ContentsController subject)
        {
            // Arrange
            mediator
                .Send(query)
                .Throws(ex);

            // Act
            var result = await subject.List(query);

            // Assert
            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Theory, CreateData]
        public async Task Delete_Valid_ReturnsNoContent(
            DeleteContent command,
            [Greedy] ContentsController subject)
        {
            // Arrange

            // Act
            var result = await subject.Delete(command);

            // Assert
            result.Should()
                .BeOfType<NoContentResult>();
        }

        [Theory, CreateData]
        public async Task Delete_NotFound_ReturnsNotFound(
            DeleteContent command,
            KeyNotFoundException ex,
            [Frozen] IMediator mediator,
            [Greedy] ContentsController subject)
        {
            // Arrange
            mediator
                .Send(command)
                .Throws(ex);

            // Act
            var result = await subject.Delete(command);

            // Assert
            result.Should()
                .BeOfType<NotFoundResult>();
        }

        [Theory, CreateData]
        public async Task Update_InvalidState_ReturnsBadRequest(
            UpdateContent command,
            [Greedy] ContentsController subject
            )
        {
            // Arrange
            subject.ModelState
                .AddModelError(nameof(command.Titulo), _errorMessage);

            // Act
            var result = await subject.Update(command);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Theory, CreateData]
        public async Task Update_Valid_ReturnsNoContent(
            UpdateContent command,
            [Greedy] ContentsController subject)
        {
            // Arrange

            // Act
            var result = await subject.Update(command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Theory, CreateData]
        public async Task Update_NotFound_ReturnsNotFound(
            UpdateContent command,
            KeyNotFoundException ex,
            [Frozen] IMediator mediator,
            [Greedy] ContentsController subject)
        {
            // Arrange
            mediator
                .Send(command)
                .Throws(ex);

            // Act
            var result = await subject.Update(command);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
