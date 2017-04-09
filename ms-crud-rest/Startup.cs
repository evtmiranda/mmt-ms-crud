using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ms_crud_rest.Startup))]
namespace ms_crud_rest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}