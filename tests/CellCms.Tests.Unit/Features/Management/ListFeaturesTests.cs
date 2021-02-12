using System;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api.Constants;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using Microsoft.FeatureManagement;

using NSubstitute;

using Xunit;

namespace CellCms.Tests.Unit.Features.Management
{
    public class ListFeaturesTests
    {
        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class ListFeaturesRequestTests
        {
            private readonly ListFeatures _subject;

            public ListFeaturesRequestTests()
            {
                _subject = new ListFeatures();
            }

            [Fact]
            public void EnabledOnly_Should_DefaultToTrue()
            {
                // Arrange

                // Act
                bool result = _subject.EnabledOnly;

                // Assert
                result.Should().BeTrue();
            }
        }

        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class ListFeaturesResponseTests
        {
            private readonly ListFeaturesResponse _subject;

            public ListFeaturesResponseTests()
            {
                _subject = new ListFeaturesResponse();
            }

            [Fact]
            public void FeatureName_Should_DefaultToEmpty()
            {
                // Arrange

                // Act
                string result = _subject.Name;

                // Assert
                result.Should().BeEmpty();
            }

            [Fact]
            public void FeatureStatus_Should_DefaultToFalse()
            {
                // Arrange

                // Act
                bool result = _subject.Status;

                // Assert
                result.Should().BeFalse();
            }

        }

        [Trait(TraitsConstants.Category.Name, TraitsConstants.Category.Values.Unit)]
        [Trait(TraitsConstants.Label.Name, TraitsConstants.Label.Values.Feature)]
        public class ListFeaturesHandlerTests
        {
            /// <summary>
            /// Helper to feed random features into IFeatureManager.
            /// </summary>
            /// <param name="rand"></param>
            /// <param name="features"></param>
            private static void SetupRandomFeatures(Random rand, IFeatureManager features)
            {
                foreach (var featName in FeatureConstants.AllFeaturesNames)
                {
                    features
                        .IsEnabledAsync(featName)
                        .Returns(rand.NextDouble() >= .5d);
                }
            }

            [Theory, CreateData]
            public async Task Handle_NullRequest_ArgumentNullException(
                ListFeaturesHandler subject)
            {
                // Arrange

                // Act
                Func<Task> act = () => subject.Handle(null, default);

                // Assert
                await act.Should().ThrowExactlyAsync<ArgumentNullException>();
            }


            [Theory, CreateData]
            public async Task Handle_EnabledOnly_FilteredConsts(
                ListFeatures query,
                Random rand,
                [Frozen] IFeatureManager features,
                [Greedy] ListFeaturesHandler subject)
            {
                // Arrange
                query.EnabledOnly = true;
                SetupRandomFeatures(rand, features);

                // Act
                var result = await subject.Handle(query, default);

                // Assert
                result
                    .Should()
                    .OnlyContain(r => r.Status);
            }

            [Theory, CreateData]
            public async Task Handle_All_AllConsts(
                ListFeatures query,
                Random rand,
                [Frozen] IFeatureManager features,
                [Greedy] ListFeaturesHandler subject)
            {
                // Arrange
                query.EnabledOnly = false;
                SetupRandomFeatures(rand, features);

                // Act
                var result = await subject.Handle(query, default);

                // Assert
                result
                    .Should()
                    .HaveSameCount(FeatureConstants.AllFeaturesNames);
            }
        }
    }
}
