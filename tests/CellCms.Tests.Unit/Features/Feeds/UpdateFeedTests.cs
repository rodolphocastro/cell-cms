using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api;
using CellCms.Api.Features.Feeds;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using FluentValidation.TestHelper;

using Xunit;

namespace CellCms.Tests.Unit.Features.Feeds
{
    public class UpdateFeedTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class UpdateFeedValidatorTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_EmptyNome_Throws(UpdateFeedValidator subject)
            {
                // Arrange

                // Act
                var result = subject.TestValidate(new UpdateFeed { Id = 1, Nome = string.Empty });

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Nome);
            }

            [Theory]
            [InlineData(-1, "A feed has no name")]
            [InlineData(-0, "The book is on the table")]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_InvalidIds_Throws(int id, string name)
            {
                // Arrange
                var subject = new UpdateFeedValidator();

                // Act
                var result = subject.TestValidate(new UpdateFeed { Id = id, Nome = name });

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Id);
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_ValidCommand_Returns(UpdateFeed command, UpdateFeedValidator subject)
            {
                // Arrange

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldNotHaveAnyValidationErrors();
            }
        }

        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class UpdateFeedHandlerTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullCommand_ThrowsArgumentNullException(
                UpdateFeedHandler subject
                )
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_FeedNotFound_ThrowsKeyNotFoundException(
                UpdateFeed command,
                UpdateFeedHandler subject
                )
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(command, default);

                // Assert
                await act.Should().ThrowExactlyAsync<KeyNotFoundException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_FeedFound_Returns(
                Feed feed,
                UpdateFeed command,
                [Frozen] CellContext context,
                UpdateFeedHandler subject
                )
            {
                // Arrange
                context.Feeds.Add(feed);
                await context.SaveChangesAsync();
                command.Id = feed.Id;

                // Act
                var result = await subject.Handle(command, default);

                // Assert
                result.Should().NotBeNull();
            }
        }
    }
}
