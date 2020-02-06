using Caliburn.Micro;
using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Interfaces;
using CustomTournamentsLibrary.Logic;
using CustomTournamentsLibrary.Models;
using System;
using System.Collections.Generic;

namespace CustomTournamentsUI.ViewModels
{
    public class EnterResultViewModel : Screen
    {
        //          BACKING FIELDS
        IEnterResult tournamentView;

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

            if (!tournamentView.CurrentTournament.IsLeague && homeScoreValue == awayScoreValue)
            {
                errors.Add("One of two teams must be a winner.");
            }

            bool somethingWrong = (!homeScoreValid || !awayScoreValid) 
                || (homeScoreValue < 0 || awayScoreValue < 0) 
                || (!tournamentView.CurrentTournament.IsLeague && homeScoreValue == awayScoreValue);

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

            tournamentView.SelectedGame.Unplayed = false;

            SqlDataHandler.UpdateGameScoreAndStatus(tournamentView.SelectedGame);

            if (tournamentView.CurrentTournament.IsLeague)
            {
                SqlDataHandler.UpdateLeagueParticipants(tournamentView.CurrentTournament, tournamentView.SelectedGame); 
            }
            else
            {
                SqlDataHandler.UpdateGameParticipantAsCupRoundWinner(tournamentView.SelectedGame);
                
                if (RoundComplete())
                {
                    int currentRoundIndex = tournamentView.SelectedRound.RoundNumber - 1;

                    List<TeamModel> nextRoundParticipants = SqlDataHandler.GetRoundWinners(tournamentView.SelectedRound.Id);
                    
                    if (tournamentView.SelectedRound.RoundNumber < tournamentView.RoundList.Count)
                    {
                        RoundModel nextRound = tournamentView.CurrentTournament.Rounds[currentRoundIndex + 1];

                        RoundLogic.CreateCupRoundGames(nextRoundParticipants, nextRound);
                    }
                }
            }

            if (TournamentComplete())
            {
                tournamentView.CurrentTournament.Finished = true;
                SqlDataHandler.UpdateTournamentStatus(tournamentView.CurrentTournament);
            }

            tournamentView.CanEnterResult = false;
            tournamentView.GameList.Refresh();

            TryClose();
        }
        private bool RoundComplete()
        {
            List<GameModel> roundGames = tournamentView.SelectedRound.Games;
            List<bool> gameStatuses = new List<bool>();

            foreach (GameModel game in roundGames)
            {
                gameStatuses.Add(game.Unplayed);
            }

            if (gameStatuses.Contains(true))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool TournamentComplete()
        {
            if (tournamentView.CurrentTournament.IsLeague)
            {
                List<GameModel> allTournamentGames = new List<GameModel>();
                List<bool> gameStatuses = new List<bool>();

                foreach (RoundModel round in tournamentView.RoundList)
                {
                    allTournamentGames.AddRange(round.Games);
                }

                foreach (GameModel game in allTournamentGames)
                {
                    gameStatuses.Add(game.Unplayed);
                }

                if (gameStatuses.Contains(true))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                int lastRoundIndex = tournamentView.RoundList.Count - 1;
                
                if (tournamentView.RoundList[lastRoundIndex].Games.Count < 1 || tournamentView.RoundList[lastRoundIndex].Games[0].Unplayed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }





        //          CONSTRUCTOR
        public EnterResultViewModel(IEnterResult previousView)
        {
            tournamentView = previousView;

            HomeTeam = tournamentView.SelectedGame.Competitors[0];
            AwayTeam = tournamentView.SelectedGame.Competitors[1];
        }
    }
}
