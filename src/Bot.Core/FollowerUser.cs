namespace Bot.Core;

using System.Runtime.CompilerServices;

using Bot.OKEXApi;

// 需要两个OKEXAPI单例(那还能叫单例吗?)
// 虽然蠢了点,但这样可以work
public sealed class FollowerUser(string AppKey, string Secret, string Passphrase): OKEXApi(AppKey, Secret, Passphrase) {
	public readonly OrderMapper Mapper = new();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task SetAccountPositionMode(string mode) {
		await SetAccountPositionMode(new() {
			PosMode = mode
		}, new(false));
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task SetAccountMode(string mode) {
		await SetAccountMode(new() {
			Mode = mode
		}, new(false));
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task SetIsolatedMode(string mode) {
		await SetIsolatedMode(new() {
			IsolatedMode = mode,
			Type = TradeType.CONTRACTS
		}, new(false));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task SetAccountLeverage(string productID, string mode, string? posSide, float lever) {
		await SetAccountLeverage(new() {
			ProductID = productID,
			Leverage = lever,
			PosSide = posSide,
			TradeMode = mode
		}, new(false));
	}
	
	public async Task<IEnumerable<string>> NewBatchOrder(IEnumerable<Order> orders) {
		var ordersArg = orders.Select(Mapper.OrderToOKEXOrderArg).ToArray();
		var resp = await NewBatchOrder(ordersArg, new(false));
		return resp?.Data.Select(v => v.PlatformOrderID) ?? throw new NullReferenceException("下单异常.");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task ModifyBatchOrders(IEnumerable<Order> orders) {
		var ordersArg = orders.Select(Mapper.OrderToModifiedOKEXOrderArg).ToArray();
		await ModifyBatchOrders(ordersArg, new(false));
		// return resp?.Data.Select(v => v.PlatformOrderID) ?? throw new NullReferenceException("修改订单异常.");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task CancelBatchOrders(IEnumerable<Order> orders) {
		var ordersArg = orders.Select(v => new CancelOrderArgDto() {
			ProductID = v.ProductID!,
			PlatformOrderID = v.PlatformOrderID
		}).ToArray();
		await CancelBatchOrders(ordersArg, new(false));
		// return resp?.Data.Select(v => v.PlatformOrderID) ?? throw new NullReferenceException("修改订单异常.");
	}
}