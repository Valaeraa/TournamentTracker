using Microsoft.Extensions.Logging;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerWPFUI.ViewModels
{
    public class CreatePersonViewModel : Screen
    {
        private readonly ILogger<CreatePersonViewModel> _logger;
        private readonly IEventAggregator _eventAggregator;
        private string _firstName = "";
        private string _lastName = "";
        private string _email = "";
        private string _cellphone = "";
        //private object emailValue;

        public CreatePersonViewModel(ILogger<CreatePersonViewModel> logger, IEventAggregator eventAggregator)
        {
            _logger = logger;
            _eventAggregator = eventAggregator;
            //_eventAggregator.Subscribe(this);
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
            }
        }


        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
            }
        }


        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                NotifyOfPropertyChange(() => Email);
            }
        }


        public string Cellphone
        {
            get { return _cellphone; }
            set
            {
                _cellphone = value;
                NotifyOfPropertyChange(() => Cellphone);
            }
        }

        public void CancelCreation()
        {
            //EventAggregationProvider.TrackerEventAggregator.PublishOnUIThreadAsync(new PersonModel());
            _eventAggregator.PublishOnUIThread(new PersonModel());

            RequestClose();
        }

        public bool CanCreatePerson(string firstName, string lastName, string email, string cellphone)
        {
            if (firstName.Length > 0 && lastName.Length > 0 && email.Length > 0 && cellphone.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreatePerson(string firstName, string lastName, string email, string cellphone)
        {
            PersonModel p = new PersonModel
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                CellphoneNumber = cellphone
            };

            GlobalConfig.Connection.CreatePerson(p);
            
            //EventAggregationProvider.TrackerEventAggregator.PublishOnUIThreadAsync(p);
            _eventAggregator.PublishOnUIThread(p);

            RequestClose();
        }
    }
}
