using FastData.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FastData.Core.Repository;
using FastUntility.Core;

namespace FastEtlService.core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FastMap.InstanceTable("FastEtlWeb.DataModel", "FastEtlService.core.dll");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseSystemd()
                .ConfigureServices((hostContext, services) =>{
                    services.AddHostedService<Worker>();
                    services.AddSingleton<IFastRepository, FastRepository>();
                    ServiceContext.Init(new ServiceEngine(services.BuildServiceProvider()));
                });
    }
}
