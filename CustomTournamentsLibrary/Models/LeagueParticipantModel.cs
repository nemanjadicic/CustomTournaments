using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Models
{
    public class LeagueParticipantModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public string TeamName { get; set; }
        public int Victories { get; set; } // nullable
        public int Draws { get; set; } // nullable
        public int Defeats { get; set; } // nullable
        public int Scored { get; set; } // nullable
        public int Conceded { get; set; } // nullable
        public int ScoreDifferential { get; set; } // nullable
        public int Points { get; set; } // nullable


        //  TODO - Change LeagueParticipants table field from Conceeded to Conceded
    }
}
