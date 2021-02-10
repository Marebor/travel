namespace Trips.Models
{
    public class UpdateCustomer
    {
        public UpdateCustomer(string? name, string? owner)
        {
            Name = name;
            Owner = owner;
        }

        public string? Name { get; }
        public string? Owner { get; }
    }
}
