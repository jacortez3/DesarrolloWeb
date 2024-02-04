using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClinicaMVC.Startup))]
namespace ClinicaMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
