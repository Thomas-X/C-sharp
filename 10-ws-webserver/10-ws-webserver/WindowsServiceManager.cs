using System;
using System.Net;

namespace _10_ws_webserver
{
    public class WindowsServiceManager
    {
        public void Start()
        {
            Webserver ws = new Webserver("127.0.0.1", 13000);
            ws.Listen();
        }

        public void Stop()
        {
        }
        
    }
}