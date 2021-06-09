using System;

using CellCms.Api.Auth;
using CellCms.Api.Constants;

using FluentValidation.AspNetCore;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

using Newtonsoft.Json;

using Serilog;

namespace CellCms.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adicionando o Context de persistência
            services.AddCellPersistence(_configuration);

            // Adicionando os serviços de autenticação
            //services.AddCellAuthentication(_configuration);

            // Adicionando a geração do swagger.json
            services.AddCellSwagger(_configuration);

            services
                .AddControllers()
                .AddNewtonsoftJson(c =>
                {
                    c.SerializerSettings.MaxDepth = 4;
                    c.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .AddFluentValidation(c =>
                {
                    c.RegisterValidatorsFromAssembly(GetType().Assembly);   // Indicando para que o FluentValidations adicione os AbstractValidator<T> da API à DI
                    c.RunDefaultMvcValidationAfterFluentValidationExecutes = false; // Indicando para que a validação padrão não seja executada após o FluentValidation
                });

            // Adicionando os elementos do MediatR à injeção de dependência
            services.AddMediatR(GetType().Assembly);

            // Adicionando os nossos Profiles à injeção de dependência
            services.AddAutoMapper(GetType().Assembly);

            // Adicionando os serviços do ApplicationInsights
            services.AddApplicationInsightsTelemetry(_configuration);

            // Adicionando
            services.AddFeatureManagement(_configuration.GetSection(FeatureConstants.FeaturesConfigKey));

            services.AddApiClient(cfg =>
                _configuration.Bind(cfg));

            // Adicionando os serviços de HealthCheck
            services
                .AddHealthChecks()
                .AddDbContextCheck<CellContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Utilizando o Serilog para log das requests
            app.UseSerilogRequestLogging();

            // Usando o Middleware para expor o swagger.json
            app.UseCellSwaggerJson();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseApiClients();

            // Usando o Middleware para export o SwaggerUi
            app.UseCellSwaggerUi(_configuration);
        }
    }
}
