using Travel.Common.Cqrs;

namespace Travel.ReadModel.Queries
{
    public class AllTravels : IQuery<Models.Travel>
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
