using System.Threading;
using System.Threading.Tasks;

namespace Trips.Common
{
    public interface IEventHandler { }

    public interface IEventHandler<T> : IEventHandler where T : IEvent
    {
        Task HandleAsync(T @event, CancellationToken cancellationToken);
    }
}
