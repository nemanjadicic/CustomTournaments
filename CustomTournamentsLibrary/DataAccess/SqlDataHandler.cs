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

        public static void InsertNewPlayer(PlayerModel player)
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

        public static void InsertNewTeam(TeamModel team)
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

        public static void InsertNewTeamMembers(TeamModel team, PlayerModel player)
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

        public static void InsertNewTournament(TournamentModel tournament)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@Id", 0, dbType:DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@TournamentName", tournament.TournamentName);
            parameters.Add("@IsLeague", tournament.IsLeague);
            parameters.Add("@EntryFee", tournament.EntryFee);

            using (IDbConnection connection = new SqlConnection(DatabaseAccess.GetConnectionString()))
            {
                connection.Execute("dbo.SP_InsertNewTournament", parameters, commandType: CommandType.StoredProcedure);
            }

            tournament.Id = parameters.Get<int>("@Id");
        }

        public static void InsertNewPrize(PrizeModel prize)
        {
            
        }
    }
}
