using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.DataAccess;
using TrackerWPFUI.ViewModels;

namespace TrackerWPFUI
{
    public class Bootstrapper : BootstrapperMS<ShellViewModel>
    {
        public Bootstrapper()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .WriteTo.File(@"C:\Users\bruno\Documents\Github\TournamentTracker\WPFLog.txt")
               .CreateLogger();
        }

        protected override void ConfigureIoC(IServiceCollection services)
        {
            base.ConfigureIoC(services);

            //services.AddTransient<ShellViewModel>();
            services.AddTransient<IDataConnection, TextConnection>();

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
        }
    }
}
