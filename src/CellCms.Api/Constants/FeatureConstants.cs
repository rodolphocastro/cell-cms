using System.Collections.Generic;

namespace CellCms.Api.Constants
{
    /// <summary>
    /// Constantes para controle de Features.
    /// </summary>
    public class FeatureConstants
    {
        /// <summary>
        /// Flag para funções básicas de CMS.
        /// </summary>
        public const string BasicCMS = nameof(BasicCMS);

        /// <summary>
        /// Flag para ativar um endpoint GraphQL.
        /// </summary>
        public const string GraphQL = nameof(GraphQL);

        /// <summary>
        /// Flag para ativar um Feed RSS.
        /// </summary>
        public const string RssFeed = nameof(RssFeed);

        /// <summary>
        /// Flag para API de métricas de uso.
        /// </summary>
        public const string Metrics = nameof(Metrics);

        /// <summary>
        /// Flag para a API de processamento de Intents.
        /// </summary>
        public const string BotIntentApi = nameof(BotIntentApi);

        /// <summary>
        /// Todas as Features disponíveis.
        /// </summary>
        public static readonly IEnumerable<string> AllFeaturesNames = new List<string>
        {
            BasicCMS,
            GraphQL,
            RssFeed,
            Metrics,
            BotIntentApi
        };
    }
}
