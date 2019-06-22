using System.Collections.Generic;
using Travel.Common.Cqrs;

namespace Travel.ReadModel.Queries
{
    public class AllTravels : IQuery<IEnumerable<Models.Travel>>
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
