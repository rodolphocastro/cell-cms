namespace CellCms.Tests.Unit.Utils
{
    /// <summary>
    /// Constantes para organizar os Traits dos testes.
    /// </summary>
    public static class TraitsConstants
    {
        /// <summary>
        /// Categorias para um teste.
        /// </summary>
        public static class Category
        {
            public const string Name = nameof(Category);

            public class Values
            {
                /// <summary>
                /// Teste Unitário.
                /// </summary>
                public const string Unit = nameof(Unit);

                /// <summary>
                /// Teste de Integração.
                /// </summary>
                public const string Integration = nameof(Integration);

                /// <summary>
                /// Teste de Funcionalidade.
                /// </summary>
                public const string Feature = nameof(Feature);
            }
        }

        /// <summary>
        /// Labels para um teste.
        /// </summary>
        public static class Label
        {
            public const string Name = nameof(Label);

            public class Values
            {
                /// <summary>
                /// Uma funcionalidade do sistema.
                /// </summary>
                public const string Feature = nameof(Feature);

                /// <summary>
                /// Um bug descoberto.
                /// </summary>
                public const string Bug = nameof(Bug);

                /// <summary>
                /// Utilitários, middlewares, configurações, et cetera...
                /// </summary>
                public const string Plumbing = nameof(Plumbing);

                /// <summary>
                /// O domínio em si.
                /// </summary>
                public const string Domain = nameof(Domain);
            }
        }
    }
}
