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
        public bool HomeAndAway { get; set; } = false;
        public int VictoryPoints { get; set; }
        public int DrawPoints { get; set; }
        public int OfficialScore { get; set; }
        public decimal EntryFee { get; set; }
        public List<TeamModel> ParticipatingTeams { get; set; } = new List<TeamModel>();
        public List<PrizeModel> TournamentPrizes { get; set; } = new List<PrizeModel>();
        public List<RoundModel> Rounds { get; set; } = new List<RoundModel>();
        public bool Finished { get; set; }



        public TournamentModel(string name, bool leagueOrNot, bool homeAway, int victory, int draw, int offScore, decimal fee)
        {
            TournamentName = name;
            IsLeague = leagueOrNot;
            HomeAndAway = homeAway;
            VictoryPoints = victory;
            DrawPoints = draw;
            OfficialScore = offScore;
            EntryFee = fee;
        }

        public TournamentModel()
        {

        }
    }
}
