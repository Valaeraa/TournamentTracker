using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerWPFUI.EventModels;

namespace TrackerWPFUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<TournamentModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly TournamentViewerViewModel _tournamentViewerVM;

        private BindableCollection<TournamentModel> _existingTournaments;
        private TournamentModel _selectedTournament;

        public ShellViewModel(IEventAggregator eventAggregator, TournamentViewerViewModel tournamentViewer)
        {
            // Initialize the database connections
            GlobalConfig.InitializeConnections(DatabaseType.Sql);

            _tournamentViewerVM = tournamentViewer;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            _existingTournaments = new BindableCollection<TournamentModel>(GlobalConfig.Connection.GetTournament_All());
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

        public void CreateTournament()
        {
            ActivateItem(IoC.Get<CreateTournamentViewModel>());
        }

        public void LoadTournament()
        {
            if (SelectedTournament != null && !String.IsNullOrWhiteSpace(SelectedTournament.TournamentName))
            {
                _eventAggregator.PublishOnUIThread(new SelectedTournamentEvent(SelectedTournament));

                ActivateItem(_tournamentViewerVM);
            }
        }

        public void Handle(TournamentModel message)
        {
            // Open the tournament viewer to the given tournament
            ExistingTournaments.Add(message);
            SelectedTournament = message;
        }
    }
}
