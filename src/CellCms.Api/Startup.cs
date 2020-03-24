using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CellCms.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var contextConnString = Configuration.GetConnectionString(CellContext.ConnectionStringKey);
            if (string.IsNullOrWhiteSpace(contextConnString))
            {
                throw new InvalidOperationException("A configura��o do context � obrigat�ria");
            }
            services.AddDbContext<CellContext>(opt => opt.UseSqlite(contextConnString));

            // Adicionando os servi�os de autentica��o
            services.AddCellAuthentication(Configuration);

            // Adicionando a gera��o do swagger.json
            services.AddCellSwagger(Configuration);

            services.AddControllers();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Usando o Middleware para export o SwaggerUi
            app.UseCellSwaggerUi(Configuration);
        }
    }
}
