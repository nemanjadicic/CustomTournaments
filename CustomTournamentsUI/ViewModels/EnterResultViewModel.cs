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
    public class EnterResultViewModel : Screen
    {
        //          BACKING FIELDS
        IEnterResult _leagueView;
        private string _homeScore;
        private string _awayScore;
        private bool _canEnterResult;
        private string _errorMessage;





        //          TEAMS
        public GameParticipantModel HomeTeam { get; set; }
        public GameParticipantModel AwayTeam { get; set; }
        public string HomeScore
        {
            get { return _homeScore; }
            set 
            { 
                _homeScore = value;
                NotifyOfPropertyChange(() => HomeScore);
                ValidateEntries();
            }
        }
        public string AwayScore
        {
            get { return _awayScore; }
            set 
            { 
                _awayScore = value;
                NotifyOfPropertyChange(() => AwayScore);
                ValidateEntries();
            }
        }





        //          ENTERING SCORES
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set 
            { 
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }
        public bool CanEnterResult
        {
            get { return _canEnterResult; }
            set 
            {
                _canEnterResult = value;
                NotifyOfPropertyChange(() => CanEnterResult);
            }
        }
        public void ValidateEntries()
        {
            List<string> errors = new List<string>();
            
            bool homeScoreValid = int.TryParse(HomeScore, out int homeScoreValue);
            bool awayScoreValid = int.TryParse(AwayScore, out int awayScoreValue);

            if (!homeScoreValid || !awayScoreValid)
            {
                errors.Add("Team's score must be a number.");
            }

            if (homeScoreValue < 0 || awayScoreValue < 0)
            {
                errors.Add("Team's score must not be lower than 0.");
            }

            bool somethingWrong = (!homeScoreValid || !awayScoreValid) || (homeScoreValue < 0 || awayScoreValue < 0);

            if (somethingWrong)
            {
                CanEnterResult = false;
                ErrorMessage = $"* {String.Join(" ", errors)}";
            }
            else
            {
                CanEnterResult = true;
                ErrorMessage = null;
            }
        }
        public void EnterResult()
        {
            HomeTeam.Score = int.Parse(HomeScore);
            AwayTeam.Score = int.Parse(AwayScore);

            _leagueView.SelectedGame.Unplayed = false;

            SqlDataHandler.UpdateGameScoreAndStatus(_leagueView.SelectedGame);
            SqlDataHandler.UpdateLeagueParticipants(_leagueView.SelectedGame);

            _leagueView.GameList.Refresh();
            _leagueView.LeagueParticipants.Refresh();
            
            TryClose();
        }





        public EnterResultViewModel(IEnterResult previousView)
        {
            _leagueView = previousView;

            HomeTeam = _leagueView.SelectedGame.Competitors[0];
            AwayTeam = _leagueView.SelectedGame.Competitors[1];
        }
    }
}
