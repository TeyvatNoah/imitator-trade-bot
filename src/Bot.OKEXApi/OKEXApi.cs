using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

using RestEase;

namespace Bot.OKEXApi;

public sealed class OKEXApi(string apiKey, string secret, string passphrase) {


	#pragma warning disable CS1998
	private readonly IOKEXApi client = RestClient.For<IOKEXApi>(async (request, _) => {
	#pragma warning restore CS1998

		var utcTimeStr = DateTime.UtcNow.ToString("O");
		var preSignedStr = request.Content is null || request.Content.ToString() == "" ?
			$"{utcTimeStr}{request.Method}{request.RequestUri?.PathAndQuery}" :
			$"{utcTimeStr}{request.Method}{request.RequestUri?.PathAndQuery}{request.Content}";
	
		// var encoding = new ASCIIEncoding();
		var encoder = Encoding.ASCII;
		var secretBytes = encoder.GetBytes(secret);
		var preSignedStrBytes = encoder.GetBytes(preSignedStr);
	
		using var hash = new HMACSHA256(secretBytes);
		
		var signedBytes = hash.ComputeHash(preSignedStrBytes);
		var sign = Convert.ToBase64String(signedBytes);
	
		
		request.Headers.Add("OK-ACCESS-KEY", apiKey);
		request.Headers.Add("OK-ACCESS-SIGN", sign);
		request.Headers.Add("OK-ACCESS-TIMESTAMP", utcTimeStr);
		request.Headers.Add("OK-ACCESS-PASSPHRASE", passphrase);
	}); 
// var resp = await okexApi.GetPendingOrderList("FUTURES", "", "partially_filled");
// foreach (var item in resp.Data) {
// 	Console.WriteLine(item.PlatformOrderID);
// }


// var time = DateTime.UtcNow;
// Console.WriteLine(time);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Task<OKEXOrderListResponse> GetPendingOrderList(string tradeType, string productID, string orderState)
		=> client.GetPendingOrderList(tradeType, productID, orderState);
}