using GroupingService.Entities;
using GroupingService.Utils;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GroupingService.Repositories
{
    // Using static classes like this is horrible, but I needed to come up
    // with something fast in order to advance with the rest of the exercise. Sorry!
    public static class PlayerQueue
    {
        // TODO: Use ConcurrentQueue
        private static Queue<Player> LowSkillLowRemoQueue = new Queue<Player>();
        private static Queue<Player> LowSkillHighRemoQueue = new Queue<Player>();
        private static Queue<Player> HighSkillLowRemoQueue = new Queue<Player>();
        private static Queue<Player> HighSkillHighRemoQueue = new Queue<Player>();

        private static IList<MatchGroup> matches = new List<MatchGroup>();

        // Default - Unless specified in model through website
        // TODO: Move to config file
        private static int playersPerMatch = 2;

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

            // Decide in which queue should player go
            Queue<Player> queue = new Queue<Player>();
            if (player.Skill < 0.5)
            {
                if (player.Remoteness < 50)
                    queue = LowSkillLowRemoQueue;
                else
                    queue = LowSkillHighRemoQueue;
            }
            else
            {
                if (player.Remoteness < 50)
                    queue = HighSkillLowRemoQueue;
                else
                    queue = HighSkillHighRemoQueue;
            }

            // Add the current player to the respective queue
            queue.Enqueue(player);

            // Attempt to create a match
            TryCreateMatch(queue, hostingEnvironment);
        }

        private static void TryCreateMatch(Queue<Player> queue, IHostingEnvironment hostingEnvironment)
        {
            // Naive approach
            // TODO: Create algorithm to determine best moment to add a user to a queue.
            lock (queue)
            {
                if (queue.Count < playersPerMatch)
                {
                    return;
                }

                int playersAdded = 0;
                var match = new MatchGroup();
                var accum = new TempAccumulator();
                while (playersAdded < playersPerMatch)
                {
                    var player = queue.Dequeue();
                    MatchUtils.UpdateMatchCounters(accum, player);
                    playersAdded++;
                    match.Players.Add(player);
                }

                MatchUtils.UpdateMatchStats(accum, match, playersAdded, currentMatchNumber);

                matches.Add(match);
                WriteMatchToFile(match, hostingEnvironment);
                currentMatchNumber++;
            }
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
