using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupingService.Entities
{
    public class MatchGroup
    {
        public int MatchNumber { get; set; }

        public int NumberOfPlayers { get; set; }

        public double MinSkillIndex { get; set; }

        public double MaxSkillIndex { get; set; }

        public double AvgSkillIndex { get; set; }

        public int MinRemotenessIndex { get; set; }

        public int MaxRemotenessIndex { get; set; }

        public double AvgRemotenessIndex { get; set; }

        public long MinWaitingTime { get; set; }

        public long MaxWaitingTime { get; set; }

        public long AvgWaitingTime { get; set; }

        public IList<Player> Players { get; set; }
    }
}
