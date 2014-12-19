using Server.Properties;
using System.IO;

namespace Server.Utils
{
    public static class PathResolver
    {
        public static string TestFilePath
        {
            get
            {
                return string.Format(Resources.TEST_FILE_FORMAT, Directory.GetCurrentDirectory());
            }
        }
    }
}
