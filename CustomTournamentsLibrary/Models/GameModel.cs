using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Models
{
    public class GameModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int RoundId { get; set; }
        public List<GameParticipantModel> Competitors { get; set; } = new List<GameParticipantModel>();
        public string GameDisplay
        {
            get
            {
                return $"{Competitors[0].TeamName} : {Competitors[1].TeamName}";
            }
        }



        public GameModel(int tourneyId, int roundId)
        {
            TournamentId = tourneyId;
            RoundId = roundId;
        }

        public GameModel()
        {

        }
    }
}
