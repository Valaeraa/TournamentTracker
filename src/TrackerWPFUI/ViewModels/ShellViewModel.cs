using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;
using Stylet;

namespace TrackerWPFUI.ViewModels
{
    // Bug - 1 Binding failure when creating new tournament
    public class ShellViewModel : Conductor<object>, IHandle<TournamentModel>
    {
        private BindableCollection<TournamentModel> _existingTournaments;
        private TournamentModel _selectedTournament;
        //private readonly ILogger<ShellViewModel> _logger;
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceProvider _service;

        public ShellViewModel(/*ILogger<ShellViewModel> logger,*/ IEventAggregator eventAggregator, IDataConnection db, IServiceProvider service)
        {
            // Initialize the database connections
            GlobalConfig.InitializeConnections(db);

            //EventAggregationProvider.TrackerEventAggregator.SubscribeOnPublishedThread(this);

            _existingTournaments = new BindableCollection<TournamentModel>(GlobalConfig.Connection.GetTournament_All());
            //_logger = logger;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _service = service;
        }

        public void CreateTournament()
        {
            var viewModel = _service.GetService<CreateTournamentViewModel>();
            ActivateItem(viewModel);
        }

        public void LoadTournament()
        {
            if (SelectedTournament != null && !string.IsNullOrWhiteSpace(SelectedTournament.TournamentName))
            {
                var viewModel = _service.GetService<TournamentViewerViewModel>();
                viewModel.SelectedTournamentRequester(SelectedTournament);

                ActivateItem(viewModel /*new TournamentViewerViewModel(SelectedTournament)*/);
            }
        }

        public void Handle(TournamentModel message)
        {
            ExistingTournaments.Add(message);
            SelectedTournament = message;
        }

        public BindableCollection<TournamentModel> ExistingTournaments
        {
            get { return _existingTournaments; }
            set { _existingTournaments = value; }
        }
        
        public TournamentModel SelectedTournament
        {
            get { return _selectedTournament; }
            set
            {
                _selectedTournament = value;
                NotifyOfPropertyChange(() => SelectedTournament);
                LoadTournament();
            }
        }
    }
}
