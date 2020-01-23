using Caliburn.Micro;
using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Interfaces;
using CustomTournamentsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsUI.ViewModels
{
    public class CreatePrizeViewModel : Screen
    {
        //                  BACKING FIELDS
        ITournamentCreator _tournamentCreationView;

        private string _placeNumber;
        private string _placeName;
        private string _prizeAmount;
        private bool _canCreatePrize;
        private string _errorMessage;





        //              PROPERTIES AND METHODS
        public string PlaceNumber
        {
            get { return _placeNumber; }
            set 
            { 
                _placeNumber = value;
                NotifyOfPropertyChange(() => PlaceNumber);
                ValidateData();
            }
        }
        public string PlaceName
        {
            get { return _placeName; }
            set 
            { 
                _placeName = value;
                NotifyOfPropertyChange(() => PlaceName);
            }
        }
        public string PrizeAmount
        {
            get { return _prizeAmount; }
            set 
            { 
                _prizeAmount = value;
                NotifyOfPropertyChange(() => PrizeAmount);
                ValidateData();
            }
        }
        public bool CanCreatePrize
        {
            get { return _canCreatePrize; }
            set 
            { 
                _canCreatePrize = value;
                NotifyOfPropertyChange(() => CanCreatePrize);
            }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }
        private void ValidateData()
        {
            bool placeNumberValid = int.TryParse(PlaceNumber, out int placeNumberValue);
            bool prizeAmountValid = decimal.TryParse(PrizeAmount, out decimal prizeAmountValue);
            List<string> errors = new List<string>();

            if (!String.IsNullOrWhiteSpace(PlaceNumber) && !placeNumberValid)
            {
                errors.Add("Place Number must be a number.");
            }

            if (!String.IsNullOrWhiteSpace(PrizeAmount) && !prizeAmountValid)
            {
                errors.Add("Prize Amount must be a number.");
            }

            if (!String.IsNullOrWhiteSpace(PlaceNumber) && placeNumberValue < 1)
            {
                errors.Add("Place Number must be greater than 0.");
            }

            if (!String.IsNullOrWhiteSpace(PrizeAmount) && prizeAmountValue < 0)
            {
                errors.Add("Prize Amount cannot be less than 0.");
            }

            bool somethingWrong = (!String.IsNullOrWhiteSpace(PlaceNumber) && (!placeNumberValid || placeNumberValue < 1)) ||
                (!String.IsNullOrWhiteSpace(PrizeAmount) && (!prizeAmountValid || prizeAmountValue < 0));

            if (somethingWrong)
            {
                CanCreatePrize = false;
                ErrorMessage = $"* {String.Join(" ", errors)}";
            }
            else if (String.IsNullOrWhiteSpace(PlaceNumber) || String.IsNullOrWhiteSpace(PrizeAmount))
            {
                CanCreatePrize = false;
                ErrorMessage = null;
            }
            else
            {
                CanCreatePrize = true;
                ErrorMessage = null;
            }
        }
        public void CreatePrize()
        {
            PrizeModel prize = new PrizeModel(int.Parse(PlaceNumber), PlaceName, decimal.Parse(PrizeAmount));
            _tournamentCreationView.TournamentPrizes.Add(prize);

            var conductor = Parent as IConductor;
            conductor.ActivateItem(_tournamentCreationView);
        }
        public void GoBack()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(_tournamentCreationView);
        }





        public CreatePrizeViewModel(ITournamentCreator previousView)
        {
            _tournamentCreationView = previousView;
        }
    }
}
