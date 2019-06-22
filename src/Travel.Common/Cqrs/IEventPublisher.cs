using System.Threading.Tasks;

namespace Travel.Common.Cqrs
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
