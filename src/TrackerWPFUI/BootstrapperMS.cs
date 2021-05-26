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
using Stylet;
using System.Reflection;

namespace TrackerWPFUI
{
    public class BootstrapperMS<TRootViewModel> : BootstrapperBase where TRootViewModel : class
    {
        private ServiceProvider _serviceProvider;

        private TRootViewModel _rootViewModel;
        protected virtual TRootViewModel RootViewModel
        {
            get { return this._rootViewModel ?? (this._rootViewModel = (TRootViewModel)this.GetInstance(typeof(TRootViewModel))); }
        }

        public IServiceProvider ServiceProvider
        {
            get { return this._serviceProvider; }
        }

        protected override void ConfigureBootstrapper()
        {
            var services = new ServiceCollection();
            this.DefaultConfigureIoC(services);
            this.ConfigureIoC(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected virtual void DefaultConfigureIoC(IServiceCollection services)
        {
            var viewManagerConfig = new ViewManagerConfig()
            {
                ViewFactory = this.GetInstance,
                ViewAssemblies = new List<Assembly>() { this.GetType().Assembly }
            };

            services.AddSingleton<IViewManager>(new ViewManager(viewManagerConfig));
            services.AddTransient<MessageBoxView>();

            services.AddSingleton<IWindowManagerConfig>(this);
            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddTransient<IMessageBoxViewModel, MessageBoxViewModel>(); // Not singleton!
            // Also need a factory
            services.AddSingleton<Func<IMessageBoxViewModel>>(() => new MessageBoxViewModel());

            
        }

        /// <summary>
        /// Override to add your own types to the IoC container.
        /// </summary>
        protected virtual void ConfigureIoC(IServiceCollection services) { }

        public override object GetInstance(Type type)
        {
            return this._serviceProvider.GetRequiredService(type);
        }

        protected override void Launch()
        {
            base.DisplayRootView(this.RootViewModel);
        }

        public override void Dispose()
        {
            base.Dispose();

            ScreenExtensions.TryDispose(this._rootViewModel);
            if (this._serviceProvider != null)
                this._serviceProvider.Dispose();
        }
    }
}