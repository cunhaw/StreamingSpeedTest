using Client.Commands;
using Client.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel, IDisposable
    {
        private MainWindowModel fModel;
        private string fStatus;
        private readonly string C_TIMESPAN_FORMAT = @"hh\:mm\:ss\.fff";

        public string Host
        {
            get
            {
                return fModel.Host;
            }

            set
            {
                fModel.Host = value;
            }
        }
        public int Port
        {
            get
            {
                return fModel.Port;
            }

            set
            {
                fModel.Port = value;
            }
        }
        public string Status
        {
            get
            {
                return fStatus;
            }

            private set
            {
                fStatus = value;
                RaisePropertyChanged();
            }
        }
        public string PushStreamContentTimeSpan
        {
            get
            {
                return fModel.PushStreamContentTimeSpan.ToString(C_TIMESPAN_FORMAT);
            }
        }
        public string PushStreamContentSpeed
        {
            get
            {
                double vSpeed = (fModel.PushStreamContentSize / 1048576) / fModel.PushStreamContentTimeSpan.TotalSeconds;
                return string.Format("{0:0.##} MBytes / sec", vSpeed);
            }
        }
        public string StreamContentTimeSpan
        {
            get
            {
                return fModel.StreamContentTimeSpan.ToString(C_TIMESPAN_FORMAT);
            }
        }
        public string StreamContentSpeed
        {
            get
            {
                double vSpeed = (fModel.StreamContentSize / 1048576) / fModel.StreamContentTimeSpan.TotalSeconds;
                return string.Format("{0:0.##} MBytes / sec", vSpeed);
            }
        }
        public string StaticTimeSpan
        {
            get
            {
                return fModel.StaticTimeSpan.ToString(C_TIMESPAN_FORMAT);
            }
        }
        public string StaticSpeed
        {
            get
            {
                double vSpeed = (fModel.StaticSize / 1048576) / fModel.StaticTimeSpan.TotalSeconds;
                return string.Format("{0:0.##} MBytes / sec", vSpeed);
            }
        }

        public ICommand DownloadFiles { get; private set; }

        public MainWindowViewModel()
        {
            fModel = new MainWindowModel();
            fModel.OnDownloadStatus += OnDownloadStatusHandler;

            DownloadFiles = new AwaitableDelegateCommand(ExecDownloadFiles);
        }

        public void Dispose()
        {
            fModel.OnDownloadStatus -= OnDownloadStatusHandler;
            fModel.Dispose();
        }

        private void OnDownloadStatusHandler(object pModel, DownloadStatusEventArgs pArgs)
        {
            Status = pArgs.Status;

            RaisePropertyChanged("PushStreamContentTimeSpan");
            RaisePropertyChanged("PushStreamContentSpeed");

            RaisePropertyChanged("StreamContentTimeSpan");
            RaisePropertyChanged("StreamContentSpeed");

            RaisePropertyChanged("StaticTimeSpan");
            RaisePropertyChanged("StaticSpeed");
        }

        private async Task ExecDownloadFiles()
        {
            await fModel.DownloadFiles();
        }
    }
}
