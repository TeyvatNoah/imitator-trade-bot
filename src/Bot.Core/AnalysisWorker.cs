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
public sealed class AnalysisWorker(
	ILogger<PlatformWatcherWorker> logger,
	IHostApplicationLifetime appLifetime,
	IConfiguration config,
	IHostEnvironment env,
	FollowerUser follower,
	Configuration botConfiguration,
	ConcurrentDictionary<string, Transaction> globalTransaction
): BackgroundService, IHostedLifecycleService {
	
	public Task StartingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StartedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("分析Worker已启动.");
		return Task.CompletedTask;
	}

	public Task StoppingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StoppedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("分析Worker已停止.");
		return Task.CompletedTask;
	}

	

	protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
		while (!cancellationToken.IsCancellationRequested) {
			try {
				foreach (var (_, transaction) in globalTransaction) {
					var pOrder = transaction.PlatformOrder;
					var fOrder = transaction.FollowerOrder;

					if (IsTransactionFinished(transaction)) {
						globalTransaction.TryRemove(transaction.PlatformOrder.PlatformOrderID, out _);
					} else if (IsOrderFinished(transaction.PlatformOrder)) {
						// TODO
					}
					// 两个都没完成没什么需要做的
				}
				
				await Task.Delay(TimeSpan.FromSeconds(15));
			} catch (Exception e) {
				// TODO
				logger.LogError(e, "未知异常");
			}
		}
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsOrderFinished(Order o) {
		return o.PlatformState == PlatformOrderState.Canceled || o.PlatformState == PlatformOrderState.Filled;
	}
	
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsTransactionFinished(Transaction t) {
		return IsOrderFinished(t.PlatformOrder) && IsOrderFinished(t.FollowerOrder);
	}
	
}
	