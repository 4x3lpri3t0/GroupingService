using GroupingService.Entities;
using GroupingService.Repositories;
using System.Text;

namespace GroupingService.Services
{
    public class MatchCreatorService
    {
        internal void AddPlayer(Player player)
        {
            PlayerQueue.AddPlayerToQueue(player);
        }

        internal StringBuilder GenerateMatches(int playersPerMatch)
        {
            var sb = new StringBuilder();

            // Get groups
            while (!PlayerQueue.IsQueueEmpty())
            {
                MatchGroup group = PlayerQueue.GetGroup(playersPerMatch);

                // Parse data
                AddMatchToString(sb, group);
            }

            // Reset queue
            PlayerQueue.Reset();

            return sb;
        }

        private void AddMatchToString(StringBuilder sb, MatchGroup group)
        {
            sb.AppendLine($"~===== Match {group.MatchNumber} =====~");
            sb.AppendLine($"Number of players: {group.NumberOfPlayers}");

            // TODO: Statistics

            // TODO: Players in each match group?

            sb.AppendLine();
        }
    }
}
