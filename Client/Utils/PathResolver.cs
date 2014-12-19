using Client.Properties;
using System.IO;

namespace Client.Utils
{
    internal static class PathResolver
    {
        public static string TestFilePath
        {
            get
            {
                return string.Format(Resources.TEST_FILE_FORMAT, Directory.GetCurrentDirectory());
            }
        }

        public static string GetPushStreamContentUrl(string pHost, int pPort)
        {
            return string.Format(Resources.PUSH_STREAM_CONTENT_FORMAT, pHost, pPort);
        }

        public static string GetStreamContentUrl(string pHost, int pPort)
        {
            return string.Format(Resources.STREAM_CONTENT_FORMAT, pHost, pPort);
        }

        public static string GetStaticUrl(string pHost, int pPort)
        {
            return string.Format(Resources.STATIC_FORMAT, pHost, pPort);
        }
    }
}
