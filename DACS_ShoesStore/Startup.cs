using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DACS_ShoesStore.Startup))]
namespace DACS_ShoesStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
