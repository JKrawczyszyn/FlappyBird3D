using System;

namespace Entry.Models
{
    [Serializable]
    public class Score
    {
        public int value;
        public long timeTicks;
        public string name;

        public Score(int value, DateTime time, string name)
        {
            this.value = value;
            timeTicks = time.Ticks;
            this.name = name;
        }
    }
}
