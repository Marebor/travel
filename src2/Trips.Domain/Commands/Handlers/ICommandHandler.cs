using System.Threading;
using System.Threading.Tasks;

namespace Trips.Domain.Commands.Handlers
{
    internal interface ICommandHandler { }

    internal interface ICommandHandler<T> : ICommandHandler where T : ICommand
    {
        Task HandleAsync(T command, CancellationToken cancellationToken);
    }
}
