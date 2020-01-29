using Caliburn.Micro;
using CustomTournamentsLibrary.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTournamentsLibrary.DataAccess
{
    public class SqlDataHandler
    {
        public static List<TournamentModel> GetAllTournaments()
        {
            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<TournamentModel>("dbo.SP_GetAllTournaments").ToList();
            }
        }

        public static List<PlayerModel> GetAllPlayers()
        {
            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<PlayerModel>("dbo.SP_GetAllPlayers").ToList();
            }
        }

        public static List<TeamModel> GetAllTeams()
        {
            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<TeamModel>("dbo.SP_GetAllTeams").ToList();
            }
        }

        public static void CreatePlayer(PlayerModel player)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@FirstName", player.FirstName);
            parameters.Add("@LastName", player.LastName);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewPlayer", parameters, commandType: CommandType.StoredProcedure);
            }

            player.Id = parameters.Get<int>("@Id");
        }

        public static List<RoundModel> GetRoundsByTournament(int tournamentId)
        {
            List<RoundModel> rounds = new List<RoundModel>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TournamentId", tournamentId);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                rounds = connection.Query<RoundModel>("dbo.SP_GetRoundsByTournament", parameters, commandType: CommandType.StoredProcedure).ToList();
            }





            foreach (RoundModel round in rounds)
            {
                parameters = new DynamicParameters();
                parameters.Add("@RoundId", round.Id);

                using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                {
                    round.Games = connection.Query<GameModel>("dbo.SP_GetGamesByRound", parameters, commandType: CommandType.StoredProcedure).ToList();
                }





                foreach (GameModel game in round.Games)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@GameId", game.Id);

                    using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                    {
                        game.Competitors = connection.Query<GameParticipantModel>("dbo.SP_GetGameParticipantsByGame", parameters, commandType: CommandType.StoredProcedure).ToList();
                    }
                }
            }





            return rounds;
        }

        public static List<LeagueParticipantModel> GetLeagueParticipantsByTournament(TournamentModel tournament)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TournamentId", tournament.Id);
            
            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<LeagueParticipantModel>("dbo.SP_GetLeagueParticipants", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public static void CreateTeam(TeamModel team)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TeamName", team.TeamName);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewTeam", parameters, commandType: CommandType.StoredProcedure);
            }

            team.Id = parameters.Get<int>("@Id");
        }

        public static void CreateTeamMembers(TeamModel team, PlayerModel player)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TeamId", team.Id);
            parameters.Add("@PlayerId", player.Id);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewTeamMember", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public static void CreateTournament(TournamentModel tournament)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TournamentName", tournament.TournamentName);
            parameters.Add("@IsLeague", tournament.IsLeague);
            parameters.Add("@EntryFee", tournament.EntryFee);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewTournament", parameters, commandType: CommandType.StoredProcedure);
            }

            tournament.Id = parameters.Get<int>("@Id");
        }

        public static void CreateLeagueParticipant(TournamentModel tournament, TeamModel team)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TournamentId", tournament.Id);
            parameters.Add("@TeamName", team.TeamName);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewLeagueParticipant", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public static void CreatePrize(TournamentModel tournament, PrizeModel prize)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TournamentId", tournament.Id);
            parameters.Add("@PlaceNumber", prize.PlaceNumber);
            parameters.Add("@Placename", prize.PlaceName);
            parameters.Add("@PrizeAmount", prize.PrizeAmount);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewPrize", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        internal static void CreateRound(TournamentModel tournament, RoundModel round)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TournamentId", tournament.Id);
            parameters.Add("@RoundNumber", round.RoundNumber);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewRound", parameters, commandType: CommandType.StoredProcedure);
            }

            round.Id = parameters.Get<int>("@Id");
        }

        internal static void CreateGame(GameModel game)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TournamentId", game.TournamentId);
            parameters.Add("@RoundId", game.RoundId);
            parameters.Add("@Unplayed", game.Unplayed);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewGame", parameters, commandType: CommandType.StoredProcedure);
            }

            game.Id = parameters.Get<int>("@Id");
        }

        internal static void CreateGameParticipant(GameParticipantModel participant)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TournamentId", participant.TournamentId);
            parameters.Add("RoundId", participant.RoundId);
            parameters.Add("@GameId", participant.GameId);
            parameters.Add("@TeamName", participant.TeamName);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewGameParticipant", parameters, commandType: CommandType.StoredProcedure);
            }

            participant.Id = parameters.Get<int>("@Id");
        }

        public static void UpdateGameScoreAndStatus(GameModel game)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", game.Id);
            parameters.Add("@Unplayed", game.Unplayed);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_UpdateGameStatus", parameters, commandType: CommandType.StoredProcedure);
            }

            foreach (GameParticipantModel team in game.Competitors)
            {
                parameters = new DynamicParameters();
                parameters.Add("@Id", team.Id);
                parameters.Add("@Score", team.Score);

                using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                {
                    connection.Execute("dbo.SP_UpdateGameParticipantScore", parameters, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public static void UpdateLeagueParticipants(GameModel game)
        {
            GameParticipantModel homeTeam = game.Competitors[0];
            GameParticipantModel awayTeam = game.Competitors[1];

            LeagueParticipantModel winner;
            LeagueParticipantModel loser;

            if (homeTeam.Score > awayTeam.Score)
            {
                
            }

            if (homeTeam.Score == awayTeam.Score)
            {

            }

            if (awayTeam.Score > homeTeam.Score)
            {

            }
        }
    }
}
