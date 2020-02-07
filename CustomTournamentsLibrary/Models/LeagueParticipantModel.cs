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
        public int Victories { get; set; }
        public int Draws { get; set; }
        public int Defeats { get; set; }
        public int Scored { get; set; }
        public int Conceded { get; set; }
        public int ScoreDifferential { get; set; }
        public int Points { get; set; }
    }
}
