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
        TournamentModel _currentTournament;
        private string _tournamentName;
        private BindableCollection<RoundModel> _roundList;
        private RoundModel _selectedRound;
        private bool _unplayedOnly = false;
        private BindableCollection<GameModel> _gameList;
        private GameModel _selectedGame;
        private bool _canEnterResult = true;
        private BindableCollection<LeagueParticipantModel> _leagueParticipants;





        //          ROUNDS AND GAMES
        public string TournamentName
        {
            get { return _tournamentName; }
            set
            {
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
            }
        }
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
            LeagueParticipants = new BindableCollection<LeagueParticipantModel>(SqlDataHandler.GetLeagueParticipantsForDisplay(_currentTournament.Id));

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





        





        public LeagueViewModel(TournamentModel selectedTournament)
        {
            _currentTournament = selectedTournament;
            
            _tournamentName = _currentTournament.TournamentName;
            _roundList = new BindableCollection<RoundModel>(SqlDataHandler.GetRoundsByTournament(_currentTournament.Id));
            _selectedRound = _roundList[0];
            _gameList = new BindableCollection<GameModel>(SelectedRound.Games);
            _leagueParticipants = new BindableCollection<LeagueParticipantModel>(SqlDataHandler.GetLeagueParticipantsForDisplay(_currentTournament.Id));

            ScoreDifferenceAndPositionNumber();
        }
    }
}
