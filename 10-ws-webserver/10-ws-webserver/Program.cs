using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using System.Net;

namespace _10_ws_webserver
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(hostConfig =>
            {
                hostConfig.Service<WindowsServiceManager>(serviceConfig =>
                {
                    serviceConfig.ConstructUsing(() => new WindowsServiceManager());
                    serviceConfig.WhenStarted(s => s.Start());
                    serviceConfig.WhenStopped(s => s.Stop());
                });
            });
        }
    }
}
