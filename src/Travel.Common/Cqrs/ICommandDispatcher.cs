using System.Threading.Tasks;

namespace Travel.Common.Cqrs
{
    public interface ICommandDispatcher
    {
        Task Dispatch<T>(T command) where T : ICommand;
    }
}
