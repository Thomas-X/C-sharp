using System.Collections.Generic;
using System.Web;
using Space_Fridge_Forum.DataStructures;

namespace Space_Fridge_Forum.Services
{

    /// <summary>
    /// This class handles rendering react components (actually just different instances of the same react app)
    /// Singleton pattern used because the react components list is per-request dependent. 
    /// </summary>
    public class ReactComponentRendering
    {
        public List<ReactComponentInstance> ReactComponentInstances = new List<ReactComponentInstance>();
        private ReactComponentRendering() { }

        public static ReactComponentRendering Instance
        {
            get
            {
                if (HttpContext.Current.Items["ReactComponentRendering"] == null)
                    HttpContext.Current.Items["ReactComponentRendering"] = new ReactComponentRendering();
                return (ReactComponentRendering)HttpContext.Current.Items["ReactComponentRendering"];
            }
        }
    }
}