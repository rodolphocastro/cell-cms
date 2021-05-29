using System;
using System.Collections.Generic;

using CellCms.Api.Settings;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Microsoft.Extensions.Configuration;

using Xunit;

namespace CellCms.Tests.Unit.Configuration
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public partial class ConfigurationExtensionsTests
    {
        public class AzureAdSetingsTests
        {
            [Theory, CreateData]
            public void GetAzureAdSettings_Valid_Returns(AzureAdSettings expected)
            {
                // Arrange                                                
                var subject = new ConfigurationBuilder()
                    .AddInMemoryCollection(new[] {
                        KeyValuePair.Create("AzureAd:AuthorizeEndpoint", expected.AuthorizeEndpoint),
                        KeyValuePair.Create("AzureAd:ClientId", expected.ClientId),
                        KeyValuePair.Create("AzureAd:MetadataEndpoint", expected.MetadataEndpoint),
                        KeyValuePair.Create("AzureAd:TokenEndpoint", expected.TokenEndpoint)
                    })
                    .Build();
                // Act
                var result = subject.GetAzureAdSettings();

                // Assert
                result.Should().NotBeNull();
                result.Should().BeEquivalentTo(expected);
            }

            [Fact]
            public void GetAzureAdSettings_Invalid_ThrowsInvalidOperation()
            {
                // Arrange                                                
                var subject = new ConfigurationBuilder()
                    .AddInMemoryCollection()
                    .Build();
                // Act

                Action act = () => subject.GetAzureAdSettings();

                // Assert
                act.Should().ThrowExactly<InvalidOperationException>();
            }

            [Fact]
            public void GetAzureAdSettings_Null_hrowsArgumentNull()
            {
                // Arrange
                IConfiguration subject = null;

                // Act
                Action act = () => subject.GetAzureAdSettings();

                // Assert
                act.Should().ThrowExactly<ArgumentNullException>();
            }
        }
    }
}
