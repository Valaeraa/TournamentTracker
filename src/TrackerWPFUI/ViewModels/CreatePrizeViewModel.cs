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
    public class CreatePrizeViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        public CreatePrizeViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        private int _placeNumber;

        public int PlaceNumber
        {
            get { return _placeNumber; }
            set
            {
                _placeNumber = value;
                NotifyOfPropertyChange(() => PlaceNumber);
            }
        }

        private string _placeName;

        public string PlaceName
        {
            get { return _placeName; }
            set
            {
                _placeName = value;
                NotifyOfPropertyChange(() => PlaceName);
            }
        }

        private decimal _prizeAmount;

        public decimal PrizeAmount
        {
            get { return _prizeAmount; }
            set
            {
                _prizeAmount = value;
                NotifyOfPropertyChange(() => PrizeAmount);
            }
        }

        private double _prizePercentage;

        public double PrizePercentage
        {
            get { return _prizePercentage; }
            set
            {
                _prizePercentage = value;
                NotifyOfPropertyChange(() => PrizePercentage);
            }
        }

        public void CancelCreation()
        {
            _eventAggregator.PublishOnUIThread(new PrizeModel());
            //EventAggregationProvider.TrackerEventAggregator.PublishOnUIThread(new PrizeModel());

            TryClose();
        }

        public bool CanCreatePrize(int placeNumber, string placeName, decimal prizeAmount, double prizePercentage)
        {
            return ValidateForm(placeNumber, placeName, prizeAmount, prizePercentage);
        }

        public void CreatePrize(int placeNumber, string placeName, decimal prizeAmount, double prizePercentage)
        {
            PrizeModel model = new PrizeModel
            {
                PlaceNumber = placeNumber,
                PlaceName = placeName,
                PrizeAmount = prizeAmount,
                PrizePercentage = prizePercentage
            };

            GlobalConfig.Connection.CreatePrize(model);

            _eventAggregator.PublishOnUIThread(model);
            //EventAggregationProvider.TrackerEventAggregator.PublishOnUIThread(model);

            TryClose();
        }

        private bool ValidateForm(int placeNumber, string placeName, decimal prizeAmount, double prizePercentage)
        {
            bool output = true;

            if (placeNumber < 1)
            {
                output = false;
            }

            if (placeName.Length == 0)
            {
                output = false;
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                output = false;
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                output = false;
            }

            return output;
        }
    }
}
