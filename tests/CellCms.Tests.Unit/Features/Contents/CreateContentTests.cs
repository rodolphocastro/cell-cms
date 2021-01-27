using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api;
using CellCms.Api.Features.Contents;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using FluentValidation.TestHelper;

using Xunit;

namespace CellCms.Tests.Unit.Features.Contents
{
    public class CreateContentTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class CreateContentValidatorTests
        {
            [Theory, CreateData]
            public void Validate_FeedIdLessThanZero_Throws(CreateContent command,
                CreateContentValidator subject)
            {
                // Arrange
                command.FeedId = -1;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.FeedId);
            }

            [Theory, CreateData]
            public void Validate_TituloEmpty_Throws(CreateContent command,
                CreateContentValidator subject)
            {
                // Arrange
                command.Titulo = string.Empty;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Titulo);
            }

            [Theory, CreateData]
            public void Validate_CorpoEmpty_Throws(CreateContent command,
                CreateContentValidator subject)
            {
                // Arrange
                command.Corpo = string.Empty;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Corpo);
            }

            [Theory, CreateData]
            public void Validate_TagsIdLessThanZero_Throws(CreateContent command,
                CreateContentValidator subject)
            {
                // Arrange
                command.TagsId = new List<int> { -1 };

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.TagsId);
            }
        }

        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]

        public class CreateContentHandlerTests
        {
            [Theory, CreateData]
            public async Task Handle_State_ThrowsArgumentNullException(
                CreateContentHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            public async Task Handle_NotFound_ThrowsKeyNotFoundException(
                CreateContent command,
                CreateContentHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(command, default);

                // Assert
                await act.Should().ThrowExactlyAsync<KeyNotFoundException>();
            }

            [Theory, CreateData]
            public async Task Handle_Valid_Returns(
                CreateContent command,
                Feed f,
                [Frozen] CellContext context,
                CreateContentHandler subject)
            {
                // Arrange
                context.Feeds.Add(f);
                await context.SaveChangesAsync();
                command.FeedId = f.Id;

                // Act
                var result = await subject.Handle(command, default);

                // Assert
                result.Should()
                    .NotBeNull().And
                    .BeEquivalentTo(command, e => e.ExcludingMissingMembers());
                context.Contents.Should().Contain(result);
            }
        }
    }
}
