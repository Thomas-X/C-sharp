using System.Web;

namespace Space_Fridge_Forum.Services
{
    // Since normal HtmlString truncates our string
    public class HtmlStringHack : IHtmlString
    {
        public string value { get; set; }

        public HtmlStringHack(string str)
        {
            value = str;
        }

        public string ToHtmlString()
        {
            return value;
        }

        public override string ToString()
        {
            return value;
        }
    }
}