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
    public class CreateTeamViewModel : Screen
    {
        //          BACKING FIELDS
        ITournamentCreator _tournamentCreationView;
        
        private string _firstName;
        private string _lastName;
        private bool _canCreatePlayer;

        private string _teamName;
        private BindableCollection<PlayerModel> _availablePlayers;
        private PlayerModel _selectedPlayer;
        private BindableCollection<PlayerModel> _teamMembers;
        private PlayerModel _selectedMember;
        private bool _canCreateTeam;
        private string _errorMessage;

        



        //          PLAYER RELATED PROPERTIES AND METHODS
        public string FirstName
        {
            get { return _firstName; }
            set 
            { 
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);

                if (String.IsNullOrWhiteSpace(FirstName) || String.IsNullOrWhiteSpace(LastName))
                {
                    CanCreatePlayer = false;
                }
                else
                {
                    CanCreatePlayer = true;
                }
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set 
            { 
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);

                if (String.IsNullOrWhiteSpace(FirstName) || String.IsNullOrWhiteSpace(LastName))
                {
                    CanCreatePlayer = false;
                }
                else
                {
                    CanCreatePlayer = true;
                }
            }
        }
        public bool CanCreatePlayer
        {
            get { return _canCreatePlayer; }
            set 
            { 
                _canCreatePlayer = value;
                NotifyOfPropertyChange(() => CanCreatePlayer);
            }
        }
        public void CreatePlayer()
        {
            PlayerModel player = new PlayerModel(FirstName, LastName);
            SqlDataHandler.CreatePlayer(player);
            TeamMembers.Add(player);
            ValidateAllData();

            FirstName = null;
            LastName = null;
        }





        //          TEAM RELATED PROPERTIES AND METHODS
        public string TeamName
        {
            get { return _teamName; }
            set 
            { 
                _teamName = value;
                NotifyOfPropertyChange(() => TeamName);
                ValidateAllData();
            }
        }
        public BindableCollection<PlayerModel> AvailablePlayers
        {
            get { return _availablePlayers; }
            set 
            {
                _availablePlayers = value;
                NotifyOfPropertyChange(() => AvailablePlayers);
            }
        }
        public PlayerModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            set 
            { 
                _selectedPlayer = value;
                NotifyOfPropertyChange(() => SelectedPlayer);
            }
        }
        public BindableCollection<PlayerModel> TeamMembers
        {
            get { return _teamMembers; }
            set 
            { 
                _teamMembers = value;
                NotifyOfPropertyChange(() => TeamMembers);
                ValidateAllData();
            }
        }
        public PlayerModel SelectedMember
        {
            get { return _selectedMember; }
            set 
            { 
                _selectedMember = value;
                NotifyOfPropertyChange(() => SelectedMember);
            }
        }
        public void AddPlayer()
        {
            if (SelectedPlayer != null)
            {
                TeamMembers.Add(SelectedPlayer);
                AvailablePlayers.Remove(SelectedPlayer);
            }

            ValidateAllData();
        }
        public void RemoveSelectedMember()
        {
            if (SelectedMember != null)
            {
                AvailablePlayers.Add(SelectedMember);
                TeamMembers.Remove(SelectedMember);
            }

            ValidateAllData();
        }
        public bool CanCreateTeam
        {
            get { return _canCreateTeam; }
            set 
            { 
                _canCreateTeam = value;
                NotifyOfPropertyChange(() => CanCreateTeam);
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
        public void ValidateAllData()
        {
            List<string> errors = new List<string>();
            List<TeamModel> existingTeams = SqlDataHandler.GetAllTeams();
            List<string> usedNames = new List<string>();
            string nameLower = "";
            if (TeamName != null)
            {
                nameLower = TeamName.ToLower(); 
            }

            foreach (TeamModel team in existingTeams)
            {
                usedNames.Add(team.TeamName);
            }

            if (usedNames.Contains(TeamName))
            {
                errors.Add($"Team name {TeamName} is already taken.");
            }

            if (String.IsNullOrWhiteSpace(TeamName))
            {
                errors.Add("Team must have a name.");
            }

            if (nameLower.Contains("dummy"))
            {
                errors.Add(@"Team name must not contain word ""dummy"".");
            }

            if (TeamMembers.Count == 0)
            {
                errors.Add("Team must have at least 1 member.");
            }

            bool somethingWrong = usedNames.Contains(TeamName) || String.IsNullOrWhiteSpace(TeamName)
                || nameLower.Contains("dummy") || TeamMembers.Count == 0;

            if (somethingWrong)
            {
                CanCreateTeam = false;
                ErrorMessage = $"* {string.Join(" ", errors)}";
            }
            else
            {
                CanCreateTeam = true;
                ErrorMessage = null;
            }
        }
        public void CreateTeam()
        {
            TeamModel team = new TeamModel(TeamName);
            SqlDataHandler.CreateTeam(team);

            foreach (PlayerModel player in TeamMembers)
            {
                team.TeamMembers.Add(player);
                SqlDataHandler.CreateTeamMembers(team, player);
            }

            if (_tournamentCreationView != null)
            {
                var conductor = Parent as IConductor;
                _tournamentCreationView.TournamentTeams.Add(team);
                conductor.ActivateItem(_tournamentCreationView);
            }
            else
            {
                TeamName = "";
                TeamMembers.Clear();
                ErrorMessage = null;
            }
        }





        //          CONSTRUCTORS
        public CreateTeamViewModel(ITournamentCreator previousView)
        {
            _tournamentCreationView = previousView;
            _availablePlayers = new BindableCollection<PlayerModel>(SqlDataHandler.GetAllPlayers());
            _teamMembers = new BindableCollection<PlayerModel>();
        }
        public CreateTeamViewModel()
        {
            _availablePlayers = new BindableCollection<PlayerModel>(SqlDataHandler.GetAllPlayers());
            _teamMembers = new BindableCollection<PlayerModel>();
        }
    }
}
