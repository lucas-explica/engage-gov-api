using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EngageGov.API.Services
{
    public class StartupDiagnosticsService : IHostedService
    {
        private readonly ILogger<StartupDiagnosticsService> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public StartupDiagnosticsService(ILogger<StartupDiagnosticsService> logger, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _lifetime = lifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartupDiagnosticsService starting at {time}", DateTime.UtcNow);

            // Attach to lifetime events to log reasons for shutdown
            _lifetime.ApplicationStopping.Register(() =>
            {
                _logger.LogWarning("ApplicationStopping triggered at {time}", DateTime.UtcNow);
            });

            _lifetime.ApplicationStopped.Register(() =>
            {
                _logger.LogInformation("ApplicationStopped triggered at {time}", DateTime.UtcNow);
            });

            // Subscribe to unhandled exceptions to log them
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                try
                {
                    _logger.LogError(e.ExceptionObject as Exception, "Unhandled exception captured by StartupDiagnosticsService");
                }
                catch { }
            };

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StartupDiagnosticsService stopping at {time}", DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}
