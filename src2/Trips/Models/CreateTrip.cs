namespace Trips.Models
{
    public class CreateTrip
    {
        public CreateTrip(string destination)
        {
            Destination = destination;
        }

        public string Destination { get; }
    }
}
