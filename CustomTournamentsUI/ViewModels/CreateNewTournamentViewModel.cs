using Caliburn.Micro;
using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Interfaces;
using CustomTournamentsLibrary.Logic;
using CustomTournamentsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CustomTournamentsUI.ViewModels
{
    public class CreateNewTournamentViewModel : Screen, ITournamentCreator
    {
        //                  BACKING FIELDS
        private string _tournamentName;
        private bool _isLeague;
        private bool _isCup;
        private bool _canClickRadioButton;
        private bool _homeAndAway;
        private bool _canClickHomeAway;
        private int _victoryPoints;
        private int _drawPoints;
        private bool _canEnterPoints;
        private int _officialScore;
        private decimal _entryFee;
        private BindableCollection<PrizeModel> _tournamentPrizes;
        private PrizeModel _selectedPrize;
        private bool _canCreatePrize;

        private BindableCollection<TeamModel> _availableTeams;
        private TeamModel _selectedAvailableTeam;
        private BindableCollection<TeamModel> _tournamentTeams;
        private TeamModel _selectedTournamentTeam;

        private bool _canCreateTournament;
        private string _errorMessage;



        

        //          TOURNAMENT NAME AND FEATURES
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

                if (IsLeague)
                {
                    CanClickHomeAway = true;
                    CanEnterPoints = true;
                }
                else
                {
                    CanClickHomeAway = false;
                    CanEnterPoints = false;
                }

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

                if (IsCup)
                {
                    HomeAndAway = false;
                    CanClickHomeAway = false;

                    CanEnterPoints = false;
                }
                else
                {
                    CanClickHomeAway = true;
                    CanEnterPoints = true;
                }

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
        public bool HomeAndAway
        {
            get { return _homeAndAway; }
            set 
            { 
                _homeAndAway = value;
                NotifyOfPropertyChange(() => HomeAndAway);
            }
        }
        public bool CanClickHomeAway
        {
            get { return _canClickHomeAway; }
            set 
            { 
                _canClickHomeAway = value;
                NotifyOfPropertyChange(() => CanClickHomeAway);
            }
        }
        public int VictoryPoints
        {
            get { return _victoryPoints; }
            set 
            { 
                _victoryPoints = value;
                NotifyOfPropertyChange(() => VictoryPoints);

                ValidateData();
            }
        }
        public int DrawPoints
        {
            get { return _drawPoints; }
            set
            { 
                _drawPoints = value;
                NotifyOfPropertyChange(() => DrawPoints);

                ValidateData();
            }
        }
        public bool CanEnterPoints
        {
            get { return _canEnterPoints; }
            set 
            { 
                _canEnterPoints = value;
                NotifyOfPropertyChange(() => CanEnterPoints);
            }
        }
        public int OfficialScore
        {
            get { return _officialScore; }
            set 
            { 
                _officialScore = value;
                NotifyOfPropertyChange(() => OfficialScore);

                ValidateData();
            }
        }
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

            if (IsLeague == true && VictoryPoints <= 1)
            {
                errorList.Add("A team must be awarded at least 2 points for a win.");
            }

            if (IsLeague == true && DrawPoints < 1)
            {
                errorList.Add("Teams must be awarded at least 1 point for a draw.");
            }

            if (IsLeague == true && DrawPoints >= VictoryPoints)
            {
                errorList.Add("Teams must be awarded less points for a draw than a win.");
            }

            if (OfficialScore <= 0)
            {
                errorList.Add("A (real) team must score more than a generated Dummy Team.");
            }

            bool somethingWrong = (String.IsNullOrWhiteSpace(TournamentName)) || (TournamentTeams.Count <= 1) || (IsLeague == false && IsCup == false) 
                || (IsLeague == true && VictoryPoints <= 1) || (IsLeague == true && DrawPoints < 1 || IsLeague == true && DrawPoints >= VictoryPoints) || (OfficialScore <= 0);

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
            TournamentModel tournament = new TournamentModel
                (TournamentName, IsLeague, HomeAndAway, VictoryPoints, DrawPoints, OfficialScore, EntryFee);
            SqlDataHandler.CreateTournament(tournament);

            foreach (TeamModel team in TournamentTeams)
            {
                tournament.ParticipatingTeams.Add(team);
            }

            RoundLogic.CreateDummyTeams(tournament);

            if (tournament.IsLeague)
            {
                foreach (TeamModel team in tournament.ParticipatingTeams)
                {
                    SqlDataHandler.CreateLeagueParticipant(tournament, team);
                }
            }

            foreach (PrizeModel prize in TournamentPrizes)
            {
                tournament.TournamentPrizes.Add(prize);
                SqlDataHandler.CreatePrize(tournament, prize);
            }

            RoundLogic.CreateRounds(tournament);

            var conductor = Parent as IConductor;
            conductor.ActivateItem(new HomeViewModel());
            //  TODO - activate tournamentView instead of homeView
        }





        //          CONSTRUCTOR
        public CreateNewTournamentViewModel()
        {
            _tournamentTeams = new BindableCollection<TeamModel>();
            _tournamentPrizes = new BindableCollection<PrizeModel>();
            _availableTeams = new BindableCollection<TeamModel>(SqlDataHandler.GetAllTeams());
            TournamentTeams.CollectionChanged += TournamentTeams_CollectionChanged;
            _victoryPoints = 3;
            _drawPoints = 1;
            _officialScore = 3;
        }
    }
}
