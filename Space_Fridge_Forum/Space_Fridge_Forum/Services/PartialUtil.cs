using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Space_Fridge_Forum.Services
{
    /// <summary>
    /// Stolen from https://stackoverflow.com/a/8016267/7153772
    /// the this keyword in front of the HtmlHelper type is used as an extension method of the type,
    /// this way a method can be added to the @Html class without recompiling it in the CLR. (is my understanding)
    /// this is super cool.
    /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
    /// </summary>
    public static class PartialUtil
    {
        public static string RequireScript(this HtmlHelper html, string path, int priority = 1)
        {
            if (!(HttpContext.Current.Items["RequiredScripts"] is List<ResourceInclude> requiredScripts))
                HttpContext.Current.Items["RequiredScripts"] = requiredScripts = new List<ResourceInclude>();
            if (requiredScripts.All(i => i.Path != path))
                requiredScripts.Add(new ResourceInclude() {Path = path, Priority = priority});
            return null;
        }

        public static HtmlString EmitRequiredScripts(this HtmlHelper html)
        {
            if (!(HttpContext.Current.Items["RequiredScripts"] is List<ResourceInclude> requiredScripts)) return null;
            var sb = new StringBuilder();
            foreach (var item in requiredScripts.OrderByDescending(i => i.Priority))
            {
                sb.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>\n", item.Path);
            }

            return new HtmlString(sb.ToString());
        }

        public class ResourceInclude
        {
            public string Path { get; set; }
            public int Priority { get; set; }
        }

        public static IEnumerable<string> SplitBy(this string str, int chunkLength)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength);
            }
        }
    }
}