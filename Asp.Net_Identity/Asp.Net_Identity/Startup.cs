using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Asp.Net_Identity.Startup))]
namespace Asp.Net_Identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
