using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            GlobalConfig.InitializeConnections(DatabaseType.Sql);

            EventAggregationProvider.TrackerEventAggregator.Subscribe(this);

            _existingTournaments = new BindableCollection<TournamentModel>(GlobalConfig.Connection.GetTournament_All());
        }

        public void CreateTournament()
        {
            ActivateItem(new CreateTournamentViewModel());
        }

        public void LoadTournament()
        {
            if (SelectedTournament != null && !String.IsNullOrWhiteSpace(SelectedTournament.TournamentName))
            {
                ActivateItem(new TournamentViewerViewModel(SelectedTournament));
            }
        }

        public void Handle(TournamentModel message)
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
