using Server.Configuration;
using Server.Utils;
using System;
using System.IO;

namespace Server
{
    public class Program
    {
        public static void Main(string[] pArgs)
        {
            #region Create a test file

            long vFileSize = 0;
            while (vFileSize == 0)
            {
                try
                {
                    Console.Write("Enter the test file size (MBytes): ");
                    vFileSize = Convert.ToInt64(Console.ReadLine());

                    if (vFileSize > 256)
                    {
                        Console.WriteLine("Maximum file size is 256 MBytes");
                        vFileSize = 0;
                    }
                }
                catch
                {
                    Console.WriteLine("Enter a valid test file size");
                    vFileSize = 0;
                }
            }

            string vTestFilePath = PathResolver.TestFilePath;
            FileInfo vTestFileInfo = new FileInfo(vTestFilePath);

            if (File.Exists(vTestFilePath) && (vTestFileInfo.Length != (vFileSize * 1048576)))
            {
                File.Delete(vTestFilePath);
            }

            if (!File.Exists(vTestFilePath))
            {
                string vDirectoryName = Path.GetDirectoryName(vTestFilePath);
                if (!Directory.Exists(vDirectoryName))
                {
                    Directory.CreateDirectory(vDirectoryName);
                }

                byte[] vArray = new byte[1048576];
                using (FileStream vFileStream = File.Open(vTestFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    byte vByte = 0;
                    while (vByte < vFileSize)
                    {
                        for (int vCount = 0; vCount < vArray.Length; vCount++)
                        {
                            vArray[vCount] = vByte;
                        }

                        vFileStream.Write(vArray, 0, vArray.Length);

                        if (vByte == byte.MaxValue)
                        {
                            break;
                        }
                        else
                        {
                            vByte++;
                        }
                    }
                }
            }

            #endregion

            #region Start the server

            Console.Write("Enter the server host (IP or computer name): ");
            string vHost = Console.ReadLine();

            int vPort = 0;
            while (vPort == 0)
            {
                try
                {
                    Console.Write("Enter the server port: ");
                    vPort = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Enter a valid port number");
                    vPort = 0;
                }
            }
            
            string vBaseAddress = string.Format(@"http://{0}:{1}/", vHost, vPort);
            Console.WriteLine();

            try
            {
                Startup.Start(vBaseAddress);

                Console.WriteLine("The server is running @ {0}", vBaseAddress);
                Console.WriteLine();
                Console.WriteLine("External tests can be executed with calls to:");
                Console.WriteLine(@"{0}StreamingSpeedTest/GetUsingPushStreamContent", vBaseAddress);
                Console.WriteLine(@"{0}StreamingSpeedTest/GetUsingStreamContent", vBaseAddress);
                Console.WriteLine(@"{0}Resources/{1}", vBaseAddress, Path.GetFileName(vTestFilePath));
                Console.WriteLine();
                Console.WriteLine("Press [ENTER] to terminate");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Server start failed");
                Console.WriteLine("Error: {0}", ex.GetType().ToString());
                Console.WriteLine("Message: {0}", ex.Message);
                Console.WriteLine("Stack trace: {0}", ex.StackTrace);
            }

            #endregion
        }
    }
}
