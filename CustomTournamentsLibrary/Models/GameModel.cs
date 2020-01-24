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
        public List<GameParticipantModel> Competitors { get; set; }
        public string GameDisplay
        {
            get
            {
                return $"{Competitors[0].TeamCompeting.TeamName} : {Competitors[1].TeamCompeting.TeamName}";
            }
        }



        public GameModel(int roundId)
        {
            RoundId = roundId;
        }

        public GameModel()
        {

        }
    }
}
