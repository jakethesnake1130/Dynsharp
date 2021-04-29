using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace Dynsharp
{
    internal class GetIp
    {
        public static string Check()
        {
            /*
             * This block serializes the appsettings.json file, allowing the settings to be applied to the program
             * It uses the custom class found at the bottom of the namespace
             */
            var config = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(GetIpOptions));
            var getIpOptions = section.Get<GetIpOptions>();

            /*
             * Here's where the serialized appsettings.json gets assigned to variables to be used in the program
             */
            string fileName = getIpOptions.FileName;

            /*
             * This block generates the IP information output
             */
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            /*
             * The output generated above is stored in its raw entirety to a string and then returned
             */
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }

    public class GetIpOptions
    {
        public string FileName { get; set; }
    }
}