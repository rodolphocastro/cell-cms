using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using Serilog;
using Serilog.Formatting.Compact;

namespace CellCms.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // TODO: Mover configura��es para o appsettings
            // Criando a inst�ncia do Logger que utilizaremos na API
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()       // Caso estejamos em Debug, capturar estes logs
#else
                .MinimumLevel.Information() // Sen�o, apenas informa��es
#endif
                // Indicando que queremos mensagens Information que vierem do Namespace Microsoft
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                // Indicando que queremos mensagens Warning ou maiores que vierem do Namespace Microsoft.EntityFrameworkCore
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()    // Indicando que � pra utilizar enriquecer os logs automaticamente com o contexto de execu��o
                .WriteTo.Console()          // Indicando que os logs devem ser apresentados no console
                .WriteTo.File(new CompactJsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day) // Indicando que os logs devem ser salvos em um txt para cada dia
#if DEBUG
                .WriteTo.Debug()    // Indicando que os logs devem ser apresentados no Debug Console
#endif
                .CreateLogger();    // Criando o logger, finalmente

            // Como agora temos log antes da execu��o, podemos encapsular a execu��o com um Try-Catch para logar erros cr�ticos
            try
            {
                CreateHostBuilder(args)
                .Build()
                .EnsureDatabaseIsMigrated()
                .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Erro ao iniciar a API");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
