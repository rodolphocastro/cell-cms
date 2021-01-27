using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api.Models;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Xunit;

namespace CellCms.Tests.Unit.Models
{
    public class FeedTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        public class FeedQueriesTests
        {
            private readonly IEnumerable<Feed> _emptyResult;

            public FeedQueriesTests()
            {
                _emptyResult = Enumerable.Empty<Feed>();
            }

            [Fact]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void AllFeeds_EmptyData_ReturnsEmpty()
            {
                // Arrange
                var subject = _emptyResult.AsQueryable();

                // Act
                var result = subject.AllFeeds();

                // Assert
                result.Should().BeEmpty();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void AllFeeds_WithData_ReturnsData(IEnumerable<Feed> feeds)
            {
                // Arrange
                var subject = feeds.AsQueryable();

                // Act
                var result = subject.AllFeeds();

                // Assert
                result.Should()
                    .NotBeEmpty().And
                    .HaveSameCount(feeds);
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void ById_EmptyData_ReturnsEmpty(int id)
            {
                // Arrange
                var subject = _emptyResult.AsQueryable();

                // Act
                var result = subject.WithId(id);

                // Assert
                result.Should().BeEmpty();
                result.SingleOrDefault().Should().BeNull();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void ById_WithDataAndValidId_ReturnsData(IEnumerable<Feed> feeds, Feed extraFeed)
            {
                // Arrange
                var id = extraFeed.Id;
                var subject = feeds
                    .Concat(new List<Feed>() { extraFeed })
                    .AsQueryable();

                // Act
                var result = subject.WithId(id);

                // Assert
                result.Should().NotBeEmpty();
                result.SingleOrDefault().Should()
                    .BeEquivalentTo(extraFeed);
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void ById_WithDataAndInvalidId_ReturnsData(IEnumerable<Feed> feeds, [Frozen] int id)
            {
                // Arrange
                var realId = id + 200;
                var subject = feeds.AsQueryable();

                // Act
                var result = subject
                    .WithId(realId);

                // Assert
                result.Should().BeEmpty();
                result.SingleOrDefault().Should().BeNull();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void WithNome_EmptyData_ReturnsEmpty(string nome)
            {
                // Arrange
                var subject = _emptyResult.AsQueryable();

                // Act
                var result = subject
                    .FilterByNome(nome);

                // Assert
                result.Should().BeEmpty();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void WithNome_WithDataAndValidName_ReturnsData(IEnumerable<Feed> feeds, Feed extraFeed)
            {
                // Arrange
                var nome = extraFeed.Nome;
                var subject = feeds
                    .Concat(new List<Feed>() { extraFeed })
                    .AsQueryable();

                // Act
                var result = subject
                    .FilterByNome(nome);

                // Assert
                result.Should().NotBeEmpty();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void WithNome_WithDataAndInvalidName_ReturnsData(IEnumerable<Feed> feeds, [Frozen] string fixedName)
            {
                // Arrange
                var nome = $"{fixedName}{Guid.NewGuid()}";
                var subject = feeds.AsQueryable();

                // Act
                var result = subject
                    .FilterByNome(nome);

                // Assert
                result.Should().BeEmpty();
            }

            [Theory, CreateData]
            [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
            public void WithIdAndNome_WithDataAndValidNameAndValidID_ReturnsData(IEnumerable<Feed> feeds, Feed extraFeed)
            {
                // Arrange
                var nome = extraFeed.Nome;
                var id = extraFeed.Id;
                var subject = feeds
                    .Concat(new List<Feed>() { extraFeed })
                    .AsQueryable();

                // Act
                var result = subject
                    .WithId(id)
                    .FilterByNome(nome);

                // Assert
                result.Should().NotBeEmpty();
            }
        }
    }
}
