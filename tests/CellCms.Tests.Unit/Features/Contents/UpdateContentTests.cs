using System;
using System.Collections.Generic;
using System.Linq;
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
    public class UpdateContentTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class UpdateContentValidatorTests
        {
            [Theory, CreateData]
            public void Validate_IdLessThanZero_Throws(UpdateContent command,
                UpdateContentValidator subject)
            {
                // Arrange
                command.Id *= -1;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Id);
            }

            [Theory, CreateData]
            public void Validate_TituloEmpty_Throws(UpdateContent command,
                UpdateContentValidator subject)
            {
                // Arrange
                command.Titulo = string.Empty;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Titulo);
            }

            [Theory, CreateData]
            public void Validate_CorpoEmpty_Throws(UpdateContent command,
                UpdateContentValidator subject)
            {
                // Arrange
                command.Corpo = string.Empty;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Corpo);
            }

            [Theory, CreateData]
            public void Validate_TagsLessThanZero_Throws(UpdateContent command,
                UpdateContentValidator subject)
            {
                // Arrange
                command.TagsId = command.TagsId.Select(t => t * -1);

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.TagsId);
            }
        }

        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class UpdateContentHandlerTests
        {
            [Theory, CreateData]
            public async Task Handle_NullRequest_ThrowsArgumentNullException(
                UpdateContentHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            public async Task Handle_NotFound_ThrowsKeyNotFoundException(
                UpdateContent command,
                UpdateContentHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(command, default);

                // Assert
                await act.Should().ThrowExactlyAsync<KeyNotFoundException>();
            }

            [Theory, CreateData]
            public async Task Handle_Found_Returns(
                UpdateContent command,
                Content c,
                [Frozen] CellContext ctx,
                UpdateContentHandler subject)
            {
                // Arrange
                command.Id = c.Id;
                ctx.Add(c);
                await ctx.SaveChangesAsync();

                // Act
                var result = await subject.Handle(command, default);

                // Assert
                result.Should().NotBeNull();
            }
        }
    }
}
