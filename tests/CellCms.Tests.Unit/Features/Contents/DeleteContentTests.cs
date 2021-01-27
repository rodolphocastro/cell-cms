using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api;
using CellCms.Api.Features.Contents;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Contents
{
    public class DeleteContentTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class DeleteContentHandlerTests
        {
            [Theory, CreateData]
            public async Task Handle_NullRequest_ThrowsArgumentNulLException(
                DeleteContentHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            public async Task Handle_NotFound_ThrowsKeyNotFoundException(
                DeleteContent command,
                DeleteContentHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(command, default);

                // Assert
                await act.Should().ThrowExactlyAsync<KeyNotFoundException>();
            }

            [Theory, CreateData]
            public async Task Handle_Valid_Returns(
                DeleteContent command,
                Content target,
                [Frozen] CellContext ctx,
                DeleteContentHandler subject)
            {
                // Arrange
                ctx.Contents.Add(target);
                await ctx.SaveChangesAsync();
                command.Id = target.Id;

                // Act
                var result = await subject.Handle(command, default);

                // Assert
                result.Should().NotBeNull();
                ctx.Contents.Should().NotContain(target);
            }
        }
    }
}
