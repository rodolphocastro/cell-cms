using System;
using System.Collections.Generic;

using CellCms.Api.Settings;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace CellCms.Tests.Unit.Swagger
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public partial class SwaggerExtensionsTests
    {
        public class SwaggerServicesExtensionsTests
        {
            [Theory, CreateData]
            public void AddCellSwagger_NullCollection_ThrowsArgumentNullException(
                IConfiguration config)
            {
                // Arrange
                IServiceCollection subject = null;

                // Act
                Action act = () => subject.AddCellSwagger(config);

                // Assert
                act.Should().ThrowExactly<ArgumentNullException>();
            }

            [Theory, CreateData]
            public void AddCellSwagger_NullConfiguration_ThrowsArgumentNullException(
                IServiceCollection subject)
            {
                // Arrange

                // Act
                Action act = () => subject.AddCellSwagger(null);

                // Assert
                act.Should().ThrowExactly<ArgumentNullException>();
            }

            [Theory, CreateData]
            public void AddCellSwagger_Valid_Returns(
                AzureAdSettings settings,
                IServiceCollection subject)
            {
                // Arrange                
                var config = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "AzureAd:AuthorizeEndpoint", settings.AuthorizeEndpoint },
                        { "AzureAd:ClientId", settings.ClientId },
                        { "AzureAd:MetadataEndpoint", settings.MetadataEndpoint },
                        { "AzureAd:TokenEndpoint", settings.TokenEndpoint }
                    })
                    .Build();

                // Act
                var result = subject.AddCellSwagger(config);

                // Assert
                result.Should().NotBeNull();
            }
        }
    }
}
