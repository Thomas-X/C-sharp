using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _10_ws_webserver.DataStructures
{
    public class HttpRequest
    {
        public const string SupportedHttpVersion = "HTTP/1.1";
        public string HttpVersion = SupportedHttpVersion;
        public string RawHeaders;
        public string RawBody;
        public HttpRequestType Type;
        public string Path;
        public Dictionary<string, string> Headers = new Dictionary<string, string>();
        private string[] lines;


        public HttpRequest(string unparsedHeaders, string unparsedBody)
        {
            RawHeaders = unparsedHeaders;

            if (unparsedBody.Trim().Length == 0)
            {
                Console.WriteLine("Empty request body");
            }
            else
            {
                RawBody = unparsedBody;
                // TODO try to parse form-data, json, etc..
            }

            lines = ParseLines();


            ParseFirstLine();
            ParseHeaders();
        }

        public void ParseHeaders()
        {
            // TODO add a cookie parser
            string[] headers = lines.Skip(1).ToArray();
            
            foreach (var header in headers)
            {
                string[] values = header.Split(new char[] {':'}, 2)
                    .Select(s => s.Trim())
                    .ToArray();
                Headers[values[0]] = values[1];
            }
        }

        public string[] ParseLines()
        {
            return RawHeaders.Split(new[] {"\r\n"}, StringSplitOptions.None);
        }

        public void ParseFirstLine()
        {
            string[] firstLine = lines[0].Split(' ');
            SetRequestType(firstLine[0].ToUpper());

            // Set path of request
            Path = firstLine[1];

            // Check HTTP version
            if (firstLine[2].ToUpper() != SupportedHttpVersion)
            {
                // TODO better error handling here if invalid HTTP version
                throw new NotImplementedException();
            }
        }

        public void SetRequestType(string rawRequestType)
        {
            HttpRequestType httpRequestType = new HttpRequestType();

            switch (rawRequestType)
            {
                case HttpRequestTypeValues.Get:
                    httpRequestType.GET = HttpRequestTypeValues.Get;
                    break;
                case HttpRequestTypeValues.Post:
                    httpRequestType.POST = HttpRequestTypeValues.Post;
                    break;
                case HttpRequestTypeValues.Patch:
                    httpRequestType.PATCH = HttpRequestTypeValues.Patch;
                    break;
                case HttpRequestTypeValues.Delete:
                    httpRequestType.DELETE = HttpRequestTypeValues.Delete;
                    break;
                case HttpRequestTypeValues.Options:
                    httpRequestType.OPTIONS = HttpRequestTypeValues.Options;
                    break;
                case HttpRequestTypeValues.Head:
                    httpRequestType.HEAD = HttpRequestTypeValues.Head;
                    break;
                case HttpRequestTypeValues.Put:
                    httpRequestType.PUT = HttpRequestTypeValues.Put;
                    break;
            }

            Type = httpRequestType;
        }
    }
}