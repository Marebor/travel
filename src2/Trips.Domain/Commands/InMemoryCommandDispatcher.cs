using System;
using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Commands.Handlers;

namespace Trips.Domain.Commands
{
    internal sealed class InMemoryCommandDispatcher : ICommandDispatcher
    {
        private readonly Func<Type, ICommandHandler> _handlersFactory;

        public InMemoryCommandDispatcher(Func<Type, ICommandHandler> handlersFactory)
        {
            _handlersFactory = handlersFactory;
        }

        Task ICommandDispatcher.DispatchAsync<T>(T command, CancellationToken cancellationToken)
        {
            var handler = (ICommandHandler<T>)_handlersFactory(typeof(T));

            return handler.HandleAsync(command, cancellationToken);
        }
    }
}
