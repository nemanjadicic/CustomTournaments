using Caliburn.Micro;
using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsUI.ViewModels
{
    public class TournamentSummaryViewModel : Screen
    {
        //          BACKING FIELDS
        private TournamentModel finishedTournament;

        private string _tournamentName;
        private string _trophyTitle;

        private TeamModel _champion;
        private TeamModel _runnerUp;
        private List<TeamModel> _thirdPlaced = new List<TeamModel>();
        private string _championAndPrize = "";
        private string _runnerUpAndPrize = "";
        private string _thirdPlacedAndPrize = "";
        private GameModel _highestScoringGame;

        private List<TeamModel> _participatingTeams;
        private List<PlayerModel> _teamMembers;
        private TeamModel _selectedTeam;




        //          HEADER AND THROPHY PROPERTIES
        public string TournamentName
        {
            get { return _tournamentName; }
            set 
            { 
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
            }
        }
        public string TrophyTitle
        {
            get { return _trophyTitle; }
            set 
            { 
                _trophyTitle = value;
                NotifyOfPropertyChange(() => TrophyTitle);
            }
        }





        //          TOURNAMENT AWARDS
        public TeamModel Champion
        {
            get { return _champion; }
            set 
            { 
                _champion = value;
                NotifyOfPropertyChange(() => Champion);
            }
        }
        public TeamModel RunnerUp
        {
            get { return _runnerUp; }
            set 
            {
                _runnerUp = value;
                NotifyOfPropertyChange(() => RunnerUp);
            }
        }
        public List<TeamModel> ThirdPlaced
        {
            get { return _thirdPlaced; }
            set 
            {
                _thirdPlaced = value;
                NotifyOfPropertyChange(() => ThirdPlaced);
            }
        }
        public string ChampionAndPrize
        {
            get { return _championAndPrize; }
            set 
            { 
                _championAndPrize = value;
                NotifyOfPropertyChange(() => ChampionAndPrize);
            }
        }
        public string RunnerUpAndPrize
        {
            get { return _runnerUpAndPrize; }
            set 
            { 
                _runnerUpAndPrize = value;
                NotifyOfPropertyChange(() => RunnerUpAndPrize);
            }
        }
        public string ThirdPlacedAndPrize
        {
            get { return _thirdPlacedAndPrize; }
            set 
            { 
                _thirdPlacedAndPrize = value;
                NotifyOfPropertyChange(() => ThirdPlacedAndPrize);
            }
        }
        public GameModel HighestScoringGame
        {
            get { return _highestScoringGame; }
            set 
            { 
                _highestScoringGame = value;
                NotifyOfPropertyChange(() => HighestScoringGame);
            }
        }



        

        //          TOURNAMENT TEAMS AND PLAYERS
        public List<TeamModel> ParticipatingTeams
        {
            get { return _participatingTeams; }
            set 
            { 
                _participatingTeams = value;
                NotifyOfPropertyChange(() => ParticipatingTeams);
            }
        }
        public TeamModel SelectedTeam
        {
            get { return _selectedTeam; }
            set 
            { 
                _selectedTeam = value;
                NotifyOfPropertyChange(() => SelectedTeam);
            }
        }
        public void ParticipatingTeamsSelectionChanged()
        {
            TeamMembers = SelectedTeam.TeamMembers;
        }
        public List<PlayerModel> TeamMembers
        {
            get { return _teamMembers; }
            set 
            {
                _teamMembers = value;
                NotifyOfPropertyChange(() => TeamMembers);
            }
        }
        
        
        
        
        
        //          CONSTRUCTOR AND HELPER METHODS
        public void OpenTournamentHistory()
        {
            var conductor = Parent as IConductor;

            if (finishedTournament.IsLeague)
            {
                conductor.ActivateItem(new LeagueViewModel(finishedTournament));
            }
            else
            {
                conductor.ActivateItem(new CupViewModel(finishedTournament));
            }
        }
        
        public TournamentSummaryViewModel(TournamentModel selectedTournament)
        {
            finishedTournament = selectedTournament;
            finishedTournament.Rounds = SqlDataHandler.GetRoundsByTournament(finishedTournament.Id);
            finishedTournament.TournamentPrizes = SqlDataHandler.GetPrizesByTournament(finishedTournament.Id);



            _tournamentName = finishedTournament.TournamentName;
            _trophyTitle = $"{DateTime.Now.Year} Winner";



            WireUpTournamentOrder();



            WireUpTournamentAwards();



            WireUpHighestScoringGame();



            _participatingTeams = SqlDataHandler.GetTeamsByTournament(finishedTournament);
            _selectedTeam = _participatingTeams[0];
            _teamMembers = _selectedTeam.TeamMembers;
        }

        private void WireUpHighestScoringGame()
        {
            List<GameModel> allGames = new List<GameModel>();

            foreach (RoundModel round in finishedTournament.Rounds)
            {
                allGames.AddRange(round.Games);
            }

            HighestScoringGame = allGames.OrderByDescending(game => game.ScoreCount).First();
        }

        private void WireUpTournamentOrder()
        {
            if (finishedTournament.IsLeague)
            {
                List<LeagueParticipantModel> leagueParticipants = SqlDataHandler.GetLeagueParticipantsForDisplay(finishedTournament.Id);
                _champion = new TeamModel(leagueParticipants.First().TeamName);
                _runnerUp = new TeamModel(leagueParticipants[1].TeamName);
                _thirdPlaced.Add(new TeamModel(leagueParticipants[2].TeamName));
            }
            else
            {
                RoundModel finalRound = finishedTournament.Rounds.Last();
                List<TeamModel> roundWinners = SqlDataHandler.GetRoundWinners(finalRound.Id);

                _champion = roundWinners.First();
                _runnerUp = new TeamModel(finalRound.Games[0].Competitors.Find(team => team.CupRoundWinner == false).TeamName);



                RoundModel secondToLastRound = finishedTournament.Rounds[finalRound.RoundNumber - 2];
                List<GameModel> semiFinalGames = SqlDataHandler.GetGamesByRound(secondToLastRound);
                List<GameParticipantModel> semiFinalLosers = new List<GameParticipantModel>();

                foreach (GameModel game in semiFinalGames)
                {
                    semiFinalLosers.Add(game.Competitors.Find(team => team.CupRoundWinner == false));
                }

                _thirdPlaced.Add(new TeamModel(semiFinalLosers[0].TeamName));
                _thirdPlaced.Add(new TeamModel(semiFinalLosers[1].TeamName));
            }
        }

        private void WireUpTournamentAwards()
        {
            PrizeModel winnerPrize = finishedTournament.TournamentPrizes.Find(prize => prize.PlaceNumber == 1);
            if (winnerPrize != null)
            {
                _championAndPrize =
                    $"{_champion.TeamName}, won {winnerPrize.PrizeAmount.ToString("0.0")} of prize money.";
            }
            else
            {
                _championAndPrize =
                    $"{_champion.TeamName}, won 0.0 of prize money.";
            }

            PrizeModel runnerupPrize = finishedTournament.TournamentPrizes.Find(prize => prize.PlaceNumber == 2);
            if (runnerupPrize != null)
            {
                _runnerUpAndPrize =
                            $"{_runnerUp.TeamName}, won {runnerupPrize.PrizeAmount.ToString("0.0")} of prize money.";
            }
            else
            {
                _runnerUpAndPrize =
                            $"{_runnerUp.TeamName}, won 0.0 of prize money.";
            }

            List<string> thirdPlacedNames = new List<string>();

            foreach (TeamModel team in _thirdPlaced)
            {
                thirdPlacedNames.Add(team.TeamName);
            }

            PrizeModel thirdPlacePrize = finishedTournament.TournamentPrizes.Find(prize => prize.PlaceNumber == 3);
            if (thirdPlacePrize != null)
            {
                _thirdPlacedAndPrize =
                            $"{String.Join(", ", thirdPlacedNames)} won {finishedTournament.TournamentPrizes.Find(prize => prize.PlaceNumber == 3).PrizeAmount.ToString("0.0")} of prize money.";
            }
            else
            {
                _thirdPlacedAndPrize = $"{String.Join(", ", thirdPlacedNames)} won 0.0 of prize money.";
            }
        }
    }
}
