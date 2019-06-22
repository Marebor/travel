using System;
using System.Threading.Tasks;
using Travel.Common.Cqrs;

namespace Travel.Infrastructure.Messaging
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly Func<Type, ICommandHandler> handlersFactory;

        public CommandDispatcher(Func<Type, ICommandHandler> handlersFactory)
        {
            this.handlersFactory = handlersFactory;
        }

        public Task Dispatch<T>(T command) where T : ICommand
        {
            ICommandHandler<T> handler = handlersFactory(typeof(T)) as ICommandHandler<T>;

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for command: {command.GetType().Name} is not registred.");
            }

            return handler.Handle(command);
        }
    }
}
