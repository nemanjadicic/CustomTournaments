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
        public bool Unplayed { get; set; } = true;
        public string GameDisplay
        {
            get
            {
                GameParticipantModel homeTeam = Competitors[0];
                GameParticipantModel awayTeam = Competitors[1];
                
                if (Unplayed)
                {
                    return $"{homeTeam.TeamName} : {awayTeam.TeamName}";
                }
                else
                {
                    return $"{homeTeam.TeamName} {homeTeam.Score} : {awayTeam.Score} {awayTeam.TeamName}";
                }
            }
        }



        public GameModel(int tourneyId, int roundId, bool unplayed)
        {
            TournamentId = tourneyId;
            RoundId = roundId;
            Unplayed = unplayed;
        }

        public GameModel()
        {

        }
    }
}
