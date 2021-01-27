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

using Xunit;

namespace CellCms.Tests.Unit.Features.Contents
{
    public class ListContentTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class ListContentHandlerTests
        {
            [Theory, CreateData]
            public async Task Handle_NullRequest_ThrowsArgumentNullException(
                ListContentHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }

            [Theory, CreateData]
            public async Task Handle_NoFeedNoTag_ReturnsAll(
                IEnumerable<Content> contents,
                [Frozen] CellContext ctx,
                ListContentHandler subject)
            {
                // Arrange
                ctx.Contents.AddRange(contents);
                await ctx.SaveChangesAsync();
                var query = new ListContent { };

                // Act
                var result = await subject.Handle(query, default);

                // Assert
                result.Should()
                    .NotBeNull().And
                    .HaveSameCount(contents);
            }

            [Theory, CreateData]
            public async Task Handle_FeedId_ReturnsFilteredByFeed(
                Feed f,
                Content c,
                IEnumerable<Content> contents,
                [Frozen] CellContext ctx,
                ListContentHandler subject)
            {
                // Arrange
                f.Contents.Add(c);
                ctx.Feeds.Add(f);
                ctx.Contents.AddRange(contents);
                await ctx.SaveChangesAsync();
                var query = new ListContent { FeedId = f.Id };

                // Act
                var result = await subject.Handle(query, default);

                // Assert
                result.Should()
                    .NotBeNull().And
                    .OnlyContain(i => i.FeedId.Equals(f.Id));
            }

            [Theory, CreateData]
            public async Task Handle_TagId_ReturnsFilteredByTag(
                Tag t,
                Content c,
                IEnumerable<Content> contents,
                [Frozen] CellContext ctx,
                ListContentHandler subject)
            {
                // Arrange
                ctx.Add(t);
                c.ContentTags.Add(new ContentTag { Content = c, Tag = t });
                ctx.Add(c);
                ctx.AddRange(contents);
                await ctx.SaveChangesAsync();
                var query = new ListContent { TagId = t.Id };

                // Act
                var result = await subject.Handle(query, default);

                // Assert
                result.Should()
                    .NotBeNull().And
                    .OnlyContain(c =>
                        c.ContentTags.All(ct => ct.TagId.Equals(t.Id))
                    );
            }
        }
    }
}
