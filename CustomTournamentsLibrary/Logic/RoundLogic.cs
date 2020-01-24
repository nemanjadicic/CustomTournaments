using CustomTournamentsLibrary.DataAccess;
using CustomTournamentsLibrary.Models;
using Medallion;
using System;
using System.Collections.Generic;

namespace CustomTournamentsLibrary.Logic
{
    public static class RoundLogic
    {
        public static void CreateRounds(TournamentModel tournament)
        {
            List<TeamModel> participants = new List<TeamModel>(RandomizeTeamOrder(tournament.ParticipatingTeams));

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
                    GameModel game = new GameModel(tournament.Id, round.Id);
                    SqlDataHandler.CreateGame(tournament, game);

                    //  Create 1st game competitors
                    game.Competitors.Add(new GameParticipantModel { RoundId = round.Id, GameId = game.Id, TeamCompeting = shortenedTeamList[teamIndexer] });
                    game.Competitors.Add(new GameParticipantModel { RoundId = round.Id, GameId = game.Id, TeamCompeting = participants[0] });

                    foreach  (GameParticipantModel participant in game.Competitors)
                    {
                        SqlDataHandler.CreateGameParticipants(participant);
                    }



                    //  Create other games of the round
                    for (int index = 1; index < halfOfTeams; index++)
                    {
                        int homeIndex = (roundNumber + index) % shortenedCount;
                        int awayIndex = (roundNumber + shortenedCount - index) % shortenedCount;

                        //  Create the next game
                        GameModel nextGame = new GameModel(tournament.Id, round.Id);
                        SqlDataHandler.CreateGame(tournament, nextGame);

                        //  Create next game's competitors
                        nextGame.Competitors.Add(new GameParticipantModel { RoundId = round.Id, GameId = nextGame.Id, TeamCompeting = shortenedTeamList[homeIndex] });
                        nextGame.Competitors.Add(new GameParticipantModel { RoundId = round.Id, GameId = nextGame.Id, TeamCompeting = shortenedTeamList[awayIndex] });

                        foreach (GameParticipantModel participant in nextGame.Competitors)
                        {
                            SqlDataHandler.CreateGameParticipants(participant); 
                        }
                    }
                }
            }
            else
            {

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
                int[] cupQuota = { 2, 4, 8, 16, 32, 64 };
                int dummyRequirements = 0;

                foreach (int quotaNumber in cupQuota)
                {
                    if (tournament.ParticipatingTeams.Count < quotaNumber)
                    {
                        dummyRequirements = quotaNumber - tournament.ParticipatingTeams.Count;

                        for (int x = 1; x <= dummyRequirements; x++)
                        {
                            TeamModel dummy = new TeamModel($"Dummy Team {x}");
                            tournament.ParticipatingTeams.Add(dummy);
                        }

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

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            teams.Shuffle();

            return teams;
        }
    }
}
