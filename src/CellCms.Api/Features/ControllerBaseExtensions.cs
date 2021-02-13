using System.Threading;

using Microsoft.AspNetCore.Mvc;

namespace CellCms.Api.Features
{
    /// <summary>
    /// Métodos para estender a funcionalidade dos Controllers
    /// </summary>
    public static class ControllerBaseExtensions
    {
        /// <summary>
        /// Roda padrão para um Controller do projeto.
        /// </summary>
        public const string DefaultRoute = "api/[controller]";

        /// <summary>
        /// Obtém um CancellationToken de uma Request HTTP OU cria um novo.
        /// </summary>
        /// <param name="controller"></param>
        /// <returns>Um CancellationToken pronto para uso</returns>
        public static CancellationToken GetRequestCancellationToken(this ControllerBase controller)
        {
            return controller?.HttpContext?.RequestAborted ?? default;
        }
    }
}
