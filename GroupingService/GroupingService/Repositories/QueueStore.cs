using GroupingService.Entities;
using System.Collections.Generic;

namespace GroupingService.Repositories
{
    // Using static classes like this is horrible, but I needed to come up
    // with something fast in order to advance with the rest of the exercise. Sorry!
    // This simulates a Queue service.
    public static class PlayerQueue
    {
        private static Queue<Player> Queue = new Queue<Player>();

        private static int MatchNumber = 1;

        public static bool IsQueueEmpty()
        {
            return Queue.Count == 0;
        }

        public static void AddPlayerToQueue(Player player)
        {
            Queue.Enqueue(player);
        }

        public static MatchGroup GetGroup(int playersPerMatch)
        {
            
            var matchGroup = new MatchGroup()
            {
                MatchNumber = MatchNumber,
                // TODO
                Players = new List<Player>()
            };

            // TODO:
            // 1- Prioritize the ones waiting longer
            // 2- Factor in remoteness
            // 3- Factor in skill
            int playersAdded = 0;
            while (!IsQueueEmpty() && playersAdded < playersPerMatch)
            {
                var player = Queue.Dequeue();
                matchGroup.Players.Add(player);

                playersAdded++;
            }

            matchGroup.NumberOfPlayers = playersAdded;

            MatchNumber++; // TODO: Atomic operation in case of multi-threading

            return matchGroup;
        }

        public static void Reset()
        {
            MatchNumber = 1;
            Queue = new Queue<Player>();
        }
    }
}
