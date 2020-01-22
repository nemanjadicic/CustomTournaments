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
        public int RoundId { get; set; }
        public int GameId { get; set; }
        public GameModel PreviousCupGame { get; set; } // nullable
        public TeamModel TeamCompeting { get; set; } // nullable
        public int Score { get; set; } // nullable
        public bool CupRoundWinner { get; set; } // nullable
    }
}
