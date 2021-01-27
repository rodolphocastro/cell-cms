using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api;
using CellCms.Api.Features.Tags;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Tags
{
    public class DeleteTagTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class DeleteTagHandlerTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullRequest_ThrowsArgumentNullException(
                DeleteTagHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NotFound_ThrowsKeyNotFoundException(
                DeleteTag command,
                DeleteTagHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(command, default);

                // Assert
                await act.Should().ThrowExactlyAsync<KeyNotFoundException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_Found_Returns(
                DeleteTag command,
                Tag tag,
                [Frozen] CellContext context,
                DeleteTagHandler subject)
            {
                // Arrange
                tag.Id = command.Id;
                context.Tags.Add(tag);
                await context.SaveChangesAsync();

                // Act
                var result = await subject.Handle(command, default);

                // Assert
                result.Should().NotBeNull();
                context
                    .Tags
                    .Should().NotContain(tag);
            }
        }
    }
}
