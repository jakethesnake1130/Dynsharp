using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace Dynsharp
{
    internal class GetIp
    {
        private readonly IConfiguration Configuration;

        public GetIp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Check()
        {
            /*
             * This block serializes the appsettings.json file, allowing the settings to be applied to the program
             * It uses the custom class found at the bottom of the namespace
             */

            /*
             * Here's where the serialized appsettings.json gets assigned to variables to be used in the program
             */
            string fileName = Configuration["GetIpOptions:FileName"];

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
}