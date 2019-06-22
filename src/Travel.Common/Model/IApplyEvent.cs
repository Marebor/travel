using Travel.Common.Cqrs;

namespace Travel.Common.Model
{
    public interface IApplyEvent
    {
    }

    public interface IApplyEvent<T> : IApplyEvent where T : IEvent
    {
        void Apply(T @event);
    }
}
