using Caliburn.Micro;
using CustomTournamentsLibrary.Models;

namespace CustomTournamentsLibrary.Interfaces
{
    public interface IEnterResult
    {
        TournamentModel CurrentTournament { get; set; }
        BindableCollection<RoundModel> RoundList { get; set; }
        RoundModel SelectedRound { get; set; }
        BindableCollection<GameModel> GameList { get; set; }
        GameModel SelectedGame { get; set; }
        BindableCollection<LeagueParticipantModel> LeagueParticipants { get; set; }
        bool CanEnterResult { get; set; }
    }
}
