using System.Collections.Generic;

using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Xunit;

namespace CellCms.Tests.Unit.Features.Management
{
    [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
    [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Plumbing)]
    public class FeaturesConstTests
    {
        [Fact]
        public void FeatureConsts_Should_HaveCMS()
        {
            // Arrange

            // Act
            string result = FeatureConstants.BasicCMS;

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void FeatureConsts_Should_HaveGraphQL()
        {
            // Arrange

            // Act
            string result = FeatureConstants.GraphQL;

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void FeatureConsts_Should_HaveRSSFeed()
        {
            // Arrange

            // Act
            string result = FeatureConstants.RssFeed;

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void FeatureConsts_Should_HaveMetrics()
        {
            // Arrange

            // Act
            string result = FeatureConstants.Metrics;

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void FeatureConsts_Should_HaveBotIntentApi()
        {
            // Arrange

            // Act
            string result = FeatureConstants.BotIntentApi;

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void FeatureConsts_Should_HaveAllFeatures()
        {
            // Arrange
            IReadOnlyList<string> allFeatures = new List<string>
            {
                FeatureConstants.BasicCMS,
                FeatureConstants.BotIntentApi,
                FeatureConstants.GraphQL,
                FeatureConstants.Metrics,
                FeatureConstants.RssFeed
            };

            // Act
            IEnumerable<string> result = FeatureConstants.AllFeaturesNames;

            // Assert
            result.Should()
                .NotBeNull().And
                .Contain(allFeatures);
        }
    }

    /// <summary>
    /// Constantes para controle de Features.
    /// </summary>
    internal class FeatureConstants
    {
        /// <summary>
        /// Flag para funções básicas de CMS.
        /// </summary>
        internal static readonly string BasicCMS = nameof(BasicCMS);

        /// <summary>
        /// Flag para ativar um endpoint GraphQL.
        /// </summary>
        internal static readonly string GraphQL = nameof(GraphQL);

        /// <summary>
        /// Flag para ativar um Feed RSS.
        /// </summary>
        internal static readonly string RssFeed = nameof(RssFeed);

        /// <summary>
        /// Flag para API de métricas de uso.
        /// </summary>
        internal static readonly string Metrics = nameof(Metrics);

        /// <summary>
        /// Flag para a API de processamento de Intents.
        /// </summary>
        internal static readonly string BotIntentApi = nameof(BotIntentApi);

        /// <summary>
        /// Todas as Features disponíveis.
        /// </summary>
        internal static readonly IEnumerable<string> AllFeaturesNames = new List<string>
        {
            BasicCMS,
            GraphQL,
            RssFeed,
            Metrics,
            BotIntentApi
        };
    }
}
