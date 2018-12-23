using GroupingService.Entities;
using GroupingService.Repositories;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace GroupingService.Services
{
    public class MatchCreatorService
    {
        internal void SetPlayersPerMatch(int playersPerMatch)
        {
            PlayerQueue.SetPlayersPerMatch(playersPerMatch);
        }

        internal void AddPlayer(Player player, IHostingEnvironment hostingEnvironment)
        {
            PlayerQueue.AddPlayerToQueue(player, hostingEnvironment);
        }

        internal StringBuilder DownloadAllMatches()
        {
            var sb = new StringBuilder();

            // Get groups
            foreach (var match in PlayerQueue.GetMatches())
            {
                // Parse data
                PlayerQueue.AppendMatchToStringBuilder(sb, match);
            }

            return sb;
        }
    }
}
