using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSAT.Startup))]
namespace CSAT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
