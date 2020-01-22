using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomTournamentsLibrary;
using CustomTournamentsLibrary.Models;
using CustomTournamentsLibrary.Interfaces;

namespace CustomTournamentsUI.ViewModels
{
    public class CreateTournamentViewModel : Screen, ITournamentCreator
    {
        private string _tournamentName;
        private bool _isLeague;
        private bool _isCup;

        private BindableCollection<TeamModel> _availableTeams;
        private TeamModel _selectedAvailableTeam;
        private BindableCollection<TeamModel> _tournamentTeams;
        private TeamModel _selectedTournamentTeam;
        
        private decimal _entryFee;
        private BindableCollection<PrizeModel> _tournamentPrizes;
        private PrizeModel _selectedPrize;









        public string TournamentName
        {
            get { return _tournamentName; }
            set 
            { 
                _tournamentName = value;
                NotifyOfPropertyChange(() => TournamentName);
            }
        }
        public bool IsLeague
        {
            get { return _isLeague; }
            set
            {
                _isLeague = value;
                NotifyOfPropertyChange(() => IsLeague);
            }
        }
        public bool IsCup
        {
            get { return _isCup; }
            set
            {
                _isCup = value;
                NotifyOfPropertyChange(() => IsCup);
            }
        }










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

        }
        public void RemoveTeamFromTournament()
        {

        }









        













        public decimal EntryFee
        {
            get { return _entryFee; }
            set 
            { 
                _entryFee = value;
                NotifyOfPropertyChange(() => EntryFee);
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
        public void CreatePrize()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(new CreatePrizeViewModel(this));
        }











        
        

        public CreateTournamentViewModel()
        {
            _tournamentTeams = new BindableCollection<TeamModel>();
        }
    }
}
