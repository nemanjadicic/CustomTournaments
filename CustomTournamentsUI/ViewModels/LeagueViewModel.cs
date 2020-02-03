using Caliburn.Micro;
using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Interfaces;
using CustomTournamentsLibrary.Models;
using System.Linq;

namespace CustomTournamentsUI.ViewModels
{
    public class LeagueViewModel : Screen, IEnterResult
    {
        //          BACKING FIELDS
        private string _tournamentName;
        private BindableCollection<RoundModel> _roundList;
        private RoundModel _selectedRound;
        private bool _unplayedOnly = false;
        private BindableCollection<GameModel> _gameList;
        private GameModel _selectedGame;
        private bool _canEnterResult = true;
        private BindableCollection<LeagueParticipantModel> _leagueParticipants;





        //          TOURNAMENT PROPERTIES
        public TournamentModel CurrentTournament { get; set; }
        public string TournamentName
        {
            get { return _tournamentName; }
            set
            {
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
            }
        }





        //          ROUNDS AND GAMES
        public BindableCollection<RoundModel> RoundList
        {
            get { return _roundList; }
            set
            {
                _roundList = value;
                NotifyOfPropertyChange(() => RoundList);
            }
        }
        public RoundModel SelectedRound
        {
            get { return _selectedRound; }
            set
            {
                _selectedRound = value;
                NotifyOfPropertyChange(() => SelectedRound);
            }
        }
        public void RoundListSelectionChanged()
        {
            if (UnplayedOnly)
            {
                GameList = new BindableCollection<GameModel>(SelectedRound.Games.Where(game => game.Unplayed == true));
            }
            else
            {
                GameList = new BindableCollection<GameModel>(SelectedRound.Games);
            }
        }
        public bool UnplayedOnly
        {
            get { return _unplayedOnly; }
            set
            {
                _unplayedOnly = value;
                NotifyOfPropertyChange(() => UnplayedOnly);
            }
        }
        public BindableCollection<GameModel> GameList
        {
            get { return _gameList; }
            set
            {
                _gameList = value;


                if (UnplayedOnly)
                {
                    _gameList = new BindableCollection<GameModel>(SelectedRound.Games.Where(game => game.Unplayed == true));
                }
                else
                {
                    SelectedRound.Games = value.ToList();
                }


                NotifyOfPropertyChange(() => GameList);
            }
        }
        public GameModel SelectedGame
        {
            get { return _selectedGame; }
            set
            {
                _selectedGame = value;
                NotifyOfPropertyChange(() => SelectedGame);

                if (SelectedGame != null)
                {
                    if (SelectedGame.Unplayed)
                    {
                        CanEnterResult = true;
                    }
                    else
                    {
                        CanEnterResult = false;
                    }
                }
            }
        }





        //          ENTERING A RESULT
        public bool CanEnterResult
        {
            get { return _canEnterResult; }
            set
            {
                _canEnterResult = value;
                NotifyOfPropertyChange(() => CanEnterResult);
            }
        }
        public void EnterResult()
        {
            IWindowManager manager = new WindowManager();
            manager.ShowWindow(new EnterResultViewModel(this));
        }
        private void EnterAutomaticResults()
        {
            foreach (RoundModel round in _roundList)
            {
                foreach (GameModel game in round.Games)
                {
                    GameParticipantModel homeTeam = game.Competitors[0];
                    GameParticipantModel awayTeam = game.Competitors[1];

                    if (homeTeam.TeamName.Contains("Dummy"))
                    {
                        homeTeam.Score = 0;
                        awayTeam.Score = CurrentTournament.OfficialScore;

                        game.Unplayed = false;
                        UpdateGameAndGameParticipants(game);
                    }
                    else if (awayTeam.TeamName.Contains("Dummy"))
                    {
                        awayTeam.Score = 0;
                        homeTeam.Score = CurrentTournament.OfficialScore;

                        game.Unplayed = false;
                        UpdateGameAndGameParticipants(game);
                    }

                    if (SelectedGame == game)
                    {
                        CanEnterResult = false;
                    }
                }
            }
        }
        private void UpdateGameAndGameParticipants(GameModel game)
        {
            SqlDataHandler.UpdateGameScoreAndStatus(game);

            if (CurrentTournament.IsLeague)
            {
                SqlDataHandler.UpdateLeagueParticipants(CurrentTournament, game);
            }
            else
            {
                SqlDataHandler.UpdateGameParticipantAsCupRoundWinner(game);
            }
        }





        //          LEAGUE TABLE
        public BindableCollection<LeagueParticipantModel> LeagueParticipants
        {
            get { return _leagueParticipants; }
            set
            {
                _leagueParticipants = value;
                NotifyOfPropertyChange(() => LeagueParticipants);
            }
        }
        public void RefreshTable()
        {
            LeagueParticipants = new BindableCollection<LeagueParticipantModel>(SqlDataHandler.GetLeagueParticipantsForDisplay(CurrentTournament.Id));

            ScoreDifferenceAndPositionNumber();
        }
        private void ScoreDifferenceAndPositionNumber()
        {
            foreach (LeagueParticipantModel team in _leagueParticipants)
            {
                team.ScoreDifferential = team.Scored - team.Conceded;
            }

            for (int num = 0; num < _leagueParticipants.Count; num++)
            {
                _leagueParticipants[num].Id = num + 1;
            }
        }





        //          CONSTRUCTOR
        public LeagueViewModel(TournamentModel selectedTournament)
        {
            CurrentTournament = selectedTournament;

            _tournamentName = CurrentTournament.TournamentName;
            _roundList = new BindableCollection<RoundModel>(SqlDataHandler.GetRoundsByTournament(CurrentTournament.Id));

            EnterAutomaticResults();
            
            _selectedRound = _roundList[0];
            _gameList = new BindableCollection<GameModel>(SelectedRound.Games);
            _leagueParticipants = new BindableCollection<LeagueParticipantModel>(SqlDataHandler.GetLeagueParticipantsForDisplay(CurrentTournament.Id));

            ScoreDifferenceAndPositionNumber();
        }
    }
}
