using Caliburn.Micro;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerWPFUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<TournamentModel>
    {
        private BindableCollection<TournamentModel> _existingTournaments;
        private TournamentModel _selectedTournament;

        public ShellViewModel()
        {
            // Initialize the database connections
            GlobalConfig.InitializeConnections(DatabaseType.TextFile);

            EventAggregationProvider.TrackerEventAggregator.SubscribeOnPublishedThread(this);

            _existingTournaments = new BindableCollection<TournamentModel>(GlobalConfig.Connection.GetTournament_All());
        }

        public async Task CreateTournament()
        {
            await ActivateItemAsync(new CreateTournamentViewModel());
        }

        public async Task LoadTournament()
        {
            if (SelectedTournament != null && !String.IsNullOrWhiteSpace(SelectedTournament.TournamentName))
            {
                await ActivateItemAsync(new TournamentViewerViewModel(SelectedTournament));
            }
        }

        public async Task HandleAsync(TournamentModel message, CancellationToken cancellationToken)
        {
            // Open the tournament viewer to the given tournament
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
