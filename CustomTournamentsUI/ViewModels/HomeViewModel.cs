using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsUI.ViewModels
{
    public class HomeViewModel : Screen
    {
        //                  BACKING FIELDS
        private BindableCollection<TournamentModel> _existingTournaments;
        private TournamentModel _selectedTournament;
        private bool _canLoadTournament;





        //                  PROPERTIES
        public BindableCollection<TournamentModel> ExistingTournaments
        {
            get { return _existingTournaments; }
            set 
            { 
                _existingTournaments = value;
                NotifyOfPropertyChange(() => ExistingTournaments);
            }
        }
        public TournamentModel SelectedTournament
        {
            get { return _selectedTournament; }
            set 
            { 
                _selectedTournament = value;
                NotifyOfPropertyChange(() => SelectedTournament);

                if (SelectedTournament != null)
                {
                    CanLoadTournament = true;
                }
                else
                {
                    CanLoadTournament = false;
                }
            }
        }
        public bool CanLoadTournament
        {
            get { return _canLoadTournament; }
            set 
            { 
                _canLoadTournament = value;
                NotifyOfPropertyChange(() => CanLoadTournament);
            }
        }






        //                  METHODS AND CONSTRUCTOR
        public void LoadTournament()
        {
            //TournamentModel tournament = SelectedTournament;

            //var conductor = Parent as IConductor;
            //if (tournament.IsLeague)
            //{
            //    conductor.ActivateItem(new LeagueViewModel(tournament));
            //}
            //else
            //{
            //    conductor.ActivateItem(new CupViewModel(tournament));
            //}

            // TODO - Uncomment when CupViewModel (and CupView) and LeagueViewModel (and LeagueView) are ready
        }
        
        public void CreateNewTournament()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(new CreateTournamentViewModel());
        }

        public HomeViewModel()
        {
            List<TournamentModel> tournaments = new List<TournamentModel>();
            tournaments.Add(new TournamentModel { TournamentName = "Ping Pong SuperLiga Srbije", IsLeague = false });
            tournaments.Add(new TournamentModel { TournamentName = "Besmisleni Kup", IsLeague = false });
            tournaments.Add(new TournamentModel { TournamentName = "Topli Drugari", IsLeague = true });
            tournaments.Add(new TournamentModel { TournamentName = "Izbori 2020.", IsLeague = true });

            _existingTournaments = new BindableCollection<TournamentModel>(tournaments);

            // TODO - Delete this dummy data when the real data from the database is ready
        }
    }
    
    public class TournamentModel
    {
        public string TournamentName { get; set; }
        public bool IsLeague { get; set; }

        // TODO - Delete this temporary class when the real TournamentModel class is ready
    }
}
