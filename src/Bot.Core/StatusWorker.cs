namespace Bot.Core;

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

using Bot.OKEXApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZLogger;


// 配置仅允许ConfigWorker修改,没有友元类的实现方式
public sealed class StatusWorker(
	ILogger<PlatformWatcherWorker> logger,
	IHostApplicationLifetime appLifetime,
	IConfiguration config,
	IHostEnvironment env,
	FollowerUser follower,
	Configuration botConfiguration,
	ChannelWrapper<OrderFinishedMessage> channelWrapper,
	ConcurrentDictionary<string, Transaction> globalTransactions
): BackgroundService, IHostedLifecycleService {
	private readonly Channel<OrderFinishedMessage> _channel = channelWrapper.Channel;
	
	public Task StartingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StartedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("状态维护Worker已启动.");
		return Task.CompletedTask;
	}

	public Task StoppingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StoppedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("状态维护Worker已停止.");
		return Task.CompletedTask;
	}

	

	protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
		await foreach (var msg in _channel.Reader.ReadAllAsync(cancellationToken)) {
			if (cancellationToken.IsCancellationRequested) {
				// TODO 是直接关闭还是把最后几单状态也更新? 更新个JB
				return;
			}
			
			foreach (var o in msg.Orders) {
				var transaction = globalTransactions.GetValueOrDefault(o.PlatformOrderID);
				transaction!.PlatformOrder.PlatformState = o.PlatformState;
				transaction!.PlatformOrder.CustomState = o.CustomState;
			}
			
		}
	}
	
}
	