namespace TrackMe.Core.Models
{
    public class Position
    {
        public Position(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double? Accuracy { get; set; }

        public override string ToString()
        {
            return $"{Latitude} {Longitude}";
        }
    }
}
