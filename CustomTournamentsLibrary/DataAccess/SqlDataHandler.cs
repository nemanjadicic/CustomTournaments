using CustomTournamentsLibrary.Models;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CustomTournamentsLibrary.DataAccess
{
    public class SqlDataHandler
    {
        /// <summary>
        /// Retrieves all records of the specific data type from the database
        /// </summary>
        /// <typeparam name="T">Data type you want to retrieve from the DB</typeparam>
        /// <param name="storedProcedure">Name of the stored procedure you are using to retrieve data. Example "dbo.SP_GetAllPlayers"</param>
        /// <returns>List of data objects you specified</returns>
        public static List<T> GetAllData<T>(string storedProcedure)
        {
            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<T>(storedProcedure).ToList();
            }
        }



        /// <summary>
        /// Retrieves participants of the specific tournament
        /// </summary>
        /// <param name="tournament">Tournament you want participants from</param>
        /// <returns>List of teams participating in the given tournament</returns>
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

            participants = GetPlayersOfTheseTeams(participants);

            return participants;
        }



        /// <summary>
        /// Retrieves all players of given teams and populates teams with those players
        /// </summary>
        /// <param name="teams">Teams you want to be populated with players</param>
        /// <returns>List of teams fully populated with their players</returns>
        private static List<TeamModel> GetPlayersOfTheseTeams(List<TeamModel> teams)
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

        

        /// <summary>
        /// Retrieves rounds of the given tournament
        /// </summary>
        /// <param name="tournamentId">Id of the tournament you want rounds from</param>
        /// <returns>List of rounds of the given torunament</returns>
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



        /// <summary>
        /// Retrieves games of the specific round
        /// </summary>
        /// <param name="round">Round you want games from</param>
        /// <returns>List of games of the given round</returns>
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

        

        /// <summary>
        /// Retrieves league participants from the DB. An ordered list designed for displaying on league table
        /// </summary>
        /// <param name="tournamentId">Id of the tournament (leage) you want league participants from</param>
        /// <returns>Ordered list of league participants</returns>
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



        /// <summary>
        /// Retrieves all league participant data of the given tournament. List designed for further use
        /// </summary>
        /// <param name="tournamentId">Id of the tournament (leage) you want league participants from</param>
        /// <returns>List of league participants</returns>
        private static List<LeagueParticipantModel> GetLeagueParticipantsByTournament(int tournamentId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TournamentId", tournamentId);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<LeagueParticipantModel>
                    ("dbo.SP_GetLeagueParticipantsByTournament", parameters, commandType: CommandType.StoredProcedure).ToList();
            }
        }



        /// <summary>
        /// Retrieves all teams that won the given cup round and advanced to the next round
        /// </summary>
        /// <param name="roundId">Id of the round you want winners from</param>
        /// <returns>List of teams that won the given round</returns>
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



        /// <summary>
        /// Retrieves all prizes of the specific tournament
        /// </summary>
        /// <param name="tournamentId">Id of the tournament you want prizes from</param>
        /// <returns>List of prizes of the given tournament</returns>
        public static List<PrizeModel> GetPrizesByTournament(int tournamentId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@TournamentId", tournamentId);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                return connection.Query<PrizeModel>("dbo.SP_GetPrizesByTournament", parameters, commandType: CommandType.StoredProcedure).ToList(); ;
            }
        }



        /// <summary>
        /// Creates a new record of player class in the DB
        /// </summary>
        /// <param name="player">PlayerModel class object you want to store in the DB</param>
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



        /// <summary>
        /// Creates a new record of team class in the DB
        /// </summary>
        /// <param name="team">TeamModel class object you want to store in the DB</param>
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



        /// <summary>
        /// Creates new record of team members. TeamMembers table contains TeamId and PlayerId foreign keys
        /// </summary>
        /// <param name="team">Team you want member to be a part of</param>
        /// <param name="player">Player you want to be member of the team</param>
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



        /// <summary>
        /// Creates a new record od tournament class in the DB
        /// </summary>
        /// <param name="tournament">TournamentModel class object you want to store in the DB</param>
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



        /// <summary>
        /// Creates a new record of league participant class in the DB
        /// </summary>
        /// <param name="tournament">Tournament that league participant is participating in</param>
        /// <param name="team">Team that represents the league participant you want to store in the DB</param>
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


        /// <summary>
        /// Creates a new record of prize model class in the DB
        /// </summary>
        /// <param name="tournament">Tournament that this particular prize exists in</param>
        /// <param name="prize">PrizeModel class object you want to store in the DB</param>
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



        /// <summary>
        /// Creates a new record of round model class in the DB
        /// </summary>
        /// <param name="tournament">Tournament you want this round to exist in</param>
        /// <param name="round">RoundModel class object you want to store in the DB</param>
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



        /// <summary>
        /// Creates a new record of game model class in the DB
        /// </summary>
        /// <param name="game">GameModel class object you want to store in the DB</param>
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



        /// <summary>
        /// Creates a new record of game participant model in the DB
        /// </summary>
        /// <param name="participant">GameParticipantModel class object you want to store in the DB</param>
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



        /// <summary>
        /// Updates a particular game's status in the DB by labeling in as played
        /// </summary>
        /// <param name="game">Game you want to update</param>
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



        /// <summary>
        /// Updates a particular game's participants in the DB by labeling them as cup round winner/loser
        /// </summary>
        /// <param name="selectedGame">Game you want game participants to be updated</param>
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



        /// <summary>
        /// Updates league participants that played in a specific game, on a specific tournament
        /// </summary>
        /// <param name="tournament">Tournament where the game of the 2 league participants happened on</param>
        /// <param name="game">Game 2 league participants played on</param>
        public static void UpdateLeagueParticipants(TournamentModel tournament, GameModel game)
        {
            GameParticipantModel homeTeam = game.Competitors[0];
            GameParticipantModel awayTeam = game.Competitors[1];

            LeagueParticipantModel winner = new LeagueParticipantModel();
            LeagueParticipantModel loser = new LeagueParticipantModel();

            if (homeTeam.Score > awayTeam.Score)
            {
                winner = GetLeagueParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == homeTeam.TeamName);
                loser = GetLeagueParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == awayTeam.TeamName);

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
                winner = GetLeagueParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == awayTeam.TeamName);
                loser = GetLeagueParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == homeTeam.TeamName);

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
                winner = GetLeagueParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == awayTeam.TeamName);
                loser = GetLeagueParticipantsByTournament(game.TournamentId).Find(team => team.TeamName == homeTeam.TeamName);

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



        /// <summary>
        /// Updates records of the specific league participant
        /// </summary>
        /// <param name="team">League participant you want records to be updated of</param>
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



        /// <summary>
        /// Updates the given tournament's status by labeling it as finished
        /// </summary>
        /// <param name="tournament">Tournament you want status to be updated of</param>
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
