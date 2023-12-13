using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public sealed class Worker(ILogger<Worker> logger): BackgroundService {
	public readonly ILogger<Worker> _logger = logger;
	protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		while (!stoppingToken.IsCancellationRequested) {
			_logger.LogInformation("Test: {time}", DateTimeOffset.Now);
			await Task.Delay(1000, stoppingToken);
		}
	}
}