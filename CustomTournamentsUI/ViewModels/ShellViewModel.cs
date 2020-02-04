using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public IScreen CurrentView { get; set; }



        public ShellViewModel()
        {
            CurrentView = new HomeViewModel();
            ActivateItem(CurrentView);
        }



        public void LoadHomeView()
        {
            CurrentView = new HomeViewModel();
            ActivateItem(CurrentView);
        }



        public void LoadCreateTournamentView()
        {
            //CurrentView = new CreateNewTournamentViewModel();
            //ActivateItem(CurrentView);

            CurrentView = new TournamentSummaryViewModel();
            ActivateItem(CurrentView);
        }



        public void LoadCreateTeamView()
        {
            CurrentView = new CreateTeamViewModel();
            ActivateItem(CurrentView);
        }
    }
}
