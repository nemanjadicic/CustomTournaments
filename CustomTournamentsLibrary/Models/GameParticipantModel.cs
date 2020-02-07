using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Models
{
    public class GameParticipantModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int RoundId { get; set; }
        public int GameId { get; set; }
        public string TeamName { get; set; }
        public int Score { get; set; }
        public bool CupRoundWinner { get; set; }
    }
}
