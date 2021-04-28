using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IP_Address_Check
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*
             * This block serializes the appsettings.json file, allowing the settings to be applied to the program
             * It uses the custom class found at the bottom of the namespace
             */
            var config = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json").Build();
            var section = config.GetSection(nameof(WorkerOptions));
            var workerOptions = section.Get<WorkerOptions>();

            /*
             * Here's where the serialized appsettings.json gets assigned to variables to be used in the program
             */
            int timerDelay = workerOptions.TimerDelayInSeconds;

            /*
            * This is the main engine of the program
            * A number of strings/collections are created and processed through the classes
            * The end result is a neat text file containing only IP information + log time with none of the original output's excess text
            * 
            * Considering refactoring to have only GetIp called here, with other classes working in the background (I think you may call that abstraction)
            * 
            * timerDelay is determined by the admin in appsettings.json
            */
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                string rawIp = GetIp.Check();
                List<string> ipList = ParseIp.SplitIp(rawIp);
                List<string> relevantIp = ReturnIp.RelevantIp(ipList);
                
                foreach(string item in relevantIp)
                {
                    _logger.LogInformation(item);
                }
                
                await Task.Delay((timerDelay * 1000), stoppingToken);
            }
        }
    }

    public class WorkerOptions
    {
        public int TimerDelayInSeconds { get; set; }
    }
}
