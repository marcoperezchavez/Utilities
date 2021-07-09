using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using IGT.CMS.Microservices.Common.Startup;
using ADIPOS.Resources;
using ADIPOS.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace ADIPOS.Service
{
    public class Startup : CommonStartup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
            :base(configuration)
        {
                this.ServiceName = "POS";
                this.ErrorMessageResourceType = typeof(Resource);
                if (env.IsDevelopment())
                    this.AuthenticationEnabled = false;

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            CommonConfigureServices(services);

            /*** Database Init block ***/
            /*** Comment out if you are not accessing SqlServer ***/
            string dbConn = Configuration.GetValue<string>("DbConn");
            if (!string.IsNullOrEmpty(dbConn))
            {
                services.AddDbContext<ADIPMContext>(options => options.UseSqlServer(dbConn));
            }
            else
            {
                NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
                Logger.Error("Error, missing configuration 'DbConn'");
            }
            /*** Database Init block ***/


            services.AddControllers().AddXmlDataContractSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CommonConfigure(app, env);
        }
    }
}
