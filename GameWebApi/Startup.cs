using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GameWebApi.Startup))]

namespace GameWebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
