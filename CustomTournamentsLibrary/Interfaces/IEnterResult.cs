using Caliburn.Micro;
using CustomTournamentsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.Interfaces
{
    public interface IEnterResult
    {
        GameModel SelectedGame { get; set; }
    }
}
