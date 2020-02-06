using CustomTournamentsLibrary.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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



        public static List<TeamModel> GetTeamsByTournament(TournamentModel tournament)
        {
            List<TeamModel> participants = new List<TeamModel>();
            DynamicParameters parameters = new DynamicParameters();

            if (tournament.IsLeague)
            {
                parameters.Add("@TournamentId", tournament.Id);

                List<LeagueParticipantModel> leagueParticipants = new List<LeagueParticipantModel>();

                using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                {
                    leagueParticipants = connection.Query<LeagueParticipantModel>
                        ("dbo.SP_GetLeagueParticipantsByTournament", parameters, commandType: CommandType.StoredProcedure).ToList();
                }



                foreach (LeagueParticipantModel team in leagueParticipants)
                {
                    parameters = new DynamicParameters();

                    parameters.Add("@TeamName", team.TeamName);

                    if (!team.TeamName.Contains("Dummy"))
                    {
                        using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                        {
                            participants.Add(connection.QuerySingle<TeamModel>("dbo.SP_GetTeamByName", parameters, commandType: CommandType.StoredProcedure));
                        } 
                    }
                    else
                    {
                        participants.Add(new TeamModel { TeamName = team.TeamName });
                    }
                }
            }
            else
            {
                List<GameParticipantModel> firstRoundParticipants = new List<GameParticipantModel>();

                foreach (GameModel game in tournament.Rounds[0].Games)
                {
                    firstRoundParticipants.Add(game.Competitors[0]);
                    firstRoundParticipants.Add(game.Competitors[1]);
                }



                foreach (GameParticipantModel team in firstRoundParticipants)
                {
                    parameters = new DynamicParameters();

                    parameters.Add("@TeamName", team.TeamName);

                    if (!team.TeamName.Contains("Dummy"))
                    {
                        using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                        {
                            participants.Add(connection.QuerySingle<TeamModel>("dbo.SP_GetTeamByName", parameters, commandType: CommandType.StoredProcedure));
                        }
                    }
                    else
                    {
                        participants.Add(new TeamModel { TeamName = team.TeamName });
                    }
                }
            }

            participants = GetPlayersByTeam(participants);

            return participants;
        }



        private static List<TeamModel> GetPlayersByTeam(List<TeamModel> teams)
        {
            DynamicParameters parameters = new DynamicParameters();
            
            foreach (TeamModel team in teams)
            {
                parameters = new DynamicParameters();

                parameters.Add("@TeamId", team.Id);

                using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                {
                    team.TeamMembers.AddRange(connection.Query<PlayerModel>("dbo.SP_GetPlayersByTeam", parameters, commandType: CommandType.StoredProcedure).ToList());
                }
            }

            return teams;
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



        public static List<GameModel> GetGamesByRound(RoundModel round)
        {
            List<GameModel> games = new List<GameModel>();
            
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RoundId", round.Id);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                games = connection.Query<GameModel>("dbo.SP_GetGamesByRound", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

            foreach (GameModel game in games)
            {
                parameters = new DynamicParameters();
                parameters.Add("@GameId", game.Id);

                using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                {
                    game.Competitors = connection.Query<GameParticipantModel>("dbo.SP_GetGameParticipantsByGame", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }

            return games;
        }

        

        public static List<LeagueParticipantModel> GetLeagueParticipantsForDisplay(int tournamentId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TournamentId", tournamentId);
            
            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<LeagueParticipantModel>
                    ("dbo.SP_GetLeagueParticipantsForDisplay", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }



        private static List<LeagueParticipantModel> GetTournamentParticipantsByTournament(int tournamentId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TournamentId", tournamentId);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<LeagueParticipantModel>
                    ("dbo.SP_GetLeagueParticipantsByTournament", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }



        public static List<TeamModel> GetRoundWinners(int roundId)
        {
            List<TeamModel> roundWinners = new List<TeamModel>();
            List<string> winnerNames = new List<string>();

            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@RoundId", roundId);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                winnerNames = connection.Query<string>("dbo.SP_GetWinnerNamesByRound", parameters, commandType: CommandType.StoredProcedure).ToList();
            }

            foreach (string name in winnerNames)
            {
                parameters = new DynamicParameters();

                parameters.Add("TeamName", name);
                
                using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
                {
                    roundWinners.Add(connection.QuerySingle<TeamModel>("dbo.SP_GetTeamByName", parameters, commandType: CommandType.StoredProcedure));
                }
            }

            return roundWinners;
        }



        public static List<PrizeModel> GetPrizesByTournament(int tournamentId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@TournamentId", tournamentId);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<PrizeModel>("dbo.SP_GetPrizesByTournament", parameters, commandType: CommandType.StoredProcedure).ToList(); ;
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
            parameters.Add("@HomeAndAway", tournament.HomeAndAway);
            parameters.Add("@VictoryPoints", tournament.VictoryPoints);
            parameters.Add("@DrawPoints", tournament.DrawPoints);
            parameters.Add("@OfficialScore", tournament.OfficialScore);
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



        public static void UpdateGameParticipantAsCupRoundWinner(GameModel selectedGame)
        {
            GameParticipantModel homeTeam = selectedGame.Competitors[0];
            GameParticipantModel awayTeam = selectedGame.Competitors[1];
            DynamicParameters parameters = new DynamicParameters();
            
            if (homeTeam.Score > awayTeam.Score)
            {
                parameters.Add("@Id", homeTeam.Id);
            }
            else
            {
                parameters.Add("@Id", awayTeam.Id);
            }

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_UpdateGameParticipantCupRoundWinner", parameters, commandType: CommandType.StoredProcedure);
            }
        }



        public static void UpdateLeagueParticipants(TournamentModel tournament, GameModel game)
        {
            GameParticipantModel homeTeam = game.Competitors[0];
            GameParticipantModel awayTeam = game.Competitors[1];

            LeagueParticipantModel winner = new LeagueParticipantModel();
            LeagueParticipantModel loser = new LeagueParticipantModel();

            if (homeTeam.Score > awayTeam.Score)
            {
                winner = GetTournamentParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == homeTeam.TeamName);
                loser = GetTournamentParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == awayTeam.TeamName);

                winner.Victories += 1;
                winner.Scored += homeTeam.Score;
                winner.Conceded += awayTeam.Score;
                winner.Points += tournament.VictoryPoints;

                loser.Defeats += 1;
                loser.Scored += awayTeam.Score;
                loser.Conceded += homeTeam.Score;
            }

            else if (homeTeam.Score == awayTeam.Score)
            {
                winner = GetTournamentParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == awayTeam.TeamName);
                loser = GetTournamentParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == homeTeam.TeamName);

                winner.Draws += 1;
                winner.Scored += awayTeam.Score;
                winner.Conceded += homeTeam.Score;
                winner.Points += tournament.DrawPoints;

                loser.Draws += 1;
                loser.Scored += homeTeam.Score;
                loser.Conceded += awayTeam.Score;
                loser.Points += tournament.DrawPoints;
            }

            else
            {
                winner = GetTournamentParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == awayTeam.TeamName);
                loser = GetTournamentParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == homeTeam.TeamName);

                winner.Victories += 1;
                winner.Scored += awayTeam.Score;
                winner.Conceded += homeTeam.Score;
                winner.Points += tournament.VictoryPoints;

                loser.Defeats += 1;
                loser.Scored += homeTeam.Score;
                loser.Conceded += awayTeam.Score;
            }

            UpdateThisLeagueParticipant(winner);
            UpdateThisLeagueParticipant(loser);
        }



        private static void UpdateThisLeagueParticipant(LeagueParticipantModel team)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", team.Id);
            parameters.Add("@Victories", team.Victories);
            parameters.Add("@Draws", team.Draws);
            parameters.Add("@Defeats", team.Defeats);
            parameters.Add("@Scored", team.Scored);
            parameters.Add("@Conceded", team.Conceded);
            parameters.Add("@ScoreDifferential", team.Scored - team.Conceded);
            parameters.Add("@Points", team.Points);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_UpdateLeagueParticipant", parameters, commandType: CommandType.StoredProcedure);
            }
        }



        public static void UpdateTournamentStatus(TournamentModel tournament)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TournamentId", tournament.Id);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_UpdateTournamentStatus", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
