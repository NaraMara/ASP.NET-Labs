using System.IO;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Backend4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TelemetryDebugWriter.IsTracingDisabled = true;
            Program.BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
