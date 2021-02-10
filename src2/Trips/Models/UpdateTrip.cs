namespace Trips.Models
{
    public class UpdateTrip
    {
        public UpdateTrip(string? destination, bool? isCancelled, string? owner)
        {
            Destination = destination;
            IsCancelled = isCancelled;
            Owner = owner;
        }

        public string? Destination { get; }
        public bool? IsCancelled { get; }
        public string? Owner { get; }
    }
}
