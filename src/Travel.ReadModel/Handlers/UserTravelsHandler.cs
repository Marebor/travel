using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Common.Cqrs;
using Travel.Common.ErrorHandling;
using Travel.Common.ErrorHandling.Exceptions;
using Travel.Common.Storage;
using Travel.ReadModel.Queries;

namespace Travel.ReadModel.Handlers
{
    public class UserTravelsHandler : IQueryHandler<UserTravels, IEnumerable<Models.Travel>>
    {
        private readonly IQueryableStore<Models.Travel> store;

        public UserTravelsHandler(IQueryableStore<Models.Travel> store)
        {
            this.store = store;
        }

        public Task<IEnumerable<Models.Travel>> Handle(UserTravels query)
        {
            if (string.IsNullOrWhiteSpace(query.User))
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(query.User));
            }

            return store.Query(x => x.Owner == query.User, x => x);
        }
    }
}
