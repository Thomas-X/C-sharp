using System;
using System.Net;

namespace _10_ws_webserver
{
    /// <summary>
    /// The class that handles the windows service life-cycles
    /// </summary>
    public class WindowsServiceManager
    {
        /// <summary>
        /// On start of the windows service
        /// </summary>
        public void Start()
        {
            Webserver ws = new Webserver("127.0.0.1", 13000);
            ws.Listen();
        }

        /// <summary>
        /// On stop of the windows service
        /// </summary>
        public void Stop()
        {
        }
        
    }
}