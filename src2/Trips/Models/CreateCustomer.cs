namespace Trips.Models
{
    public class CreateCustomer
    {
        public CreateCustomer(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
