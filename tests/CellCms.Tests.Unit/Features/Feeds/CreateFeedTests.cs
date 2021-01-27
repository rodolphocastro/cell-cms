
using System;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api;
using CellCms.Api.Features.Feeds;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using FluentValidation.TestHelper;

using Xunit;

namespace CellCms.Tests.Unit.Features.Feeds
{
    public class CreateFeedTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class CreateFeedValidatorTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_EmptyNome_Throws(CreateFeedValidator subject)
            {
                // Arrange

                // Act
                var result = subject.TestValidate(new CreateFeed { Nome = string.Empty });

                // Assert
                result.ShouldHaveValidationErrorFor(r => r.Nome);
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public void Validate_Nome_Validates(CreateFeed command, CreateFeedValidator subject)
            {
                // Arrange

                // Act
                var result = subject.TestValidate(command);

                // Assert
                result.ShouldNotHaveAnyValidationErrors();
            }
        }

        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class CreateFeedHandlerTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullCommand_ThrowsArgumentNullException(CreateFeedHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_ValidRequest_StoresIntoContext(
                [Frozen] CellContext context,
                CreateFeed command,
                CreateFeedHandler subject
                )
            {
                // Arrange

                // Act
                var result = await subject.Handle(command, default);

                // Assert
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(command);
                context.Feeds.Should().Contain(result);
            }
        }
    }
}
