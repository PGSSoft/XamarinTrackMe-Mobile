namespace TrackMe.Core.Models
{
    public class PossibleTime
    {
        public PossibleTime(int minutes)
        {
            Minutes = minutes;
        }
        public int Minutes { get; }

        public override string ToString() => Minutes == 1 ? $"{Minutes} minute" : $"{Minutes} minutes";
    }
}