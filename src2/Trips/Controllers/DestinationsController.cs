using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Repositories;

namespace Trips.Controllers
{
    [ApiController]
    [Route("api/destinations")]
    public class DestinationsController : ControllerBase
    {
        private readonly IDestinationRepository _destinationRepository;

        public DestinationsController(IDestinationRepository destinationRepository)
        {
            _destinationRepository = destinationRepository;
        }

        [HttpGet]
        public Task<IReadOnlyCollection<string>> GetDestinations(CancellationToken cancellationToken)
        {
            return _destinationRepository.GetDestinationsAsync(cancellationToken);
        }
    }
}
