using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Models
{
    public class RoundModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int RoundNumber { get; set; }
        public List<GameModel> Games { get; set; } = new List<GameModel>();



        public RoundModel(int tourneyId, int roundnumber)
        {
            TournamentId = tourneyId;
            RoundNumber = roundnumber;
        }

        public RoundModel()
        {

        }
    }
}
