using CellCms.Api.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
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
            // Buscando as nossas configurações do Azure AD
            var aadSettings = Configuration.GetAzureAdSettings();

            if (aadSettings is null)
            {
                throw new InvalidOperationException("As configurações do Azure AD são obrigatórias para o Cell CMS.");
            }

            // Adicionando os serviços de autenticação
            services
                .AddAuthentication(c =>
                {
                    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;   // Definindo que os JWTs serão nosso schema de autenticação padrão
                    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Definindo que os JWTs serão nosso schema padrão para challenges de autenticação
                })
                .AddJwtBearer(c =>
                {
                    c.IncludeErrorDetails = true;   // Garantindo que o Middleware nos retorno erros detalhados, em produção pode ser melhor desabilitar
                    c.SaveToken = true; // Indica que o Token deve ser salvo
                    // As linhas abaixo configuram a validação do Token, com base nas nossas configurações
                    c.MetadataAddress = aadSettings.MetadataEndpoint;
                    c.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = aadSettings.ClientId,
                        ValidateAudience = true,
                        // Na documentação (https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-convert-app-to-be-multi-tenant#update-your-code-to-handle-multiple-issuer-values)
                        // recomenda-se validar Guid-a-Guid os issuers para cada tenant que puder utilizar nosso App.
                        // Como nossa ideia aqui é fazer apenas o App, não vamos fazer este passo
                        ValidateIssuer = false
                    };
                });

            // Adicionando a geração do swagger.json
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
