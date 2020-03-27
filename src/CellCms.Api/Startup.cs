using System;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            // Adicionando o Context de persist�ncia
            services.AddCellPersistence(_configuration);

            // Adicionando os servi�os de autentica��o
            services.AddCellAuthentication(_configuration);

            // Adicionando a gera��o do swagger.json
            services.AddCellSwagger(_configuration);

            services.AddControllers();

            // Adicionando os elementos do MediatR � inje��o de depend�ncia
            services.AddMediatR(GetType().Assembly);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Usando o Middleware para expor o swagger.json
            app.UseCellSwaggerJson();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            // Usando o Middleware para export o SwaggerUi
            app.UseCellSwaggerUi(_configuration);
        }
    }
}
