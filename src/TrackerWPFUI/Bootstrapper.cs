using Caliburn.Micro;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using TrackerWPFUI.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TrackerLibrary.DataAccess;

namespace TrackerWPFUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private IHost _host;

        public Bootstrapper()
        {
            Initialize();

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .WriteTo.File(@"C:\Users\bruno\Documents\Github\TournamentTracker\WPFLog.txt")
               .CreateLogger();

            _host = CreateHostBuilder(new string[0]).Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ShellViewModel>();
                    services.AddTransient<CreateTournamentViewModel>();
                    services.AddTransient<TournamentViewerViewModel>();
                    services.AddTransient<CreateTeamViewModel>();
                    services.AddTransient<CreatePrizeViewModel>();
                    services.AddTransient<CreatePersonViewModel>();
                    services.AddTransient<IDataConnection, TextConnection>();
                    services.AddSingleton<IWindowManager, WindowManager>();
                    services.AddSingleton<IEventAggregator, EventAggregator>();
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

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _host.Services.GetService(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _host.Services.GetServices(service);
        }

        protected override void BuildUp(object instance)
        {
        }
    }
}