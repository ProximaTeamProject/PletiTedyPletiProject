using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PletiTedyPleti.Startup))]
namespace PletiTedyPleti
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
