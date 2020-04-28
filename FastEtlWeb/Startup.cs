using FastData.Core;
using FastEtlWeb.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace FastEtlWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            FastMap.InstanceMap();
            FastMap.InstanceTable("FastEtlWeb.DataModel", "FastEtlWeb.dll");
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            services.AddRazorPages();
            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));

            services.AddMvc(options =>
            {
                options.Filters.Add(new CheckFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddRazorPagesOptions(o =>
            {
                o.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                o.RootDirectory = "/Pages";
                o.Conventions.AddPageRoute("/Data", "");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
             app.UseExceptionHandler(error =>
            {
                error.Use(async (context, next) =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {                       
                         BaseLog.SaveLog(contextFeature.Error.Message, "error");
                        context.Response.ContentType = "application/json;charset=utf-8";
                        context.Response.StatusCode = 200;
                        var result = new Dictionary<string, object>();
                        result.Add("success", false);
                        result.Add("msg", contextFeature.Error.Message);
                        await context.Response.WriteAsync(BaseJson.ModelToJson(result));
                    }                    
                });
            });
            
            app.UseStaticFiles();
            app.UseRouting();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
