using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Web.Http;

namespace Server.Configuration
{
    public class Startup
    {
        public void Configuration(IAppBuilder pAppBuilder)
        {
            HttpConfiguration vConfig = new HttpConfiguration();

            Config.Register(vConfig);

            pAppBuilder.UseWebApi(vConfig);

            string vStaticFilesDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Resources";
            
            StaticFileOptions vStaticFileOptions = new StaticFileOptions();
            vStaticFileOptions.FileSystem = new PhysicalFileSystem(@".\Resources");
            vStaticFileOptions.RequestPath = new PathString("/Resources");
            vStaticFileOptions.ServeUnknownFileTypes = true;

            pAppBuilder.UseStaticFiles(vStaticFileOptions);
        }

        public static IDisposable Start(string pBaseAddress)
        {
            return WebApp.Start<Startup>(pBaseAddress);
        }
    }
}
