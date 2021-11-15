using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Serilog;
using Serilog.Events;
using System.IO;
using System.Security;

namespace MailBookApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.File(@"C:\Users\arrak\source\NeighborOS\MailBookApi\Logs\Error.txt", LogEventLevel.Error, rollingInterval: RollingInterval.Day)
                        .WriteTo.File(@"C:\Users\arrak\source\NeighborOS\MailBookApi\Logs\Info.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                        .WriteTo.Console()
                        .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected Web Host Error.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    var isDevelopment = environment == Environments.Development;
                    var appsettingsFile = "appsettings.json";

                    if (isDevelopment)
                    {
                        appsettingsFile = "appsettings.Development.json";
                    }

                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile(appsettingsFile)
                        .Build();

                    var certificatePath = configuration.GetSection("Certificates:Default").Value.ToString();

                    webBuilder
                        .UseSerilog()
                        .ConfigureKestrel(listenOption =>
                        {
                            listenOption.ListenAnyIP(8900, opt =>
                            {
                                var certPfxFileBytes = File.ReadAllBytes(certificatePath);
                                var securePfxPasswordStr = new SecureString();
                                foreach (var passwordChar in "vkiydKN8986".ToCharArray())
                                {
                                    securePfxPasswordStr.AppendChar(passwordChar);
                                }

                                var cert = new X509Certificate2(certPfxFileBytes, securePfxPasswordStr);
                                opt.UseHttps(cert);
                            });
                        })
                        .UseStartup<Startup>();
                });
    }
}
