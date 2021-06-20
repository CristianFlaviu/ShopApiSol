using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ShopApi.Extensions
{
    public static class LoggingExtension
    {
        public static void ConfigureLogging(this IApplicationBuilder app, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
        }
    }
}
