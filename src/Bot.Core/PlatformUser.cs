namespace Bot.Core;

using System.Collections;
using System.Linq.Expressions;

using Bot.OKEXApi;

// 需要两个OKEXAPI单例(那还能叫单例吗?)
// 虽然蠢了点,但这样可以work
public sealed class PlatformUser(string APIKey, string Secret, string Passphrase): OKEXApi(APIKey, Secret, Passphrase) {
	private readonly OrderMapper Mapper = new();
	public async Task<AccountConfigurationDto> GetAccountConfiguration() {
		var resp = await GetAccountConfiguration(new(false));
		// TODO null异常处理
		return resp!.Data[0];
	}
	
	public async Task<LeverageInfoDto> GetAccountLeverage(string productID, string tradeMode) {
		var resp = await GetAccountLeverage(productID, tradeMode, new(false));
		return resp!.Data[0];
	}

	public async Task<InstrumentsDto> GetInstruments(string productID, string tradeType) {
		var resp = await GetInstruments(tradeType, productID, new(false));
		return resp!.Data[0];
	}

	public async Task<IEnumerable<Order>> GetUnfilledOrders(string tradeType, string productID) {
		var len = 0;
		string? lastID = null;
		List<Order> orders = [];

		do {
			var resp = await GetPendingOrderList(tradeType, productID, OKEXOrderState.Unfilled, lastID, new(false));
			var result = resp?.Data.Select(Mapper.OKEXOrderDtoToOrder)
			.ForEach(v => {
				v.PlatformState = PlatformOrderState.Unfilled;
				v.CustomState = CustomOrderState.New;
			});
			orders.AddRange(result ?? []);
			len = orders.Count;
			lastID = orders.LastOrDefault()?.PlatformOrderID;
		} while (len == 100);
		
		return orders;
	}

	public async Task<IEnumerable<Order>> GetPartialFilledOrders(string tradeType, string productID) {
		var len = 0;
		string? lastID = null;
		List<Order> orders = [];

		do {
			var resp = await GetPendingOrderList(tradeType, productID, OKEXOrderState.PartialFilled, lastID, new(false));
			var result = resp?.Data.Select(Mapper.OKEXOrderDtoToOrder)
			.ForEach(v => {
				v.PlatformState = PlatformOrderState.PartialFilled;
				v.CustomState = CustomOrderState.New;
			});
			orders.AddRange(result ?? []);
			len = orders.Count;
			lastID = orders.LastOrDefault()?.PlatformOrderID;
		} while (len == 100);
		
		return orders;
	}

	public async Task<IEnumerable<Order>> GetCanceledOrders(string tradeType, string productID) {
		var resp = await GetHistoryOrderList(tradeType, productID, OKEXOrderState.Canceled, new(false));
		var orders = resp?.Data.Select(Mapper.OKEXOrderDtoToOrder)
			.ForEach(v => {
				v.PlatformState = PlatformOrderState.Canceled;
				v.CustomState = CustomOrderState.Canceled;
			}) ?? [];
		return orders;
	}

	public async Task<IEnumerable<Order>> GetFilledOrders(string tradeType, string productID) {
		var resp = await GetHistoryOrderList(tradeType, productID, OKEXOrderState.Filled, new(false));
		var orders = resp?.Data.Select(Mapper.OKEXOrderDtoToOrder)
			.ForEach(v => {
				v.PlatformState = PlatformOrderState.Filled;
				v.CustomState = CustomOrderState.New;
			}) ?? [];
		return orders;
	}
}