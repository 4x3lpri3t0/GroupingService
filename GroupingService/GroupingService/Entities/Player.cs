using Microsoft.AspNetCore.Mvc;

namespace GroupingService.Entities
{
    public class Player
    {
        public string Name { get; set; }

        public double Skill { get; set; }

        public int Remoteness { get; set; }

        // Milliseconds
        public long WaitingTime { get; set; }
    }
}
