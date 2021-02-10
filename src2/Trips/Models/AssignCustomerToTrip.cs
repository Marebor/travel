namespace Trips.Models
{
    public class AssignCustomerToTrip
    {
        public AssignCustomerToTrip(int customerId)
        {
            CustomerId = customerId;
        }

        public int CustomerId { get; }
    }
}
