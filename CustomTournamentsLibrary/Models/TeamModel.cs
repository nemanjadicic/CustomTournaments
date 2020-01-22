using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Models
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<PlayerModel> TeamMembers { get; set; } = new List<PlayerModel>();



        public TeamModel(string name)
        {
            TeamName = name;
        }

        public TeamModel()
        {

        }
    }
}
