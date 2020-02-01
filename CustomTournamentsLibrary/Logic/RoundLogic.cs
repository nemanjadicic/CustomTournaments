using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Models;
using Medallion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomTournamentsLibrary.Logic
{
    public static class RoundLogic
    {
        public static void CreateRounds(TournamentModel tournament)
        {
            List<TeamModel> participants = tournament.ParticipatingTeams;
            participants.Shuffle();

            if (tournament.IsLeague)
            {
                //  SOLUTION FOUND ON https://stackoverflow.com/questions/1293058/round-robin-tournament-algorithm-in-c-sharp

                int numberOfRounds = GetNumberOfRounds(tournament);
                int halfOfTeams = tournament.ParticipatingTeams.Count / 2;

                List<TeamModel> shortenedTeamList = new List<TeamModel>();
                shortenedTeamList.AddRange(participants);
                shortenedTeamList.RemoveAt(0);

                int shortenedCount = shortenedTeamList.Count;



                //  Create rounds
                for (int roundNumber = 1; roundNumber <= numberOfRounds; roundNumber++)
                {
                    //  Create round
                    RoundModel round = new RoundModel(tournament.Id, roundNumber);
                    SqlDataHandler.CreateRound(tournament, round);

                    int teamIndexer = roundNumber % shortenedCount;

                    //  Create 1st game in the round
                    GameModel game = new GameModel(tournament.Id, round.Id, true);
                    SqlDataHandler.CreateGame(game);

                    //  Create 1st game competitors
                    game.Competitors.Add(new GameParticipantModel { TournamentId = tournament.Id, RoundId = round.Id, GameId = game.Id, TeamName = shortenedTeamList[teamIndexer].TeamName });
                    game.Competitors.Add(new GameParticipantModel { TournamentId = tournament.Id, RoundId = round.Id, GameId = game.Id, TeamName = participants[0].TeamName });

                    foreach  (GameParticipantModel participant in game.Competitors)
                    {
                        SqlDataHandler.CreateGameParticipant(participant);
                    }



                    //  Create other games of the round
                    for (int index = 1; index < halfOfTeams; index++)
                    {
                        int homeIndex = (roundNumber + index) % shortenedCount;
                        int awayIndex = (roundNumber + shortenedCount - index) % shortenedCount;

                        //  Create the next game
                        GameModel nextGame = new GameModel(tournament.Id, round.Id, true);
                        SqlDataHandler.CreateGame(nextGame);

                        //  Create next game's competitors
                        nextGame.Competitors.Add(new GameParticipantModel { TournamentId = tournament.Id, RoundId = round.Id, GameId = nextGame.Id, TeamName = shortenedTeamList[homeIndex].TeamName });
                        nextGame.Competitors.Add(new GameParticipantModel { TournamentId = tournament.Id, RoundId = round.Id, GameId = nextGame.Id, TeamName = shortenedTeamList[awayIndex].TeamName });

                        foreach (GameParticipantModel participant in nextGame.Competitors)
                        {
                            SqlDataHandler.CreateGameParticipant(participant); 
                        }
                    }
                }
            }
            else
            {
                int numberOfRounds = GetNumberOfRounds(tournament);

                for (int roundNumber = 1; roundNumber <= numberOfRounds; roundNumber++)
                {
                    RoundModel round = new RoundModel(tournament.Id, roundNumber);
                    SqlDataHandler.CreateRound(tournament, round);

                    if (round.RoundNumber == 1)
                    {
                        CreateCupRoundGames(participants, round);
                    }

                    //  Every next round's games will be created subsequently after all previous round's results are stored
                    //  Its only needed to get previous round winners and pass them as parameter to the CreateCupRoundGames() method
                }
            }
        }



        public static void CreateCupRoundGames(List<TeamModel> roundParticipants, RoundModel round)
        {
            int numberOfGames = roundParticipants.Count / 2;
            List<TeamModel> homeTeams = new List<TeamModel>(roundParticipants);
            List<TeamModel> awayTeams = new List<TeamModel>();

            if (homeTeams.Contains(homeTeams.Find(team => team.TeamName == "Dummy Team")))
            {
                //  Transfer all dummy teams to awayList
                foreach (TeamModel team in roundParticipants)
                {
                    if (team.TeamName == "Dummy Team")
                    {
                        awayTeams.Add(team);
                        homeTeams.Remove(team);
                    }
                }

                //  Separate teams in 2 lists
                while (awayTeams.Count != homeTeams.Count)
                {
                    awayTeams.Add(homeTeams[0]);
                    homeTeams.Remove(homeTeams[0]);
                }


                //  Pair teams from 2 lists into game
                for (int gameNumber = 1; gameNumber <= numberOfGames; gameNumber++)
                {
                    GameModel game = new GameModel(round.TournamentId, round.Id, true);
                    SqlDataHandler.CreateGame(game);

                    game.Competitors.Add
                        (new GameParticipantModel { TournamentId = round.TournamentId, RoundId = round.Id, GameId = game.Id, TeamName = homeTeams[0].TeamName });
                    game.Competitors.Add
                        (new GameParticipantModel { TournamentId = round.TournamentId, RoundId = round.Id, GameId = game.Id, TeamName = awayTeams[0].TeamName });

                    homeTeams.RemoveAt(0);
                    awayTeams.RemoveAt(0);

                    foreach (GameParticipantModel team in game.Competitors)
                    {
                        SqlDataHandler.CreateGameParticipant(team);
                    }
                }
            }
            else
            {
                //  Separate teams in 2 lists
                while (awayTeams.Count != homeTeams.Count)
                {
                    awayTeams.Add(homeTeams[0]);
                    homeTeams.Remove(homeTeams[0]);
                }


                //  Pair teams from 2 lists into game
                for (int gameNumber = 1; gameNumber <= numberOfGames; gameNumber++)
                {
                    GameModel game = new GameModel(round.TournamentId, round.Id, true);
                    SqlDataHandler.CreateGame(game);

                    game.Competitors.Add
                        (new GameParticipantModel { TournamentId = round.TournamentId, RoundId = round.Id, GameId = game.Id, TeamName = homeTeams[0].TeamName });
                    game.Competitors.Add
                        (new GameParticipantModel { TournamentId = round.TournamentId, RoundId = round.Id, GameId = game.Id, TeamName = awayTeams[0].TeamName });

                    homeTeams.RemoveAt(0);
                    awayTeams.RemoveAt(0);

                    foreach (GameParticipantModel team in game.Competitors)
                    {
                        SqlDataHandler.CreateGameParticipant(team);
                    }
                }
            }
        }



        public static void CreateDummyTeams(TournamentModel tournament)
        {
            if (tournament.IsLeague)
            {
                if (tournament.ParticipatingTeams.Count % 2 != 0)
                {
                    TeamModel dummy = new TeamModel("Dummy Team");
                    tournament.ParticipatingTeams.Add(dummy);
                }
            }
            else
            {
                int[] cupQuota = { 2, 4, 8, 16, 32, 64, 128 };
                int dummyRequirements = 0;

                foreach (int quotaNumber in cupQuota)
                {
                    if (tournament.ParticipatingTeams.Count < quotaNumber)
                    {
                        dummyRequirements = quotaNumber - tournament.ParticipatingTeams.Count;

                        for (int x = 1; x <= dummyRequirements; x++)
                        {
                            TeamModel dummy = new TeamModel($"Dummy Team");
                            tournament.ParticipatingTeams.Add(dummy);
                        }

                        break;
                    }
                    else if (tournament.ParticipatingTeams.Count == quotaNumber)
                    {
                        break;
                    }
                }
            }
        }



        private static int GetNumberOfRounds(TournamentModel tournament)
        {
            int roundsCount = 0;

            if (tournament.IsLeague)
            {
                roundsCount = tournament.ParticipatingTeams.Count - 1;
            }
            else
            {
                switch (tournament.ParticipatingTeams.Count)
                {
                    case 2:
                        roundsCount = 1;
                        break;
                    case 4:
                        roundsCount = 2;
                        break;
                    case 8:
                        roundsCount = 3;
                        break;
                    case 16:
                        roundsCount = 4;
                        break;
                    case 32:
                        roundsCount = 5;
                        break;
                    case 64:
                        roundsCount = 6;
                        break;
                }
            }

            return roundsCount;
        }
    }
}
