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
            var conductor = Parent as IConductor;

            if (SelectedTournament.IsLeague)
            {
                conductor.ActivateItem(new LeagueViewModel(SelectedTournament));
            }
            else
            {
                // TODO - Implement this
            }
        }
        
        public void CreateNewTournament()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(new CreateTournamentViewModel());
        }

        public HomeViewModel()
        {
            _existingTournaments = new BindableCollection<TournamentModel>(SqlDataHandler.GetAllTournaments());
        }
    }
}
