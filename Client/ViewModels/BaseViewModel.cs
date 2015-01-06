using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] String pPropertyName = "")
        {
            OnPropertyChanged(new PropertyChangedEventArgs(pPropertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs pArgs)
        {
            var vHandler = this.PropertyChanged;

            if (vHandler != null)
            {
                vHandler(this, pArgs);
            }
        }
    }
}
