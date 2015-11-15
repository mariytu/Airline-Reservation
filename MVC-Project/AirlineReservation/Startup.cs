using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AirlineReservation.Startup))]
namespace AirlineReservation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
