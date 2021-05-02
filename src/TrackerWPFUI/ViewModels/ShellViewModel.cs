using Caliburn.Micro;
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

namespace TrackerWPFUI.ViewModels
{
    // Bug - 1 Binding failure when creating new tournament
    public class ShellViewModel : Conductor<object>, IHandle<TournamentModel>
    {
        private BindableCollection<TournamentModel> _existingTournaments;
        private TournamentModel _selectedTournament;
        private readonly ILogger<ShellViewModel> _logger;
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceProvider _service;

        public ShellViewModel(ILogger<ShellViewModel> logger, IEventAggregator eventAggregator, IDataConnection db, IServiceProvider service)
        {
            // Initialize the database connections
            GlobalConfig.InitializeConnections(db);

            //EventAggregationProvider.TrackerEventAggregator.SubscribeOnPublishedThread(this);

            _existingTournaments = new BindableCollection<TournamentModel>(GlobalConfig.Connection.GetTournament_All());
            _logger = logger;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnPublishedThread(this);
            _service = service;
        }

        public async Task CreateTournament()
        {
            var viewModel = _service.GetService<CreateTournamentViewModel>();
            await ActivateItemAsync(viewModel);
        }

        public async Task LoadTournament()
        {
            if (SelectedTournament != null && !string.IsNullOrWhiteSpace(SelectedTournament.TournamentName))
            {
                var viewModel = _service.GetService<TournamentViewerViewModel>();
                viewModel.SelectedTournamentRequester(SelectedTournament);

                await ActivateItemAsync(viewModel /*new TournamentViewerViewModel(SelectedTournament)*/);
            }
        }

        public async Task HandleAsync(TournamentModel message, CancellationToken cancellationToken)
        {
            // Open the tournament viewer to the given tournament
            await Task.Run(() =>
            {
                ExistingTournaments.Add(message);
                SelectedTournament = message;
            });
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
