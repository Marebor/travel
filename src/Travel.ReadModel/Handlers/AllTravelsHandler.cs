using System.Collections.Generic;
using System.Threading.Tasks;
using Travel.Common.Auth;
using Travel.Common.Cqrs;
using Travel.Common.Storage;
using Travel.ReadModel.Queries;

namespace Travel.ReadModel.Handlers
{
    public class AllTravelsHandler : IQueryHandler<AllTravels, Models.Travel>
    {
        private readonly IQueryableStore<Models.Travel> store;
        private readonly IIdentityProvider identityProvider;

        public AllTravelsHandler(IQueryableStore<Models.Travel> store, IIdentityProvider identityProvider)
        {
            this.store = store;
            this.identityProvider = identityProvider;
        }

        public Task<IEnumerable<Models.Travel>> Handle(AllTravels query)
        {
            return store.Query(x => true, x => x);
        }
    }
}
