using Common.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Models
{
    internal class DownloadStatusEventArgs : EventArgs
    {
        public string Status { get; set; }
    }

    internal class MainWindowModel : IDisposable
    {
        private HttpClient fHttpClient;
        private DownloadStatusEventArgs fStatusEventArgs;
        private long fSize;

        public string Host { get; set; }
        public int Port { get; set; }
        public string BaseAddress
        {
            get
            {
                return PathResolver.GetCommonBaseAddress(Host, Port);
            }
        }
        public TimeSpan PushStreamContentTimeSpan { get; private set; }
        public long PushStreamContentSize { get; private set; }
        public TimeSpan StreamContentTimeSpan { get; private set; }
        public long StreamContentSize { get; private set; }
        public TimeSpan StaticTimeSpan { get; private set; }        
        public long StaticSize { get; private set; }
        public event EventHandler<DownloadStatusEventArgs> OnDownloadStatus;

        public MainWindowModel()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.Expect100Continue = false;

            HttpClientHandler vHttpClientHandler = new HttpClientHandler();
            vHttpClientHandler.Proxy = null;
            vHttpClientHandler.UseProxy = false;

            fHttpClient = new HttpClient(vHttpClientHandler);
            fHttpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            fStatusEventArgs = new DownloadStatusEventArgs();

            PushStreamContentTimeSpan = TimeSpan.Zero;
            PushStreamContentSize = 0;
            StreamContentTimeSpan = TimeSpan.Zero;
            StreamContentSize = 0;
            StaticTimeSpan = TimeSpan.Zero;
            StaticSize = 0;
        }

        public void Dispose()
        {
            if (fHttpClient != null)
            {
                fHttpClient.CancelPendingRequests();
                fHttpClient.Dispose();
            }
        }

        private void RaiseDownloadEvent()
        {
            EventHandler<DownloadStatusEventArgs> vEventHandler = OnDownloadStatus;
            if (vEventHandler != null)
            {
                vEventHandler(this, fStatusEventArgs);
            }
        }

        private async Task<TimeSpan> DownloadFile(Uri pDownloadUrl)
        {
            HttpRequestMessage vRequest = new HttpRequestMessage(HttpMethod.Get, pDownloadUrl);
            HttpResponseMessage vResponse = await fHttpClient.SendAsync(vRequest, HttpCompletionOption.ResponseHeadersRead);
            Stopwatch vStopwatch = new Stopwatch();
            vStopwatch.Reset();

            try
            {
                if (vResponse.IsSuccessStatusCode)
                {
                    using (Stream vContentStream = await vResponse.Content.ReadAsStreamAsync())
                    {
                        using (FileStream vFileStream = File.Create(PathResolver.ClientTestFilePath, Constants.C_STREAM_COPY_BUFFER_SIZE))
                        {
                            vStopwatch.Start();
                            fSize = await vContentStream.CopyToStreamDoubleBuffered(vFileStream);
                            vStopwatch.Stop();
                        }
                    }
                }
            }
            finally
            {
                vResponse.Dispose();
                vRequest.Dispose();
            }

            return vStopwatch.Elapsed;
        }

        public async Task DownloadFiles()
        {
            fStatusEventArgs.Status = "Downloading file via push stream content";
            RaiseDownloadEvent();

            Uri vDownloadUrl = new Uri(PathResolver.GetClientPushStreamContentUrl(Host, Port));
            PushStreamContentTimeSpan = await DownloadFile(vDownloadUrl);
            PushStreamContentSize = fSize;

            fStatusEventArgs.Status = "Downloading file via stream content";
            RaiseDownloadEvent();

            vDownloadUrl = new Uri(PathResolver.GetClientStreamContentUrl(Host, Port));
            StreamContentTimeSpan = await DownloadFile(vDownloadUrl);
            StreamContentSize = fSize;

            fStatusEventArgs.Status = "Downloading file via static link";
            RaiseDownloadEvent();

            vDownloadUrl = new Uri(PathResolver.GetClientStaticUrl(Host, Port));
            StaticTimeSpan = await DownloadFile(vDownloadUrl);
            StaticSize = fSize;

            fStatusEventArgs.Status = "Downloading finished";
            RaiseDownloadEvent();

            if (File.Exists(PathResolver.ClientTestFilePath))
            {
                File.Delete(PathResolver.ClientTestFilePath);
            }
        }
    }
}
