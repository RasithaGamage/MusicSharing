using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MusicSharing.Startup))]
namespace MusicSharing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
