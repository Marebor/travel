using System.Threading.Tasks;

namespace Travel.Common.Cqrs
{
    public interface IEventHandler
    {
    }

    public interface IEventHandler<T> : IEventHandler where T : IEvent
    {
        Task Handle(T @event);
    }
}
