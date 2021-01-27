using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api;
using CellCms.Api.Features.Feeds;
using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Feeds
{
    public class ListAllFeedsTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class ListAllFeedsHandlerTests
        {
            [Theory]
            [CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullRequest_ThrowsArgumentNull(ListAllFeedsHandler subject)
            {
                // Arrange            

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act
                    .Should()
                    .ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory]
            [CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_EmptyContext_ReturnsEmptyList(ListAllFeedsHandler subject)
            {
                // Arrange

                // Act
                var result = await subject.Handle(new ListAllFeeds(), default);

                // Assert
                result
                    .Should()
                    .NotBeNull().And
                    .BeEmpty();
            }

            [Theory]
            [CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_ExistingContext_ReturnsList([Frozen] CellContext context,
                IEnumerable<Feed> feeds,
                ListAllFeedsHandler subject)
            {
                // Arrange
                context.AddRange(feeds);
                await context.SaveChangesAsync();

                // Act
                var result = await subject.Handle(new ListAllFeeds(), default);

                // Assert
                result
                    .Should()
                    .NotBeNull().And
                    .HaveSameCount(context.Feeds);
            }
        }

    }
}
