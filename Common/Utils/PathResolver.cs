using System.IO;

namespace Common.Utils
{
    public static class PathResolver
    {
        #region Server paths

        public static string ServerTestFilePath
        {
            get
            {
                return string.Format(Paths.SERVER_TEST_FILE_FORMAT, Directory.GetCurrentDirectory());
            }
        }

        #endregion

        #region Client paths

        public static string ClientTestFilePath
        {
            get
            {
                return string.Format(Paths.CLIENT_TEST_FILE_FORMAT, Directory.GetCurrentDirectory());
            }
        }

        public static string GetClientPushStreamContentUrl(string pHost, int pPort)
        {
            return string.Format(Paths.CLIENT_PUSH_STREAM_CONTENT_FORMAT, pHost, pPort);
        }

        public static string GetClientStreamContentUrl(string pHost, int pPort)
        {
            return string.Format(Paths.CLIENT_STREAM_CONTENT_FORMAT, pHost, pPort);
        }

        public static string GetClientStaticUrl(string pHost, int pPort)
        {
            return string.Format(Paths.CLIENT_STATIC_FORMAT, pHost, pPort);
        }

        #endregion

        #region Common paths

        public static string GetCommonBaseAddress(string pHost, int pPort)
        {
            return string.Format(Paths.COMMON_BASE_ADDRESS, pHost, pPort);
        }

        #endregion
    }
}
