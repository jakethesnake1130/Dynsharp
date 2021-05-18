using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dynsharp
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _config = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            /*
             * Here's where the value from appsettings.json gets assigned to a variable
             */
            int timerDelay = _config.GetValue<int>("WorkerOptions:TimerDelayInSeconds");
            string fileName = _config["GetIpOptions:FileName"];

            /*
            * This is the main engine of the program
            * A number of strings/collections are created and processed through the classes
            * The end result is a neat text file containing only IP information + log time with none of the original output's excess text
            * 
            * Considering refactoring to have only GetIp called here, with other classes working in the background (I think you may call that abstraction)
            * 
            * timerDelay is determined by the admin in appsettings.json
            */
            GetIp getIp = new GetIp(_config);

            while (!stoppingToken.IsCancellationRequested)
            {
                List<string> ipList = new List<string>();
                List<string> relevantIp = new List<string>();

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                string rawIp = getIp.Check();

                switch (fileName)
                {
                    case "ipconfig":
                        ipList = ParseIp.WindowsParse(rawIp);
                        relevantIp = ReturnIp.WindowsReturn(ipList);

                        break;

                    case "ip addr show":
                        ipList = ParseIp.LinuxParse(rawIp);
                        relevantIp = ReturnIp.LinuxReturn(ipList);
                        break;

                    default:
                        break;
                }
                
                foreach(string item in relevantIp)
                {
                    _logger.LogInformation(item);
                }
                
                await Task.Delay((timerDelay * 1000), stoppingToken);
            }
        }
    }
}
