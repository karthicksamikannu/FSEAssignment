using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_20MVCEFAssignment.Startup))]
namespace _20MVCEFAssignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
