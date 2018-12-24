using GroupingService.Entities;
using GroupingService.Repositories;
using GroupingService.Utils;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GroupingService.Services
{
    public class MatchCreatorService : IMatchCreatorService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public MatchCreatorService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void SetPlayersPerMatch(int playersPerMatch)
        {
            PlayerQueue.SetPlayersPerMatch(playersPerMatch);
        }

        public void AddPlayer(Player player)
        {
            PlayerQueue.AddPlayerToQueue(player, _hostingEnvironment);
        }

        public StringBuilder DownloadAllMatches()
        {
            var sb = new StringBuilder();

            // Get groups
            foreach (var match in PlayerQueue.GetMatches())
            {
                // Parse data
                MatchUtils.AppendMatchToStringBuilder(sb, match);
            }

            return sb;
        }

        public void AssignTopPriorityPlayers()
        {
            // Search through queues for players with top priority
            var queues = PlayerQueue.GetQueues();
            
            var prioritizedPlayers = new Queue<Player>();

            Parallel.ForEach(queues, queue =>
            {
                if (queue.Count > 0)
                {
                    lock (queue)
                    {
                        long waitingTimeInMs = (DateTime.Now.Ticks - queue.Peek().QueueIngressTime) / 10_000;
                        if (waitingTimeInMs > PlayerQueue.PlayerMaxWaitingTimeMs)
                        {
                            // Assign player as top priority, since it's been waiting for too long
                            PlayerQueue.AssignTopPriorityToPlayer(queue.Dequeue());
                        }
                    }
                }
            });

            PlayerQueue.TryCreateMatchWithPriorityPlayers(_hostingEnvironment);
        }
    }
}
