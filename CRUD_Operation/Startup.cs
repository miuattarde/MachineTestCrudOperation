using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRUD_Operation.Startup))]
namespace CRUD_Operation
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
