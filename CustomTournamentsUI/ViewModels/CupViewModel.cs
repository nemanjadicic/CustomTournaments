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
    public class CupViewModel : Screen, IEnterResult
    {
        //          BACKING FIELDS
        private string _tournamentName;
        private BindableCollection<RoundModel> _roundList;
        private RoundModel _selectedRound;
        private string _roundDisplay;
        private bool _unplayedOnly = false;
        private BindableCollection<GameModel> _gameList;
        private GameModel _selectedGame;
        private bool _canEnterResult = true;





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
            if (SelectedRound.Games.Count == 0)
            {
                SelectedRound.Games = SqlDataHandler.GetGamesByRound(SelectedRound); 
            }
            
            if (UnplayedOnly)
            {
                GameList = new BindableCollection<GameModel>(SelectedRound.Games.Where(game => game.Unplayed == true));
            }
            else
            {
                GameList = new BindableCollection<GameModel>(SelectedRound.Games);
            }

            DisplayRound();
        }
        public string RoundDisplay
        {
            get { return _roundDisplay; }
            set 
            { 
                _roundDisplay = value;
                NotifyOfPropertyChange(() => RoundDisplay);
            }
        }
        private void DisplayRound()
        {
            int numberOfRounds = RoundList.Count;

            if (SelectedRound.RoundNumber == numberOfRounds)
            {
                RoundDisplay = $"Round {SelectedRound.RoundNumber} - Final";
            }
            else if (SelectedRound.RoundNumber == numberOfRounds - 1)
            {
                RoundDisplay = $"Round {SelectedRound.RoundNumber} - Semifinals";
            }
            else if (SelectedRound.RoundNumber == numberOfRounds - 2)
            {
                RoundDisplay = $"Round {SelectedRound.RoundNumber} - Quarterfinals";
            }
            else
            {
                RoundDisplay = $"Round {SelectedRound.RoundNumber}";
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





        public CupViewModel(TournamentModel selectedTournament)
        {
            CurrentTournament = selectedTournament;

            _tournamentName = CurrentTournament.TournamentName;
            _roundList = new BindableCollection<RoundModel>(SqlDataHandler.GetRoundsByTournament(CurrentTournament.Id));
            _selectedRound = _roundList[0];
            _gameList = new BindableCollection<GameModel>(_selectedRound.Games);
            DisplayRound();
        }

        public BindableCollection<LeagueParticipantModel> LeagueParticipants { get; set; }
    }
}
