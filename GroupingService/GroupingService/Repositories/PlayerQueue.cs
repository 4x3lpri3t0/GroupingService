using GroupingService.Entities;
using GroupingService.Utils;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GroupingService.Repositories
{
    // Using static classes like this is horrible, but I needed to come up
    // with something fast in order to advance with the rest of the exercise.
    // I wanted to change it to be a Singleton or something a bit more ideal, sorry!
    public static class PlayerQueue
    {
        // Threshold which, if surpassed, we try to assign the 
        // player to any match as soon as possible
        public static int PlayerMaxWaitingTimeMs = 5_000; // 5 seconds

        // TODO: Have a more dynamic way to handle skill/remoteness balance
        private static readonly Queue<Player> lowSkillLowRemoQueue = new Queue<Player>();
        private static readonly Queue<Player> lowSkillHighRemoQueue = new Queue<Player>();
        private static readonly Queue<Player> highSkillLowRemoQueue = new Queue<Player>();
        private static readonly Queue<Player> highSkillHighRemoQueue = new Queue<Player>();
        
        // TODO: Make it a custom priority queue so it reorganizes its oldest players internally
        private static readonly Queue<Player> topPriorityQueue = new Queue<Player>();

        private static IList<MatchGroup> matches = new List<MatchGroup>();

        // Default - Unless specified through website in first page
        private static int playersPerMatch = 2;

        private static int currentMatchNumber = 1;

        public static IList<Queue<Player>> GetQueues()
        {
            var queues = new List<Queue<Player>>();
            queues.Add(lowSkillLowRemoQueue);
            queues.Add(lowSkillHighRemoQueue);
            queues.Add(highSkillLowRemoQueue);
            queues.Add(highSkillHighRemoQueue);
            return queues;
        }

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
                    queue = lowSkillLowRemoQueue;
                else
                    queue = lowSkillHighRemoQueue;
            }
            else
            {
                if (player.Remoteness < 50)
                    queue = highSkillLowRemoQueue;
                else
                    queue = highSkillHighRemoQueue;
            }

            // Add the current player to the respective queue
            queue.Enqueue(player);

            // Attempt to create a match
            TryCreateMatch(queue, hostingEnvironment);
        }

        public static void AssignTopPriorityToPlayer(Player player)
        {
            topPriorityQueue.Enqueue(player);
        }

        public static void TryCreateMatchWithPriorityPlayers(IHostingEnvironment hostingEnvironment)
        {
            lock (topPriorityQueue)
            {
                if (topPriorityQueue.Count < playersPerMatch)
                {
                    return;
                }

                CreateMatch(topPriorityQueue, hostingEnvironment);
            }
        }

        // TODO: Create a better algorithm to determine optimal moment to add a user to a match.
        private static bool TryCreateMatch(Queue<Player> queue, IHostingEnvironment hostingEnvironment)
        {
            lock (queue)
            {
                if (queue.Count + topPriorityQueue.Count < playersPerMatch)
                {
                    return false;
                }

                CreateMatch(queue, hostingEnvironment);

                return true;
            }
        }

        private static void CreateMatch(Queue<Player> queue, IHostingEnvironment hostingEnvironment)
        {
            int playersAdded = 0;
            var match = new MatchGroup();
            var accumulator = new TempAccumulator();

            // 1) Add players from priority queue (if any)
            while (playersAdded < playersPerMatch && topPriorityQueue.Count > 0)
            {
                var player = topPriorityQueue.Dequeue();
                playersAdded = AddPlayerToMatch(playersAdded, match, accumulator, player);
            }

            // 2) Add players from proper queue
            while (playersAdded < playersPerMatch)
            {
                var player = queue.Dequeue();
                playersAdded = AddPlayerToMatch(playersAdded, match, accumulator, player);
            }

            MatchUtils.UpdateMatchStats(accumulator, match, playersAdded, currentMatchNumber);

            matches.Add(match);
            WriteMatchToFile(match, hostingEnvironment);
            currentMatchNumber++;
        }

        private static int AddPlayerToMatch(int playersAdded, MatchGroup match, TempAccumulator accumulator, Player player)
        {
            MatchUtils.UpdateMatchCounters(accumulator, player);
            playersAdded++;
            match.Players.Add(player);
            return playersAdded;
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
