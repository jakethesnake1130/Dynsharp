using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using System.IO;
using System.Reflection;

namespace IP_Address_Check
{
    public class Program
    {

        public static void Main(string[] args)
        {
            using (var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"{typeof(Program).Namespace}.readmycontent.txt"))
            using (var reader = new StreamReader(stream))
            {

                var content = reader.ReadToEnd();
                Console.WriteLine(content);
            }
            /*
             * This block serializes the appsettings.json file, allowing the settings to be applied to the program
             * It uses the custom class found at the bottom of the namespace
             */
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(ProgramOptions));
            var programOptions = section.Get<ProgramOptions>();

            /*
             * Here's where the serialized appsettings.json gets assigned to variables to be used in the program
             */
            string filePath = programOptions.LogFilePath;
            string fileName = programOptions.LogFileName;
            string fullFilePath = Path.Combine(filePath, fileName);


            /*
             * This block creates the logger
             * fullFilePath is determined by the admin in apsettings.json
             */
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(fullFilePath)
                .CreateLogger();

            try
            {
                Log.Information("Service begin at: {time}", DateTime.Now);
                CreateHostBuilder(args).Build().Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a fatal error with the service");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseSerilog();
    }

    public class ProgramOptions
    {
        public string LogFilePath { get; set; }
        public string LogFileName { get; set; }
    }
}
