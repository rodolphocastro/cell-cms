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
            // TODO: Mover configurações para o appsettings
            // Criando a instância do Logger que utilizaremos na API
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()       // Caso estejamos em Debug, capturar estes logs
#else
                .MinimumLevel.Information() // Senão, apenas informações
#endif
                // Indicando que queremos mensagens Information que vierem do Namespace Microsoft
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                // Indicando que queremos mensagens Warning ou maiores que vierem do Namespace Microsoft.EntityFrameworkCore
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
                .Enrich.FromLogContext()    // Indicando que é pra utilizar enriquecer os logs automaticamente com o contexto de execução
                .WriteTo.Console()          // Indicando que os logs devem ser apresentados no console
                .WriteTo.File(new CompactJsonFormatter(), "logs/log.txt", rollingInterval: RollingInterval.Day) // Indicando que os logs devem ser salvos em um txt para cada dia
#if DEBUG
                .WriteTo.Debug()    // Indicando que os logs devem ser apresentados no Debug Console
#endif
                .CreateLogger();    // Criando o logger, finalmente

            // Como agora temos log antes da execução, podemos encapsular a execução com um Try-Catch para logar erros críticos
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
