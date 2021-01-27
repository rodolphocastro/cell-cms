
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api;
using CellCms.Api.Features.Tags;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using FluentValidation.TestHelper;

using Xunit;

namespace CellCms.Tests.Unit.Features.Tags
{
    public class CreateTagTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class CreateTagValidatorTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_LowerThanZeroId_Throws(CreateTag command,
                CreateTagValidator subject)
            {
                // Arrange
                command.FeedId = -1;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.FeedId);
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_EmptyName_Throws(CreateTag command,
                CreateTagValidator subject)
            {
                // Arrange
                command.Nome = string.Empty;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Nome);
            }
        }

        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class CreateTagHandlerTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullRequest_ThrowsArgumentNullException(
                CreateTagHandler subject
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
            public async Task Handle_NotFound_ThrowsKeyNotFoundException(
                CreateTag command,
                CreateTagHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(command, default);

                // Assert
                await act.Should().ThrowExactlyAsync<KeyNotFoundException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_Valid_Returns(
                CreateTag command,
                Feed f,
                [Frozen] CellContext context,
                CreateTagHandler subject)
            {
                // Arrange
                f.Id = command.FeedId;
                context.Feeds.Add(f);
                await context.SaveChangesAsync();

                // Act
                var result = subject.Handle(command, default);

                // Assert
                result.Should().NotBeNull();
                context.Tags.Should().ContainEquivalentOf(command);
            }
        }
    }
}
