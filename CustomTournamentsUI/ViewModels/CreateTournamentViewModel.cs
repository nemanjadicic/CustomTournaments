using Caliburn.Micro;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using CustomTournamentsLibrary.Models;
using CustomTournamentsLibrary.Interfaces;
using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Logic;

namespace CustomTournamentsUI.ViewModels
{
    public class CreateTournamentViewModel : Screen, ITournamentCreator
    {
        //                  BACKING FIELDS
        private string _tournamentName;
        private bool _isLeague;
        private bool _isCup;
        private bool _canClickRadioButton;
        
        private BindableCollection<TeamModel> _availableTeams;
        private TeamModel _selectedAvailableTeam;
        private BindableCollection<TeamModel> _tournamentTeams;
        private TeamModel _selectedTournamentTeam;

        private decimal _entryFee;
        private BindableCollection<PrizeModel> _tournamentPrizes;
        private PrizeModel _selectedPrize;
        private bool _canCreatePrize;

        private bool _canCreateTournament;
        private string _errorMessage;





        //          TOURNAMENT NAME AND TYPE
        public string TournamentName
        {
            get { return _tournamentName; }
            set 
            { 
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);

                ValidateData();
            }
        }
        public bool IsLeague
        {
            get { return _isLeague; }
            set
            {
                _isLeague = value;
                NotifyOfPropertyChange(() => IsLeague);

                ValidateData();
            }
        }
        public bool IsCup
        {
            get { return _isCup; }
            set
            {
                _isCup = value;
                NotifyOfPropertyChange(() => IsCup);

                ValidateData();
            }
        }
        public bool CanClickRadioButton
        {
            get { return _canClickRadioButton; }
            set 
            { 
                _canClickRadioButton = value;
                NotifyOfPropertyChange(() => CanClickRadioButton);
            }
        }





        //          TEAMS MANAGEMENT
        public BindableCollection<TeamModel> AvailableTeams
        {
            get { return _availableTeams; }
            set
            {
                _availableTeams = value;
                NotifyOfPropertyChange(() => AvailableTeams);
            }
        }
        public TeamModel SelectedAvailableTeam
        {
            get { return _selectedAvailableTeam; }
            set 
            { 
                _selectedAvailableTeam = value;
                NotifyOfPropertyChange(() => SelectedAvailableTeam);
            }
        }
        public BindableCollection<TeamModel> TournamentTeams
        {
            get { return _tournamentTeams; }
            set 
            {
                _tournamentTeams = value;
                NotifyOfPropertyChange(() => TournamentTeams);

                ValidateData();
            }
        }
        private void TournamentTeams_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (TournamentTeams.Count < 2)
            {
                CanClickRadioButton = false;
            }
            else
            {
                CanClickRadioButton = true;
            }
        }
        public TeamModel SelectedTournamentTeam
        {
            get { return _selectedTournamentTeam; }
            set 
            { 
                _selectedTournamentTeam = value;
                NotifyOfPropertyChange(() => SelectedTournamentTeam);
            }
        }
        public void CreateNewTeam()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(new CreateTeamViewModel(this));
        }
        public void AddTeamToTournament()
        {
            if (SelectedAvailableTeam != null)
            {
                TournamentTeams.Add(SelectedAvailableTeam);
                AvailableTeams.Remove(SelectedAvailableTeam);
            }

            ValidateData();
        }
        public void RemoveTeamFromTournament()
        {
            if (SelectedTournamentTeam != null)
            {
                AvailableTeams.Add(SelectedTournamentTeam);
                TournamentTeams.Remove(SelectedTournamentTeam);
            }

            ValidateData();
        }





        //          ENTRY FEE / TOURNAMENT PRIZES MANAGEMENT
        public decimal EntryFee
        {
            get { return _entryFee; }
            set 
            { 
                _entryFee = value;
                NotifyOfPropertyChange(() => EntryFee);

                if (EntryFee <= 0)
                {
                    CanCreatePrize = false;
                }
                else
                {
                    CanCreatePrize = true;
                }
            }
        }
        public BindableCollection<PrizeModel> TournamentPrizes
        {
            get { return _tournamentPrizes; }
            set 
            {
                _tournamentPrizes = value;
                NotifyOfPropertyChange(() => TournamentPrizes);
            }
        }
        public PrizeModel SelectedPrize
        {
            get { return _selectedPrize; }
            set 
            { 
                _selectedPrize = value;
                NotifyOfPropertyChange(() => SelectedPrize);
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
        public void CreatePrize()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(new CreatePrizeViewModel(this));
        }




        
        //          TOURNAMENT CREATION
        public bool CanCreateTournament
        {
            get { return _canCreateTournament; }
            set 
            { 
                _canCreateTournament = value;
                NotifyOfPropertyChange(() => CanCreateTournament);
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
            List<string> errorList = new List<string>();
            
            if (String.IsNullOrWhiteSpace(TournamentName))
            {
                errorList.Add("Please name your tournament.");
            }

            if (TournamentTeams.Count < 2)
            {
                errorList.Add("Tournament must have at least 2 teams.");
            }

            if (IsLeague == false && IsCup == false)
            {
                errorList.Add("Please select a tournament type.");
            }

            bool somethingWrong = String.IsNullOrWhiteSpace(TournamentName) || TournamentTeams.Count <= 1 || IsLeague == false && IsCup == false;

            if (somethingWrong)
            {
                CanCreateTournament = false;
                ErrorMessage = $"* {String.Join(" ", errorList)}";
            }
            else
            {
                CanCreateTournament = true;
                ErrorMessage = null;
            }
        }
        public void CreateTournament()
        {
            //TournamentModel tournament = new TournamentModel(TournamentName, IsLeague, EntryFee);
            //SqlDataHandler.CreateTournament(tournament);

            //foreach (TeamModel team in TournamentTeams)
            //{
            //    tournament.ParticipatingTeams.Add(team);
            //}

            //RoundLogic.CreateDummyTeams(tournament);

            //if (tournament.IsLeague)
            //{
            //    foreach (TeamModel team in tournament.ParticipatingTeams)
            //    {
            //        SqlDataHandler.CreateLeagueParticipant(tournament, team);
            //    }
            //}

            //foreach (PrizeModel prize in TournamentPrizes)
            //{
            //    tournament.TournamentPrizes.Add(prize);
            //    SqlDataHandler.CreatePrize(tournament, prize);
            //}

            //RoundLogic.CreateRounds(tournament);

            //var conductor = Parent as IConductor;
            //conductor.ActivateItem(new HomeViewModel());
        }



        
        
        //          CONSTRUCTOR
        public CreateTournamentViewModel()
        {
            _tournamentTeams = new BindableCollection<TeamModel>();
            _tournamentPrizes = new BindableCollection<PrizeModel>();
            _availableTeams = new BindableCollection<TeamModel>(SqlDataHandler.GetAllTeams());
            TournamentTeams.CollectionChanged += TournamentTeams_CollectionChanged;
        }
    }
}
