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
    public class CreateTournamentViewModel : Conductor<object>.Collection.AllActive, IHandle<TeamModel>, IHandle<PrizeModel>
    {
        private readonly IEventAggregator _eventAggregator;

        private string _tournamentName = "";
        private decimal _entryFee;
        private BindableCollection<TeamModel> _availibleTeams = new BindableCollection<TeamModel>();
        private TeamModel _selectedTeamToAdd;
        private BindableCollection<TeamModel> _selectedTeams = new BindableCollection<TeamModel>();
        private TeamModel _selectedTeamToRemove;
        private Screen _activeAddTeamView;
        private BindableCollection<PrizeModel> _selectedPrizes = new BindableCollection<PrizeModel>();
        private PrizeModel _selectedPrizeToRemove;
        private Screen _activeAddPrizeView;
        private bool _selectedTeamsIsVisible = true;
        private bool _addTeamIsVisible = false;
        private bool _selectedPrizesIsVisible = true;
        private bool _addPrizeIsVisible = false;

        public CreateTournamentViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            // Initialize the avalibleTeams list with all of the teams in our database/text files
            AvailibleTeams = new BindableCollection<TeamModel>(GlobalConfig.Connection.GetTeam_All());
        }

        public string TournamentName
        {
            get { return _tournamentName; }
            set
            {
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }
        }

        public decimal EntryFee
        {
            get { return _entryFee; }
            set
            {
                _entryFee = value;
                NotifyOfPropertyChange(() => EntryFee);
            }
        }

        public BindableCollection<TeamModel> AvailibleTeams
        {
            get { return _availibleTeams; }
            set
            { _availibleTeams = value; }
        }
        
        public TeamModel SelectedTeamToAdd
        {
            get { return _selectedTeamToAdd; }
            set
            {
                _selectedTeamToAdd = value;
                NotifyOfPropertyChange(() => SelectedTeamToAdd);
                NotifyOfPropertyChange(() => CanAddTeam);
            }
        }
        
        public BindableCollection<TeamModel> SelectedTeams
        {
            get { return _selectedTeams; }
            set
            {
                _selectedTeams = value;
                NotifyOfPropertyChange(() => SelectedTeams);
            }
        }

        public TeamModel SelectedTeamToRemove
        {
            get { return _selectedTeamToRemove; }
            set
            {
                _selectedTeamToRemove = value;
                NotifyOfPropertyChange(() => SelectedTeamToRemove);
                NotifyOfPropertyChange(() => CanRemoveTeam);
            }
        }

        public Screen ActiveAddTeamView
        {
            get { return _activeAddTeamView; }
            set
            {
                _activeAddTeamView = value;
                NotifyOfPropertyChange(() => ActiveAddTeamView);
            }
        }

        public BindableCollection<PrizeModel> SelectedPrizes
        {
            get { return _selectedPrizes; }
            set { _selectedPrizes = value; }
        }

        public PrizeModel SelectedPrizeToRemove
        {
            get { return _selectedPrizeToRemove; }
            set
            {
                _selectedPrizeToRemove = value;
                NotifyOfPropertyChange(() => SelectedPrizeToRemove);
                NotifyOfPropertyChange(() => CanRemovePrize);
            }
        }
        
        public Screen ActiveAddPrizeView
        {
            get { return _activeAddPrizeView; }
            set
            {
                _activeAddPrizeView = value;
                NotifyOfPropertyChange(() => ActiveAddPrizeView);
            }
        }
        
        public bool SelectedTeamsIsVisible
        {
            get { return _selectedTeamsIsVisible; }
            set
            {
                _selectedTeamsIsVisible = value;
                NotifyOfPropertyChange(() => SelectedTeamsIsVisible);
            }
        }
        
        public bool AddTeamIsVisible
        {
            get { return _addTeamIsVisible; }
            set
            {
                _addTeamIsVisible = value;
                NotifyOfPropertyChange(() => AddTeamIsVisible);
            }
        }

        public bool SelectedPrizesIsVisible
        {
            get { return _selectedPrizesIsVisible; }
            set
            {
                _selectedPrizesIsVisible = value;
                NotifyOfPropertyChange(() => SelectedPrizesIsVisible);
            }
        }

        public bool AddPrizeIsVisible
        {
            get { return _addPrizeIsVisible; }
            set
            {
                _addPrizeIsVisible = value;
                NotifyOfPropertyChange(() => AddPrizeIsVisible);
            }
        }

        public bool CanAddTeam
        {
            get
            {
                return SelectedTeamToAdd != null;
            }
        }

        public void AddTeam()
        {
            SelectedTeams.Add(SelectedTeamToAdd);
            AvailibleTeams.Remove(SelectedTeamToAdd);

            NotifyOfPropertyChange(() => CanCreateTournament);
        }

        public void CreateTeam()
        {
            // Create a new CreateTeamViewModel() and add it to the property
            ActiveAddTeamView = IoC.Get<CreateTeamViewModel>();

            // Items is a list of controls
            // Add the property to the items list
            Items.Add(ActiveAddTeamView);

            SelectedTeamsIsVisible = false;
            AddTeamIsVisible = true;
        }

        public bool CanRemoveTeam
        {
            get
            {
                return SelectedTeamToRemove != null;
            }
        }

        public void RemoveTeam()
        {
            AvailibleTeams.Add(SelectedTeamToRemove);
            SelectedTeams.Remove(SelectedTeamToRemove);

            NotifyOfPropertyChange(() => CanCreateTournament);
        }

        public void CreatePrize()
        {
            ActiveAddPrizeView = IoC.Get<CreatePrizeViewModel>();

            Items.Add(ActiveAddPrizeView);

            SelectedPrizesIsVisible = false;
            AddPrizeIsVisible = true;
        }

        public bool CanRemovePrize
        {
            get
            {
                return SelectedPrizeToRemove != null;
            }
        }

        public void RemovePrize()
        {
            SelectedPrizes.Remove(SelectedPrizeToRemove);
        }

        public bool CanCreateTournament
        {
            get
            {
                if (SelectedTeams != null)
                {
                    if (TournamentName.Length > 0 && SelectedTeams.Count > 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    } 
                }
                else
                {
                    return false;
                }
            }
        }

        public void CreateTournament()
        {
            var tm = new TournamentModel
            {
                TournamentName = TournamentName,
                EntryFee = EntryFee,
                Prizes = SelectedPrizes.ToList(),
                EnteredTeams = SelectedTeams.ToList()
            };

            TournamentLogic.CreateRounds(tm);

            GlobalConfig.Connection.CreateTournament(tm);

            tm.AlertUsersToNewRound();

            _eventAggregator.PublishOnUIThread(tm);

            TryClose();
        }

        public void Handle(TeamModel message)
        {
            if (!String.IsNullOrWhiteSpace(message.TeamName))
            {
                SelectedTeams.Add(message);
                NotifyOfPropertyChange(() => CanCreateTournament);
            }

            SelectedTeamsIsVisible = true;
            AddTeamIsVisible = false;
        }

        public void Handle(PrizeModel message)
        {
            if (!String.IsNullOrWhiteSpace(message.PlaceName))
            {
                SelectedPrizes.Add(message);
            }

            SelectedPrizesIsVisible = true;
            AddPrizeIsVisible = false;
        }
    }
}
