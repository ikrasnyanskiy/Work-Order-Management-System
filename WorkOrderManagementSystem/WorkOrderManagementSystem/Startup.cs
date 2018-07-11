using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkOrderManagementSystem.Startup))]
namespace WorkOrderManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
