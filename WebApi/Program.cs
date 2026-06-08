using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var _logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config", optional: false).GetCurrentClassLogger();

            try
            {
                _logger.Debug("Before Creating Web Host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, conf) =>
            {

            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
            })
            .UseNLog();
    }
}
