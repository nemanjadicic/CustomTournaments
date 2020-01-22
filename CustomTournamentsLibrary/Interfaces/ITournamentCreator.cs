using Caliburn.Micro;
using CustomTournamentsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Interfaces
{
    public interface ITournamentCreator
    {
        BindableCollection<TeamModel> TournamentTeams { get; set; }
        BindableCollection<PrizeModel> TournamentPrizes { get; set; }
    }
}
