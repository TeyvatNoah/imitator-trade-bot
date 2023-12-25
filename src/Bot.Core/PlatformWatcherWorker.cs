namespace Bot.Core;

using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Channels;

using Bot.OKEXApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.Metrics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ZLogger;


// 配置仅允许ConfigWorker修改,没有友元类的实现方式
public sealed class PlatformWatcherWorker(
	ILogger<PlatformWatcherWorker> logger,
	IHostApplicationLifetime appLifetime,
	IConfiguration config,
	IHostEnvironment env,
	PlatformUser platformWatcher,
	Configuration botConfiguration,
	ChannelWrapper<OrderDiffMessage> diffChannelWrapper,
	ChannelWrapper<OrderFinishedMessage> finishChannelWrapper
): BackgroundService, IHostedLifecycleService {
	private readonly Channel<OrderDiffMessage> _diffChannel = diffChannelWrapper.Channel;
	private readonly Channel<OrderFinishedMessage> _finishChannel = finishChannelWrapper.Channel;
	private readonly Dictionary<string, Order> LastUnfilledOrders = [];
	private readonly Dictionary<string, Order> LastPartialFilledOrders = [];
	private readonly Dictionary<string, Order> LastCanceledOrders = [];
	private readonly Dictionary<string, Order> LastFilledOrders = [];
	private readonly Dictionary<string, Order> LastAllOrders = [];
	private readonly List<Order> CurrentUnfilledOrders = [];
	private readonly List<Order> CurrentPartialFilledOrders = [];
	private readonly List<Order> CurrentCanceledOrders = [];
	private readonly List<Order> CurrentFilledOrders = [];
	private readonly List<Order> CurrentAllOrders = [];
	
	public Task StartingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StartedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("平台策略Worker已启动.");
		return Task.CompletedTask;
	}

	public Task StoppingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StoppedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("平台策略Worker已停止.");
		return Task.CompletedTask;
	}

	

	protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
		while (!cancellationToken.IsCancellationRequested) {
			try {
				await GetCurrentAllOrdersSnapshot();
				// 获取新增订单
				var newOrders = GetUnfilledOrdersIncrements();
				newOrders = newOrders
				.Concat(FilterNewPartialFilledOrders())
				.Concat(FilterNewFilledOrders())
				.ForEach(v => v.CustomState = CustomOrderState.New);
				
				await _diffChannel.Writer.WriteAsync(new() {
					Orders = newOrders,
					State = CustomOrderState.New
				});

				// 获取新修改订单
				var modifiedOrders = FilterNewModifiedOrders().ForEach(v => v.CustomState = CustomOrderState.Modified);

				await _diffChannel.Writer.WriteAsync(new() {
					Orders = modifiedOrders,
					State = CustomOrderState.Modified
				});
				
				// 获取新撤单
				var canceledOrders = FilterNewCanceledOrders().ForEach(v => v.CustomState = CustomOrderState.Canceled);
				
				await _diffChannel.Writer.WriteAsync(new() {
					Orders = canceledOrders,
					State = CustomOrderState.Canceled
				});

				// 也通知分析worker
				await _finishChannel.Writer.WriteAsync(new() {
					Orders = canceledOrders,
					State = PlatformOrderState.Canceled
				});
				
				var newFinishedOrders = GetFilledOrdersIncrements();

				await _finishChannel.Writer.WriteAsync(new() {
					Orders = newFinishedOrders,
					State = PlatformOrderState.Filled
				});
				
				CommitAllOrders();
			} catch (Exception e) {
				// TODO
				logger.LogError(e, "未知异常");
			}
			
			// 15s一轮
			await Task.Delay(TimeSpan.FromSeconds(15));
		}
	}
	
	// 必然是新增订单
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> GetUnfilledOrdersIncrements() {
		return GetNewOrders(CurrentUnfilledOrders, LastUnfilledOrders);
	}

	// 可能是新增订单也可能不是
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> GetPartialFilledOrdersIncrements() {
		return GetNewOrders(CurrentPartialFilledOrders, LastPartialFilledOrders);
	}

	// 可能是新增订单也可能不是
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> GetFilledOrdersIncrements() {
		return GetNewOrders(CurrentFilledOrders, LastFilledOrders);
	}

	// 必然是新增撤单同时也可能是新增订单
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> GetCanceledOrdersIncrements() {
		return GetNewOrders(CurrentCanceledOrders, LastCanceledOrders);
	}
	
	private IEnumerable<Order> GetNewOrders(IEnumerable<Order> currentOrders, Dictionary<string, Order> oldOrders) {
		var newOrders = new List<Order>();
		foreach (var order in currentOrders) {
			if (!oldOrders.ContainsKey(order.PlatformOrderID)) {
				newOrders.Add(order);
			}
		}
		return newOrders;
	}
	
	private async Task GetCurrentAllOrdersSnapshot() {
		var t0 = platformWatcher.GetUnfilledOrders(OKEXTradeType.SWAP, botConfiguration.ProductID);
		var t1 = platformWatcher.GetPartialFilledOrders(OKEXTradeType.SWAP, botConfiguration.ProductID);
		var t2 = platformWatcher.GetCanceledOrders(OKEXTradeType.SWAP, botConfiguration.ProductID);
		var t3 = platformWatcher.GetFilledOrders(OKEXTradeType.SWAP, botConfiguration.ProductID);
		
		await Task.WhenAll(t0, t1, t2, t3);
		var newUnfilledOrders = await t0;
		var newPartialOrders = await t1;
		var newCancelOrders = await t2;
		var newFilledOrders = await t3;

		CurrentUnfilledOrders.Clear();
		CurrentPartialFilledOrders.Clear();
		CurrentFilledOrders.Clear();
		CurrentCanceledOrders.Clear();
		CurrentAllOrders.Clear();
		
		CurrentUnfilledOrders.AddRange(newUnfilledOrders);
		CurrentPartialFilledOrders.AddRange(newPartialOrders);
		CurrentFilledOrders.AddRange(newFilledOrders);
		CurrentCanceledOrders.AddRange(newCancelOrders);

		CurrentAllOrders.AddRange(CurrentUnfilledOrders);
		CurrentAllOrders.AddRange(CurrentPartialFilledOrders);
		CurrentAllOrders.AddRange(CurrentFilledOrders);
		CurrentAllOrders.AddRange(CurrentCanceledOrders);
	}
	
	public void CommitAllOrders() {
		LastUnfilledOrders.Clear();
		LastPartialFilledOrders.Clear();
		LastFilledOrders.Clear();
		LastCanceledOrders.Clear();
		LastAllOrders.Clear();
		
		AddManyToDictionary(CurrentUnfilledOrders, LastUnfilledOrders);
		AddManyToDictionary(CurrentPartialFilledOrders, LastPartialFilledOrders);
		AddManyToDictionary(CurrentFilledOrders, LastFilledOrders);
		AddManyToDictionary(CurrentCanceledOrders, LastCanceledOrders);
		AddManyToDictionary(CurrentAllOrders, LastAllOrders);
	}
	
	
	private void AddManyToDictionary(IEnumerable<Order> orders, Dictionary<string, Order> set) {
		foreach (var item in orders) {
			set.Add(item.PlatformOrderID, item);
		}
	}
	
	// 必是新增订单
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> FilterNewPartialFilledOrders() {
		return CurrentPartialFilledOrders.Where(v => !LastUnfilledOrders.ContainsKey(v.PlatformOrderID));
	}

	// 必是新增订单
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> FilterNewFilledOrders() {
		return CurrentFilledOrders.Where(v => !LastUnfilledOrders.ContainsKey(v.PlatformOrderID) && !LastPartialFilledOrders.ContainsKey(v.PlatformOrderID));
	}

	// 获取已修改订单
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> FilterNewModifiedOrders() {
		return CurrentAllOrders
			// 不合并了,没多少
		.Where(v => LastAllOrders.ContainsKey(v.PlatformOrderID))
		// TODO 进一步确认判断条件
		// 可能出现重复提交修改
		.Where(v => v.OrderModifiedTime != v.OrderCreatedTime)
		.Where(v => {
			Order? oldOrder = default;
			LastAllOrders.TryGetValue(v.PlatformOrderID, out oldOrder);
			return v.ConsignmentPrice != oldOrder!.ConsignmentPrice || v.Size != oldOrder.Size;
		});
	}

	// 忽略上一轮未存在的删除订单
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IEnumerable<Order> FilterNewCanceledOrders() {
		return GetCanceledOrdersIncrements().Where(v => LastAllOrders.ContainsKey(v.PlatformOrderID));
	}
	
}
	