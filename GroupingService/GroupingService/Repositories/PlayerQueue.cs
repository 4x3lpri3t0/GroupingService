using GroupingService.Entities;
using GroupingService.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GroupingService.Repositories
{
    // Using static classes like this is horrible, but I needed to come up
    // with something fast in order to advance with the rest of the exercise. Sorry!
    public static class PlayerQueue
    {
        private static Queue<Player> unassignedQueue = new Queue<Player>();

        private static IList<MatchGroup> matches = new List<MatchGroup>();

        private static int playersPerMatch = 5; // Default

        private static int currentMatchNumber = 1;

        public static IList<MatchGroup> GetMatches()
        {
            return matches;
        }

        public static void SetPlayersPerMatch(int playersPerGroup)
        {
            playersPerMatch = playersPerGroup;
        }

        public static void AddPlayerToQueue(Player player, IHostingEnvironment hostingEnvironment)
        {
            player.QueueIngressTime = DateTime.Now.Ticks;
            unassignedQueue.Enqueue(player);

            TryCreateMatch(hostingEnvironment);
        }

        private static void TryCreateMatch(IHostingEnvironment hostingEnvironment)
        {
            if (unassignedQueue.Count < playersPerMatch)
            {
                return;
            }

            // Naive approach
            // TODO: Create algorithm to determine best moment to add a user to a queue.

            int playersAdded = 0;
            var match = new MatchGroup();
            var accum = new TempAccumulator();
            while (playersAdded < playersPerMatch)
            {
                var player = unassignedQueue.Dequeue();
                MatchUtils.UpdateMatchCounters(accum, player);
                playersAdded++;
                match.Players.Add(player);
            }

            MatchUtils.UpdateMatchStats(accum, match, playersAdded, currentMatchNumber);

            matches.Add(match);
            WriteMatchToFile(match, hostingEnvironment);
            currentMatchNumber++;
        }


        private static void WriteMatchToFile(MatchGroup match, IHostingEnvironment hostingEnvironment)
        {
            // The file gets temporarily saved into application directory
            string path = hostingEnvironment.ContentRootPath + "/ongoing_match_grouping.txt";
            
            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine(GetMatchAsString(match));
            }
        }

        private static string GetMatchAsString(MatchGroup match)
        {
            var sb = new StringBuilder();
            MatchUtils.AppendMatchToStringBuilder(sb, match);
            return sb.ToString();
        }
    }
}
