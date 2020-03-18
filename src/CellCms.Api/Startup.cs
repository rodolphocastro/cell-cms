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
            // Buscando as nossas configura��es do Azure AD
            var aadSettings = Configuration.GetAzureAdSettings();

            if (aadSettings is null)
            {
                throw new InvalidOperationException("As configura��es do Azure AD s�o obrigat�rias para o Cell CMS.");
            }

            // Adicionando os servi�os de autentica��o
            services
                .AddAuthentication(c =>
                {
                    c.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;   // Definindo que os JWTs ser�o nosso schema de autentica��o padr�o
                    c.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Definindo que os JWTs ser�o nosso schema padr�o para challenges de autentica��o
                })
                .AddJwtBearer(c =>
                {
                    c.IncludeErrorDetails = true;   // Garantindo que o Middleware nos retorno erros detalhados, em produ��o pode ser melhor desabilitar
                    c.SaveToken = true; // Indica que o Token deve ser salvo
                    // As linhas abaixo configuram a valida��o do Token, com base nas nossas configura��es
                    c.MetadataAddress = aadSettings.MetadataEndpoint;
                    c.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = aadSettings.ClientId,
                        ValidateAudience = true,
                        // Na documenta��o (https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-convert-app-to-be-multi-tenant#update-your-code-to-handle-multiple-issuer-values)
                        // recomenda-se validar Guid-a-Guid os issuers para cada tenant que puder utilizar nosso App.
                        // Como nossa ideia aqui � fazer apenas o App, n�o vamos fazer este passo
                        ValidateIssuer = false
                    };
                });

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
