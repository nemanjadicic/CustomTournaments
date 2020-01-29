using Caliburn.Micro;
using CustomTournamentsLibrary.Models;

namespace CustomTournamentsLibrary.Interfaces
{
    public interface IEnterResult
    {
        GameModel SelectedGame { get; set; }
        BindableCollection<GameModel> GameList { get; set; }
        BindableCollection<LeagueParticipantModel> LeagueParticipants { get; set; }
    }
}
