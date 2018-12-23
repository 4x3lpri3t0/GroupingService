using GroupingService.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using static System.Math;

namespace GroupingService.Repositories
{
    // Using static classes like this is horrible, but I needed to come up
    // with something fast in order to advance with the rest of the exercise. Sorry!
    public static class PlayerQueue
    {
        private static Queue<Player> queue = new Queue<Player>();

        private static IList<MatchGroup> matches = new List<MatchGroup>();

        private static int playersPerMatch = 5; // Default

        private static int currentMatchNumber = 1;

        public static bool IsQueueEmpty()
        {
            return queue.Count == 0;
        }

        public static IList<MatchGroup> GetMatches()
        {
            return matches;
        }

        public static void SetPlayersPerMatch(int playersPerGroup)
        {
            playersPerMatch = playersPerGroup;
        }

        internal static void AppendMatchToStringBuilder(StringBuilder sb, MatchGroup group)
        {
            sb.AppendLine($"~===== Match {group.MatchNumber} =====~");
            sb.AppendLine($"Number of players: {group.NumberOfPlayers}");

            sb.AppendLine($"Min. skill index: {group.MinSkillIndex}");
            sb.AppendLine($"Max. skill index: {group.MaxSkillIndex}");
            sb.AppendLine($"Avg. skill index: {group.AvgSkillIndex}");

            sb.AppendLine($"Min. remoteness index: {group.MinRemotenessIndex}");
            sb.AppendLine($"Max. remoteness index: {group.MaxRemotenessIndex}");
            sb.AppendLine($"Avg. remoteness index: {group.AvgRemotenessIndex}");

            sb.AppendLine($"Min. waiting time: {group.MinWaitingTime} ms");
            sb.AppendLine($"Max. waiting time: {group.MaxWaitingTime} ms");
            sb.AppendLine($"Avg. waiting time: {group.AvgWaitingTime} ms");

            // TODO: Also print players in each match group?

            sb.AppendLine();
        }

        public static void AddPlayerToQueue(Player player, IHostingEnvironment hostingEnvironment)
        {
            player.QueueIngressTime = DateTime.Now.Ticks;
            queue.Enqueue(player);

            TryCreateMatch(hostingEnvironment);
        }

        private static void TryCreateMatch(IHostingEnvironment hostingEnvironment)
        {
            if (queue.Count < playersPerMatch)
            {
                return;
            }

            // Naive approach
            // TODO: Create algorithm to determine best moment to add a user to a queue.

            int playersAdded = 0;
            MatchGroup match = new MatchGroup();
            double accumSkill = 0;
            long accumRemoteness = 0;
            long accumWaitingTime = 0;
            double maxSkill = double.MinValue, minSkill = double.MaxValue;
            int maxRemoteness = int.MinValue, minRemoteness = int.MaxValue;
            long maxWaitingTime = long.MinValue, minWaitingTime = long.MaxValue;
            while (playersAdded < playersPerMatch)
            {
                var player = queue.Dequeue();
                var playerWaitingTime = (DateTime.Now.Ticks - player.QueueIngressTime) / 10_000; // Ticks to ms

                minSkill = Min(minSkill, player.Skill);
                maxSkill = Max(maxSkill, player.Skill);
                accumSkill += player.Skill;

                minRemoteness = Min(minRemoteness, player.Remoteness);
                maxRemoteness = Max(maxRemoteness, player.Remoteness);
                accumRemoteness += player.Remoteness;

                minWaitingTime = Min(minWaitingTime, playerWaitingTime);
                maxWaitingTime = Max(maxWaitingTime, playerWaitingTime);
                accumWaitingTime += playerWaitingTime;

                playersAdded++;
                match.Players.Add(player);
            }

            match.MinSkillIndex = minSkill;
            match.MaxSkillIndex = maxSkill;
            match.AvgSkillIndex = accumSkill / playersAdded;
            match.MinRemotenessIndex = minRemoteness;
            match.MaxRemotenessIndex = maxRemoteness;
            match.AvgRemotenessIndex = accumRemoteness / playersAdded;
            match.MinWaitingTime = minWaitingTime;
            match.MaxWaitingTime = maxWaitingTime;
            match.AvgWaitingTime = accumWaitingTime / playersAdded;
            match.MatchNumber = currentMatchNumber;
            match.NumberOfPlayers = playersAdded;

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
            AppendMatchToStringBuilder(sb, match);
            return sb.ToString();
        }
    }
}
