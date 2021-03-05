using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LeaveManagementSystem.Startup))]
namespace LeaveManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
