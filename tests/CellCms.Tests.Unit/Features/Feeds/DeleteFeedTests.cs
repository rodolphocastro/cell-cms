
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
    public class DeleteFeedTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class DeleteFeedHandlerTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullRequest_ThrowsArgumentNullException(DeleteFeedHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_FeedNotFound_ThrowsKeyNotFoundException(DeleteFeed request, DeleteFeedHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(request, default);

                // Assert
                await act.Should().ThrowExactlyAsync<KeyNotFoundException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_FeedExists_ReturnsUnit(
                [Frozen] CellContext context,
                Feed feed,
                DeleteFeedHandler subject)
            {
                // Arrange
                context.Feeds.Add(feed);
                await context.SaveChangesAsync();

                // Act
                var result = await subject.Handle(new DeleteFeed { Id = feed.Id }, default);

                // Assert
                result.Should().NotBeNull();
                context.Feeds.Should().BeEmpty();
            }
        }
    }
}
