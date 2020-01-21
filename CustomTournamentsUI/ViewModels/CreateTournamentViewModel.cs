using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsUI.ViewModels
{
    public class CreateTournamentViewModel : Screen
    {
        public void CreateNewTeam()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(new CreateTeamViewModel());
        }
        
        public void CreatePrize()
        {
            var conductor = Parent as IConductor;
            conductor.ActivateItem(new CreatePrizeViewModel());
        }
    }
}
