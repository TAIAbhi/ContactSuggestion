using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ContactSuggestion.Startup))]
namespace ContactSuggestion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
