using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Models
{
    public class PrizeModel
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int PlaceNumber { get; set; }
        public string PlaceName { get; set; }
        public decimal PrizeAmount { get; set; }
        public string PrizeDisplay
        {
            get
            {
                return $"{PlaceName}, {PrizeAmount}";
            }
        }



        public PrizeModel(int placenumber, string placename, decimal prizeamount)
        {
            PlaceNumber = placenumber;
            PlaceName = placename;
            PrizeAmount = prizeamount;
        }

        public PrizeModel()
        {

        }
    }
}
