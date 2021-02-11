
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using CellCms.Tests.Unit.Utils;

using FluentAssertions;

using MediatR;

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

            // TODO: Write behavior tests
        }
    }

    public class ListFeaturesHandler : IRequestHandler<ListFeatures, IEnumerable<ListFeaturesResponse>>
    {
        public ListFeaturesHandler()
        {

        }

        public Task<IEnumerable<ListFeaturesResponse>> Handle(ListFeatures request, CancellationToken cancellationToken) => throw new System.NotImplementedException();
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
        public bool EnabledOnly { get; set; }
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
        public string Name { get; init; }
    }
}
