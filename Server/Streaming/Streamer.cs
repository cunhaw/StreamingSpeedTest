using Common.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Server.Streaming
{
    internal class Streamer
    {
        public Func<Stream, HttpContent, TransportContext, Task> StreamWriter { get; private set; }

        public Streamer()
        {
            StreamWriter = WriteToStream;
        }

        private async Task WriteToStream(Stream pStream, HttpContent pContent, TransportContext pContext)
        {
            try
            {
                string vTestFilePath = PathResolver.ServerTestFilePath;
                long vFileSize;
                int vBufferSize = Constants.C_STREAM_COPY_BUFFER_SIZE, vTotalBytesRead, vBufferBytesRead, vBytesRead;
                byte[] vBuffer = new byte[vBufferSize];
                
                using (FileStream vFileStream = File.Open(vTestFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    vFileSize = vFileStream.Length;
                    vTotalBytesRead = 0;

                    do
                    {
                        vBufferBytesRead = 0;

                        while (vBufferBytesRead < vBufferSize)
                        {
                            vBytesRead = await vFileStream.ReadAsync(vBuffer, vBufferBytesRead, vBufferSize - vBufferBytesRead);

                            if (vBytesRead == 0)
                            {
                                break;
                            }
                            else
                            {
                                vBufferBytesRead += vBytesRead;
                            }
                        }

                        vTotalBytesRead += vBufferBytesRead;

                        await pStream.WriteAsync(vBuffer, 0, vBufferBytesRead);
                    }
                    while (vTotalBytesRead < vFileSize);
                }
            }
            finally
            {
                pStream.Close();
            }
        }
    }
}
