using System.Collections.Generic;
using Travel.Common.Cqrs;

namespace Travel.ReadModel.Queries
{
    public class UserTravels : AllTravels, IQuery<IEnumerable<Models.Travel>>
    {
    }
}
