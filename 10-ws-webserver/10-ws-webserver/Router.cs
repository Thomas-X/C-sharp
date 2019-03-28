using System;
using System.IO;
using System.Linq;
using System.Text;
using _10_ws_webserver.DataStructures;

namespace _10_ws_webserver
{
    /// <summary>
    /// 
    /// </summary>
    public class Router
    {
        private HttpRequest request;

        public Router(HttpRequest request)
        {
            this.request = request;
        }

        public byte[] Render()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string path = "";

//            string[] dirs = Directory.GetDirectories("./public", "*", SearchOption.AllDirectories);

            // TODO EXTREMELY DANGEROUS. TODO BETTER ESCAPING / MAPPING VALUES
            if (request.Path.IndexOf("..", StringComparison.Ordinal) != -1)
            {
                path = "./public/index.html";
            }
            else if (File.Exists($"./public{request.Path}"))
            {
                path = $"./public{request.Path}";
            }
            else
            {
                path = $"./public/404.html";
            }

            byte[] content = File.ReadAllBytes(path);

            // TODO map all MIME types
            // https://gist.github.com/aksakalli/9191056#file-simplehttpserver-cs-L24

            string mimeType = "text/html";
            if (Path.GetExtension(path) == ".png")
            {
//                mimeType = "image/x-icon";
//                mimeType = "application/octet-stream";
                mimeType = "image/png";
            }

            // Build headers
            stringBuilder.Append("HTTP 1.1\r\n");
            stringBuilder.Append("Server: Team-1337\r\n");
            stringBuilder.Append($"Content-Type: {mimeType}\r\n");
            stringBuilder.Append("Accept-Ranges: bytes\r\n");
            stringBuilder.Append($"Content-Length: {content.Length}\r\n\r\n");
            // Change encoding based on file type
            byte[] msg = Encoding.UTF8.GetBytes(stringBuilder.ToString());
            return msg.Concat(content).ToArray();
        }
    }
}