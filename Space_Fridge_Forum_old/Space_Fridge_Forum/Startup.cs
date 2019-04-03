using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Space_Fridge_Forum.Startup))]
namespace Space_Fridge_Forum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
