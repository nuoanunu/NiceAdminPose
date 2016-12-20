using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NicePose.Startup))]
namespace NicePose
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
