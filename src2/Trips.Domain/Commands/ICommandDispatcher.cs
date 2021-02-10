using System.Threading;
using System.Threading.Tasks;

namespace Trips.Domain.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<T>(T command, CancellationToken cancellationToken) where T : ICommand;
    }
}
