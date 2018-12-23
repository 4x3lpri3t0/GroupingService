using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupingService.Utils
{
    public class TempAccumulator
    {
        public int maxRemoteness { get; internal set; }
        public int minRemoteness { get; internal set; }
        public long maxWaitingTime { get; internal set; }
        public long minWaitingTime { get; internal set; }
        public double accumSkill { get; internal set; }
        public long accumRemoteness { get; internal set; }
        public long accumWaitingTime { get; internal set; }
        public double maxSkill { get; internal set; }
        public double minSkill { get; internal set; }

        public TempAccumulator()
        {
            maxSkill = double.MinValue;
            minSkill = double.MaxValue;
            maxRemoteness = int.MinValue;
            minRemoteness = int.MaxValue;
            maxWaitingTime = long.MinValue;
            minWaitingTime = long.MaxValue;
            accumSkill = 0;
            accumRemoteness = 0;
            accumWaitingTime = 0;
        }
    }
}
