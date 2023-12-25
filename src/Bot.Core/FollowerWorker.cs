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
public sealed class FollowerWorker(
	ILogger<PlatformWatcherWorker> logger,
	IHostApplicationLifetime appLifetime,
	IConfiguration config,
	IHostEnvironment env,
	FollowerUser follower,
	Configuration botConfiguration,
	ChannelWrapper<OrderDiffMessage> channelWrapper,
	ConcurrentDictionary<string, Transaction> globalTransactions
): BackgroundService, IHostedLifecycleService {
	private readonly Channel<OrderDiffMessage> _channel = channelWrapper.Channel;
	
	public Task StartingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StartedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("订单跟随Worker已启动.");
		return Task.CompletedTask;
	}

	public Task StoppingAsync(CancellationToken cancellationToken) {
		return Task.CompletedTask;
	}

	public Task StoppedAsync(CancellationToken cancellationToken) {
		logger.LogInformation("订单跟随Worker已停止.");
		return Task.CompletedTask;
	}

	

	protected override async Task ExecuteAsync(CancellationToken cancellationToken) {
		await foreach (var msg in _channel.Reader.ReadAllAsync(cancellationToken)) {
			if (cancellationToken.IsCancellationRequested) {
				// TODO 是直接关闭还是把最后几单同步?
				return;
			}

			try {
				switch (msg.State) {
					case CustomOrderState.New:
						await CreateNewOrders(msg);
						break;
					case CustomOrderState.Modified:
						await ModifyOrders(msg);
						break;
					case CustomOrderState.Canceled:
						await CancelOrders(msg);
						break;
					default:
						throw new NotImplementedException();
				}
			} catch (Exception e) {
				// TODO
				logger.LogError(e, "未知异常");
			}
		}
	}
	
	public async Task CreateNewOrders(OrderDiffMessage msg) {

		var followerOrders = msg.Orders.Select(v => new Order(v)).ForEach(ScalePriceAndSize);
		
		var IDs = (await follower.NewBatchOrder(followerOrders)).ToArray();
		followerOrders.ForEach((v, i) => v.PlatformOrderID = IDs[i]);

		var transactions = followerOrders.Zip(msg.Orders, (f, p) => new Transaction() {
			PlatformOrder = p,
			FollowerOrder = f
		});
		
		transactions.ForEach(v => globalTransactions.TryAdd(v.PlatformOrder.PlatformOrderID, v));
	}

	public async Task ModifyOrders(OrderDiffMessage msg) {

		var transactions = msg.Orders.Select(v => globalTransactions.GetValueOrDefault(v.PlatformOrderID));
		var followerOrders = transactions.Select(v => v!.FollowerOrder);
		
		await follower.ModifyBatchOrders(followerOrders);
		
		// 修改订单完成后修正关联订单价格和数量
		var pair = followerOrders.Zip(msg.Orders, (f, p) => (f, p));
		transactions.Zip(pair, (t, pp) => {
			var (f, p) = pp;
			t!.PlatformOrder.ConsignmentPrice = p.ConsignmentPrice;
			t.PlatformOrder.Size = p.Size;
			t.FollowerOrder.ConsignmentPrice = f.ConsignmentPrice;
			t.FollowerOrder.Size = f.Size;
			return t;
		});
	}

	// 部分成交也撤单,部分成交说明平台也部分成交,它敢撤我为什么不敢撤
	public async Task CancelOrders(OrderDiffMessage msg) {

		var transactions = msg.Orders.Select(v => globalTransactions.GetValueOrDefault(v.PlatformOrderID));
		var cancelOrders = transactions.Select(v => v!.FollowerOrder);
		
		await follower.CancelBatchOrders(cancelOrders);
		// 撤单后修改关联订单状态
		transactions.ForEach(v => {
			v!.State = TransactionState.Finished;
			v.FollowerOrder.CustomState = CustomOrderState.Canceled;
			v.PlatformOrder.CustomState = CustomOrderState.Canceled;
		} );
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ScalePriceAndSize(Order order) {
		order.ConsignmentPrice = order.ConsignmentPrice;
		order.Size *= 2;
		FixPrice(order);
		FixSize(order);
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void FixPrice(Order order) {
		// TODO 根据下单价格精度修正并记录偏差值
		// botConfiguration.PricePrecision;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void FixSize(Order order) {
		// TODO 根据下单数量精度和最小下单数量修正并记录偏差值
		// 需要注意如果修改订单则数量赢大于累计部分成交数量
		// botConfiguration.MinSize;
		// botConfiguration.SizePrecision;
	}
	

	
}
	