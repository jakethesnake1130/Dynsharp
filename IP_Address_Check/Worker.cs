using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;

namespace IP_Address_Check
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly System.Timers.Timer _timer;
        private const int thirtySeconds = 30000;

        public Worker(ILogger<Worker> logger)
        {

            _logger = logger;
            _timer = new System.Timers.Timer(thirtySeconds);
            _timer.Elapsed += OnIntervalElapsed;
        }

        private void OnIntervalElapsed(object sender, ElapsedEventArgs e)
        {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogInformation(GetIp.Check());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            OnIntervalElapsed(null, null);
            _timer.Start();

            await Task.Run(() => stoppingToken.Register(() => { _timer.Stop(); }));
        }

        public override void Dispose()
        {
            _timer.Dispose();
            base.Dispose();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            return base.StopAsync(cancellationToken);
        }
    }

}
