using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(pb_category.Startup))]
namespace pb_category
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
