using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Repositories;

namespace Trips.Infrastructure
{
    internal sealed class RestCountriesComDestinationRepository : IDestinationRepository
    {
        private readonly HttpClient _httpClient;

        public RestCountriesComDestinationRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        async Task<IReadOnlyCollection<string>> IDestinationRepository.GetDestinationsAsync(CancellationToken cancellationToken)
        {
            var countries = await GetCountriesAsync(cancellationToken);

            return countries.Select(country => country.Name).ToArray();
        }

        async Task<bool> IDestinationRepository.DestinationExistsAsync(string destination, CancellationToken cancellationToken)
        {
            var countries = await GetCountriesAsync(cancellationToken);

            return countries.Any(country => country.Name == destination);
        }

        private async Task<IReadOnlyCollection<Country>> GetCountriesAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("/v2/all?fields=name", cancellationToken);
            var content = await response
                .EnsureSuccessStatusCode()
                .Content
                .ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<IReadOnlyCollection<Country>>(content);

            return countries;
        }

        private class Country
        {
            public Country(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }
    }
}
