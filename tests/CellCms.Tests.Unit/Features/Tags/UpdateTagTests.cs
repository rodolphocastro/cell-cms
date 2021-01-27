using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class UpdateTagTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class UpdateTagValidatorTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_IdLessThanZero_Throws(UpdateTag command,
                UpdateTagValidator subject)
            {
                // Arrange
                command.Id = -1;

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldHaveValidationErrorFor(c => c.Id);
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_EmptyName_Throws(UpdateTag command,
                UpdateTagValidator subject)
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

        public class UpdateTagHandlerTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullRequest_ThrowsArgumentNullException(
                UpdateTagHandler subject)
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
                UpdateTag command,
                UpdateTagHandler subject)
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
                Tag tag,
                UpdateTag command,
                [Frozen] CellContext ctx,
                UpdateTagHandler subject)
            {
                // Arrange
                string nomeOriginal = tag.Nome;
                ctx.Tags.Add(tag);
                await ctx.SaveChangesAsync();
                command.Id = tag.Id;

                // Act
                var result = await subject.Handle(command, default);

                // Assert
                result.Should().NotBeNull();
                ctx.Tags
                    .SingleOrDefault().Nome
                    .Should()
                        .NotBeEquivalentTo(nomeOriginal);
            }
        }
    }
}
