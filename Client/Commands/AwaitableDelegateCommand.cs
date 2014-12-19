using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Commands
{
    internal class AwaitableDelegateCommand : AwaitableDelegateCommand<object>, IAsyncCommand
    {
        public AwaitableDelegateCommand(Func<Task> pExecute) : base(c => pExecute()) { }

        public AwaitableDelegateCommand(Func<Task> pExecute, Func<bool> pCanExecute) : base(c => pExecute(), c => pCanExecute()) { }
    }

    internal class AwaitableDelegateCommand<T> : IAsyncCommand<T>, ICommand
    {
        private readonly Func<T, Task> fExecute;
        private readonly DelegateCommand<T> fDelegateCommand;
        private bool fIsExecuting;

        public ICommand Command { get { return this; } }

        public event EventHandler CanExecuteChanged
        {
            add { fDelegateCommand.CanExecuteChanged += value; }
            remove { fDelegateCommand.CanExecuteChanged -= value; }
        }

        public AwaitableDelegateCommand(Func<T, Task> pExecute) : this(pExecute, _ => true) { }

        public AwaitableDelegateCommand(Func<T, Task> pExecute, Func<T, bool> pCanExecute)
        {
            fExecute = pExecute;
            fDelegateCommand = new DelegateCommand<T>(c => { }, pCanExecute);
        }

        public bool CanExecute(object pParameter)
        {
            return !fIsExecuting && fDelegateCommand.CanExecute((T)pParameter);
        }

        public async void Execute(object pParameter)
        {
            await ExecuteAsync((T)pParameter);
        }

        public void RaiseCanExecuteChanged()
        {
            fDelegateCommand.RaiseCanExecuteChanged();
        }

        public async Task ExecuteAsync(T pObject)
        {
            try
            {
                fIsExecuting = true;
                RaiseCanExecuteChanged();
                await fExecute(pObject);
            }
            finally
            {
                fIsExecuting = false;
                RaiseCanExecuteChanged();
            }
        }
    }
}
