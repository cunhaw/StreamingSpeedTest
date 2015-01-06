using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Commands
{
    internal interface IAsyncCommand : IAsyncCommand<object> { }

    internal interface IAsyncCommand<in T> : IRaiseCanExecuteChanged
    {
        Task ExecuteAsync(T pObject);
        bool CanExecute(object pObject);
        ICommand Command { get; }
    }
}
