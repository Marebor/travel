using System;
using System.Collections.Generic;
using System.Linq;
using Travel.Common.Cqrs;

namespace Travel.Common.Model
{
    public class Aggregate
    {
        public Guid Id { get; protected set; }
        public int Version { get; private set; }

        public void ApplyEvent(IEvent e)
        {
            var eventType = e.GetType();
            var applierType = typeof(IApplyEvent<>).MakeGenericType(eventType);
            applierType.GetMethod(nameof(IApplyEvent<IEvent>.Apply))
                .Invoke(this, new object[] { e });

            Version++;
        }

        public static class Builder
        {
            public static T Build<T>(IEnumerable<IEvent> events) where T : Aggregate, new()
            {
                if (events == null || !events.Any())
                    return null;

                var aggregate = new T();
                foreach (var e in events)
                {
                    aggregate.ApplyEvent(e);
                }

                return aggregate;
            }
        }
    }
}
