using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Move to appsettings
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Users\bruno\Documents\Github\TournamentTracker\WinFormsLog.txt")
                .CreateLogger();

            try
            {
                // Can create empty string array instead
                var host = CreateHostBuilder(args).Build();

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Initialize the database connections
                GlobalConfig.InitializeConnections(host.Services.GetService<IDataConnection>());
                Application.Run(host.Services.GetService<TournamentDashboardForm>());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a major problem that crashed the application");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Set to sql or text file saving
                    services.AddTransient<IDataConnection, TextConnection>();
                    services.AddTransient<TournamentDashboardForm>();
                    services.AddTransient<CreateTournamentForm>();
                    services.AddTransient<TournamentViewerForm>();
                    services.AddTransient<CreatePrizeForm>();
                    services.AddTransient<CreateTeamForm>();


                    services.AddSingleton<ILoggerFactory>(x =>
                    {
                        var providerCollection = x.GetService<LoggerProviderCollection>();
                        var factory = new SerilogLoggerFactory(null, true, providerCollection);

                        foreach (var provider in x.GetServices<ILoggerProvider>())
                        {
                            factory.AddProvider(provider);
                        }

                        return factory;
                    });
                });
        }
    }
}
