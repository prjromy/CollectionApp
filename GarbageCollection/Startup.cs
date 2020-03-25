using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GarbageCollection.Startup))]
namespace GarbageCollection
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
