using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Models
{
    public class TournamentModel
    {
        public int Id { get; set; }
        public string TournamentName { get; set; }
        public bool IsLeague { get; set; }
        public decimal EntryFee { get; set; } // nullable
        public List<TeamModel> ParticipatingTeams { get; set; } = new List<TeamModel>();
        public List<PrizeModel> TournamentPrizes { get; set; } = new List<PrizeModel>();
        public List<RoundModel> Rounds { get; set; } = new List<RoundModel>();



        public TournamentModel(string name, bool league, decimal entryfee)
        {
            TournamentName = name;
            IsLeague = league;
            EntryFee = entryfee;
        }

        public TournamentModel()
        {

        }
    }
}
