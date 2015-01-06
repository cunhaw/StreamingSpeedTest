using System;
using System.Windows.Input;

namespace Client.Commands
{
    internal class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action pExecute) : base(c => pExecute()) { }

        public DelegateCommand(Action pExecute, Func<bool> pCanExecute) : base(c => pExecute(), c => pCanExecute()) { }
    }

    internal class DelegateCommand<T> : ICommand, IRaiseCanExecuteChanged
    {
        private readonly Func<T, bool> fCanExecute;
        private readonly Action<T> fExecute;
        private bool fIsExecuting;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DelegateCommand(Action<T> pExecute) : this(pExecute, null) { }

        public DelegateCommand(Action<T> pExecute, Func<T, bool> pCanExecute)
        {
            if ((pExecute == null) && (pCanExecute == null))
            {
                throw new ArgumentNullException("pExecute", "Execute Method cannot be null");
            }

            fExecute = pExecute;
            fCanExecute = pCanExecute;
        }

        bool ICommand.CanExecute(object pParameter)
        {
            return !fIsExecuting && CanExecute((T)pParameter);
        }

        void ICommand.Execute(object pParameter)
        {
            fIsExecuting = true;

            try
            {
                RaiseCanExecuteChanged();
                Execute((T)pParameter);
            }
            finally
            {
                fIsExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public bool CanExecute(T pParameter)
        {
            if (fCanExecute == null)
            {
                return true;
            }

            return fCanExecute(pParameter);
        }

        public void Execute(T pParameter)
        {
            fExecute(pParameter);
        }
    }
}
