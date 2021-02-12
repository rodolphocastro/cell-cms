using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoFixture.Xunit2;

using CellCms.Api.Constants;
using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using MediatR;

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

    /// <summary>
    /// Handler para Listar Features
    /// </summary>
    public class ListFeaturesHandler : IRequestHandler<ListFeatures, IEnumerable<ListFeaturesResponse>>
    {
        private readonly IFeatureManager _featureManager;
        private readonly IEnumerable<string> _features;

        public ListFeaturesHandler(IFeatureManager featureManager) : this()
        {
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        public ListFeaturesHandler()
        {
            _features = FeatureConstants.AllFeaturesNames;
        }

        public Task<IEnumerable<ListFeaturesResponse>> Handle(ListFeatures request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return HandleInternalAsync(request.EnabledOnly, cancellationToken);
        }

        private async Task<IEnumerable<ListFeaturesResponse>> HandleInternalAsync(bool enabledOnly, CancellationToken cancellationToken)
        {
            List<ListFeaturesResponse> result = new List<ListFeaturesResponse>();

            foreach (var feat in _features)
            {
                result.Add(new ListFeaturesResponse
                {
                    Name = feat,
                    Status = await _featureManager.IsEnabledAsync(feat)
                });
            }

            return enabledOnly ? result.Where(r => r.Status) : result;
        }
    }

    /// <summary>
    /// Query para Listar Features.
    /// </summary>
    public class ListFeatures : IRequest<IEnumerable<ListFeaturesResponse>>
    {
        public ListFeatures()
        {
        }

        /// <summary>
        /// Filtrar por ativas
        /// </summary>
        public bool EnabledOnly { get; set; } = true;
    }

    /// <summary>
    /// Resposta para a query de Listar Features.
    /// </summary>
    public class ListFeaturesResponse
    {
        public ListFeaturesResponse()
        {
        }

        /// <summary>
        /// Status (ativa/inativa) da Feature.
        /// </summary>
        public bool Status { get; init; }

        /// <summary>
        /// Nome da Feature.
        /// </summary>
        public string Name { get; init; } = string.Empty;
    }
}
