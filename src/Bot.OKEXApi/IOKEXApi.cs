// // using RestEase;
// using RestEase;
// // using RestEase.SourceGenerator;

// namespace Bot.OKEXApi;

// // [BaseAddress("https://www.okx.com")]
// [BaseAddress("http://localhost")]
// [Header("Content-Type", "application/json;charset=utf-8")]
// public interface IOKEXApi {
	

// 	[Get("/api/v5/trade/orders-pending")]
// 	Task<HttpResponseMessage> GetPendingOrderList(
// 		[Query(nameof(OKEXOrderKeys.instType))]string tradeType,
// 		[Query(nameof(OKEXOrderKeys.instId))]string productID,
// 		[Query(nameof(OKEXOrderKeys.state))]string orderState
// 	);
// }