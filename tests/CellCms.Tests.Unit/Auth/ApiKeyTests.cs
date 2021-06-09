using System.Collections.Generic;
using System.Linq;

using CellCms.Api.Auth;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Xunit;

namespace CellCms.Tests.Unit.Auth
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    public class ApiKeyTests
    {
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
        public class ApiClientQueriesTests
        {
            private readonly IEnumerable<ApiClient> _empty;

            public ApiClientQueriesTests()
            {
                _empty = Enumerable.Empty<ApiClient>();
            }

            [Theory, CreateData]
            public void WithKey_Contains_Returns(IEnumerable<ApiClient> clients)
            {
                // Arrange
                var subject = clients.ToList().AsQueryable();

                // Act
                var result = subject.WithKey(subject.First().Key);

                // Assert
                result.Should().NotBeNullOrEmpty();
            }

            [Theory, CreateData]
            public void WithKey_DoesntContain_ReturnsEmpty(string fakeKey)
            {
                // Arrange
                var subject = _empty.ToList().AsQueryable();

                // Act
                var result = subject.WithKey(fakeKey);

                // Assert
                result.Should().BeEmpty();
            }

            [Theory, CreateData]
            public void IsAuthorized_Contains_ReturnsTrue(IEnumerable<ApiClient> clients)
            {
                // Arrange
                var subject = clients.ToList().AsQueryable();
                var expected = subject.First();

                // Act
                var result = subject.IsAuthorized(expected.Key);

                // Assert
                result.Should().BeTrue();
            }

            [Theory, CreateData]
            public void IsAuthorized_DoesntContain_ReturnsFalse(string fakeKey)
            {
                // Arrange
                var subject = _empty.ToList().AsQueryable();

                // Act
                var result = subject.IsAuthorized(fakeKey);

                // Assert
                result.Should().BeFalse();
            }
        }

        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Domain)]
        public class ApiClientTests
        {
            [Theory, CreateData]
            public void Equals_String_MatchesKey(ApiClient subject)
            {
                // Arrange
                var key = subject.Key;

                // Act
                var result = subject.Equals(key);

                // Assert
                result.Should().BeTrue();
            }
        }
    }
}
