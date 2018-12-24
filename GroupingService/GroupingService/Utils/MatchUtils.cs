using GroupingService.Entities;
using System;
using System.Text;
using static System.Math;

namespace GroupingService.Utils
{
    public static class MatchUtils
    {
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

        internal static void UpdateMatchCounters(TempAccumulator accum, Player player)
        {
            long playerWaitingTime = (DateTime.Now.Ticks - player.QueueIngressTime) / 10_000; // Ticks to ms

            accum.minSkill = Min(accum.minSkill, player.Skill);
            accum.maxSkill = Max(accum.maxSkill, player.Skill);
            accum.accumSkill += player.Skill;

            accum.minRemoteness = Min(accum.minRemoteness, player.Remoteness);
            accum.maxRemoteness = Max(accum.maxRemoteness, player.Remoteness);
            accum.accumRemoteness += player.Remoteness;

            accum.minWaitingTime = Min(accum.minWaitingTime, playerWaitingTime);
            accum.maxWaitingTime = Max(accum.maxWaitingTime, playerWaitingTime);
            accum.accumWaitingTime += playerWaitingTime;
        }

        internal static void UpdateMatchStats(TempAccumulator accum, MatchGroup match, int playersAdded, int currentMatchNumber)
        {
            match.MinSkillIndex = accum.minSkill;
            match.MaxSkillIndex = accum.maxSkill;
            match.AvgSkillIndex = accum.accumSkill / playersAdded;
            match.MinRemotenessIndex = accum.minRemoteness;
            match.MaxRemotenessIndex = accum.maxRemoteness;
            match.AvgRemotenessIndex = accum.accumRemoteness / playersAdded;
            match.MinWaitingTime = accum.minWaitingTime;
            match.MaxWaitingTime = accum.maxWaitingTime;
            match.AvgWaitingTime = accum.accumWaitingTime / playersAdded;
            match.MatchNumber = currentMatchNumber;
            match.NumberOfPlayers = playersAdded;
        }
    }
}
