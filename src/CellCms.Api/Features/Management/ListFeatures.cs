using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CellCms.Api.Constants;

using MediatR;

using Microsoft.FeatureManagement;

namespace CellCms.Tests.Unit.Features.Management
{
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
}
