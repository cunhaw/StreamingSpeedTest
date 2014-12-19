using System.IO;
using System.Threading.Tasks;

namespace Client.Utils
{
    public static class Extensions
    {
        public static async Task<long> CopyToStreamDoubleBuffered(this Stream pSource, Stream pDestination)
        {
            byte[][] vBuffers = new byte[2][] { new byte[Constants.C_STREAM_COPY_BUFFER_SIZE], new byte[Constants.C_STREAM_COPY_BUFFER_SIZE] };
            int vIndex = 0, vBytesRead;
            long vTotalBytesRead = 0;
            Task vWriteTask = null;

            while (true)
            {
                vBytesRead = await pSource.ReadAsync(vBuffers[vIndex], 0, vBuffers[vIndex].Length).ConfigureAwait(false);
                vTotalBytesRead += vBytesRead;

                if (vWriteTask != null)
                {
                    await vWriteTask.ConfigureAwait(false);
                }

                if (vBytesRead <= 0)
                    break;

                vWriteTask = pDestination.WriteAsync(vBuffers[vIndex], 0, vBytesRead);
                vIndex ^= 1;
            }

            return vTotalBytesRead;
        }
    }
}
