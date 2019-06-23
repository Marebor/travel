using Travel.Common.Cqrs;

namespace Travel.ReadModel.Queries
{
    public class UserTravels : AllTravels, IQuery<Models.Travel>
    {
    }
}
