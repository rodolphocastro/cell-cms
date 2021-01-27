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
    public class ListTagsTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class ListTagsHandlerTests
        {
            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullRequest_ThrowsArgumentNullException(
                ListTagsHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NullFeed_Returns(
                ListTags request,
                IEnumerable<Tag> tags,
                [Frozen] CellContext context,
                ListTagsHandler subject)
            {
                // Arrange
                request.FeedId = null;
                context.Tags.AddRange(tags);
                await context.SaveChangesAsync();

                // Act
                var result = await subject.Handle(request, default);

                // Assert
                result.Should()
                    .NotBeNull().And
                    .HaveSameCount(context.Tags);
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
            public async Task Handle_NotNullFeed_Returns(
                ListTags request,
                Feed feed,
                Tag tag,
                [Frozen] CellContext context,
                ListTagsHandler subject)
            {
                // Arrange
                request.FeedId = feed.Id;
                feed.Tags.Add(tag);
                context.Feeds.Add(feed);
                await context.SaveChangesAsync();

                // Act
                var result = await subject.Handle(request, default);

                // Assert
                result.Should()
                    .NotBeNull().And
                    .HaveSameCount(feed.Tags);
            }
        }
    }
}
