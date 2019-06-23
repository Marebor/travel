using System.Collections.Generic;
using System.Threading.Tasks;
using Travel.Common.Auth;
using Travel.Common.Cqrs;
using Travel.Common.Storage;
using Travel.ReadModel.Queries;

namespace Travel.ReadModel.Handlers
{
    public class UserTravelsHandler : IQueryHandler<UserTravels, IEnumerable<Models.Travel>>
    {
        private readonly IQueryableStore<Models.Travel> store;
        private readonly IIdentityProvider identityProvider;

        public UserTravelsHandler(IQueryableStore<Models.Travel> store, IIdentityProvider identityProvider)
        {
            this.store = store;
            this.identityProvider = identityProvider;
        }

        public Task<IEnumerable<Models.Travel>> Handle(UserTravels query)
        {
            return store.Query(x => x.Owner == identityProvider.GetIdentity().Username, x => x);
        }
    }
}
