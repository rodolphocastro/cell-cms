using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CellCms.Api.Auth
{
    /// <summary>
    /// Um cliente autorizado a chamar a API.
    /// </summary>
    public record ApiClient(string Name, string Key) : IEquatable<string>
    {
        public virtual bool Equals(string other) => Key.Equals(other, StringComparison.InvariantCulture);
    }

    /// <summary>
    /// Queries para consultar ApiClients.
    /// </summary>
    public static class ApiClientQueries
    {
        /// <summary>
        /// Filtra clients baseados em uma key.
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static IQueryable<ApiClient> WithKey(this IQueryable<ApiClient> clients, string key) => clients.Where(c => c.Equals(key));

        /// <summary>
        /// Query para verificar se existe um client para uma key específica.
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsAuthorized(this IQueryable<ApiClient> clients, string key) => clients.WithKey(key).Any();
    }
    /// <summary>
    /// Configuração do Middleware de Api Key.
    /// </summary>
    public class ApiClientServiceParameters
    {
        public Dictionary<string, string> ApiKeys { get; set; } = new();
        public IEnumerable<ApiClient> Clients => ApiKeys.Select(kv => new ApiClient(kv.Key, kv.Value));
    }

    public class ApiClientMiddleware
    {
        private const string ApiKeyHeader = "ApiKey";
        private readonly RequestDelegate _next;
        private readonly IOptionsMonitor<ApiClientServiceParameters> _middlewareOptionsMonitor;

        private ApiClientServiceParameters GetParameters() => _middlewareOptionsMonitor.CurrentValue;
        private IEnumerable<ApiClient> GetClients() => GetParameters().Clients;

        public ApiClientMiddleware(RequestDelegate next, IOptionsMonitor<ApiClientServiceParameters> middlewareOptionsMonitor)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _middlewareOptionsMonitor = middlewareOptionsMonitor ?? throw new ArgumentNullException(nameof(middlewareOptionsMonitor));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var headerApiKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            var extractedKey = headerApiKey.ToString();
            var authorizedClients = GetClients().ToList();
            if (!authorizedClients.AsQueryable().IsAuthorized(extractedKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await _next(context);
        }
    }

    public static class ApiClientStartupExtensions
    {
        public static IServiceCollection AddApiClient(this IServiceCollection services, Action<ApiClientServiceParameters> paramsFactory)
        {
            if (paramsFactory is null)
            {
                throw new ArgumentNullException(nameof(paramsFactory));
            }

            services.Configure(paramsFactory);
            return services;
        }

        public static IApplicationBuilder UseApiClients(this IApplicationBuilder app)
        {
            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseMiddleware<ApiClientMiddleware>();
            });
            return app;
        }
    }
}
