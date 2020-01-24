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
    }
}
