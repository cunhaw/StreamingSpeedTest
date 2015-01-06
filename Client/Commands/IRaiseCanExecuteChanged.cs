using System.Windows.Input;

namespace Client.Commands
{
    internal interface IRaiseCanExecuteChanged
    {
        void RaiseCanExecuteChanged();
    }

    internal static class CommandExtensions
    {
        public static void RaiseCanExecuteChanged(this ICommand pCommand)
        {
            IRaiseCanExecuteChanged vCanExecuteChanged = pCommand as IRaiseCanExecuteChanged;

            if (vCanExecuteChanged != null)
            {
                vCanExecuteChanged.RaiseCanExecuteChanged();
            }
        }
    }
}
