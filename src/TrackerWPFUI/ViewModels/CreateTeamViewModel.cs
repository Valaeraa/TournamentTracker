﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;
using Stylet;

namespace TrackerWPFUI.ViewModels
{
    public class CreateTeamViewModel : Conductor<object>, IHandle<PersonModel>
    {
        private string _teamName = "";
        private bool _selectedTeamMembersIsVisible = true;
        private bool _addPersonIsVisible = false;
        private BindableCollection<PersonModel> _availibleTeamMembers;
        private PersonModel _selectedTeamMemberToAdd;
        private BindableCollection<PersonModel> _selectedTeamMembers = new BindableCollection<PersonModel>();
        private PersonModel _selectedTeamMemberToRemove;
        private readonly ILogger<CreateTeamViewModel> _logger;
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceProvider _service;

        public CreateTeamViewModel(ILogger<CreateTeamViewModel> logger, IEventAggregator eventAggregator, IServiceProvider service)
        {
            AvailibleTeamMembers = new BindableCollection<PersonModel>(GlobalConfig.Connection.GetPerson_All());
            //EventAggregationProvider.TrackerEventAggregator.SubscribeOnPublishedThread(this);
            _logger = logger;
            _eventAggregator = eventAggregator;
            _service = service;

            _eventAggregator.Subscribe(this);
        }

        public string TeamName
        {
            get { return _teamName; }
            set
            {
                _teamName = value;
                NotifyOfPropertyChange(() => TeamName);
                NotifyOfPropertyChange(() => CanCreateTeam);
            }
        }

        public bool SelectedTeamMembersIsVisible
        {
            get { return _selectedTeamMembersIsVisible; }
            set
            {
                _selectedTeamMembersIsVisible = value;
                NotifyOfPropertyChange(() => SelectedTeamMembersIsVisible);
            }
        }
        
        public bool AddPersonIsVisible
        {
            get { return _addPersonIsVisible; }
            set
            {
                _addPersonIsVisible = value;
                NotifyOfPropertyChange(() => AddPersonIsVisible);
            }
        }

        public BindableCollection<PersonModel> AvailibleTeamMembers
        {
            get { return _availibleTeamMembers; }
            set { _availibleTeamMembers = value; }
        }
        
        public PersonModel SelectedTeamMemberToAdd
        {
            get { return _selectedTeamMemberToAdd; }
            set
            {
                _selectedTeamMemberToAdd = value;
                NotifyOfPropertyChange(() => SelectedTeamMemberToAdd);
                NotifyOfPropertyChange(() => CanAddMember);
            }
        }
        
        public BindableCollection<PersonModel> SelectedTeamMembers
        {
            get { return _selectedTeamMembers; }
            set
            {
                _selectedTeamMembers = value;
                NotifyOfPropertyChange(() => CanCreateTeam);
            }
        }
        
        public PersonModel SelectedTeamMemberToRemove
        {
            get { return _selectedTeamMemberToRemove; }
            set
            {
                _selectedTeamMemberToRemove = value;
                NotifyOfPropertyChange(() => SelectedTeamMemberToRemove);
                NotifyOfPropertyChange(() => CanRemoveMember);
            }
        }

        public bool CanAddMember
        {
            get
            {
                return SelectedTeamMemberToAdd != null;
            }
        }

        public void AddMember()
        {
            SelectedTeamMembers.Add(SelectedTeamMemberToAdd);
            AvailibleTeamMembers.Remove(SelectedTeamMemberToAdd);

            NotifyOfPropertyChange(() => CanCreateTeam);
        }

        public void CreateMember()
        {
            var vm = _service.GetService<CreatePersonViewModel>();
            ActivateItem(vm /*new CreatePersonViewModel()*/);

            SelectedTeamMembersIsVisible = false;
            AddPersonIsVisible = true;
        }

        public bool CanRemoveMember
        {
            get
            {
                return SelectedTeamMemberToRemove != null;
            }
        }

        public void RemoveMember()
        {
            AvailibleTeamMembers.Add(SelectedTeamMemberToRemove);
            SelectedTeamMembers.Remove(SelectedTeamMemberToRemove);

            NotifyOfPropertyChange(() => CanCreateTeam);
        }

        public void CancelCreation()
        {
            //await EventAggregationProvider .TrackerEventAggregator.PublishOnUIThreadAsync(new TeamModel());
            _eventAggregator.PublishOnUIThread(new TeamModel());

            RequestClose();
        }
        
        public bool CanCreateTeam
        {
            get
            {
                if (SelectedTeamMembers != null)
                {
                    if (TeamName.Length > 0 && SelectedTeamMembers.Count > 0)
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

        public void CreateTeam()
        {
            TeamModel t = new TeamModel
            {
                TeamName = TeamName,
                TeamMembers = SelectedTeamMembers.ToList()
            };

            GlobalConfig.Connection.CreateTeam(t);

            //await EventAggregationProvider.TrackerEventAggregator.PublishOnUIThreadAsync(t);
            _eventAggregator.PublishOnUIThread(t);

            RequestClose();
        }

        public void Handle(PersonModel message)
        {
            if (!string.IsNullOrWhiteSpace(message.FullName))
            {
                SelectedTeamMembers.Add(message);
                NotifyOfPropertyChange(() => CanCreateTeam);
            }

            SelectedTeamMembersIsVisible = true;
            AddPersonIsVisible = false;
        }
    }
}
