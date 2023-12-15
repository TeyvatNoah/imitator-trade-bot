namespace Bot.OKEXApi;


public delegate Task<HttpRequestMessage?> SendAsyncHook(HttpRequestMessage req, CancellationToken cancellationToken);

public delegate HttpRequestMessage? SendSyncHook(HttpRequestMessage req, CancellationToken cancellationToken);

public delegate Task<HttpResponseMessage?> ResponseAsyncHook(HttpResponseMessage res, CancellationToken cancellationToken);

public delegate HttpResponseMessage? ResponseSyncHook(HttpResponseMessage res, CancellationToken cancellationToken);


public sealed class RequestHandler: HttpClientHandler {
	public SendAsyncHook? BeforeSendAsync { get; init; }
	public SendSyncHook? BeforeSend { get; init; }
	public ResponseAsyncHook? AfterResponseAsync { get; init; }
	public ResponseSyncHook? AfterResponse { get; init; }

	
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken cancellationToken) {
		if (cancellationToken.IsCancellationRequested) {
			throw new OperationCanceledException("SendAsync was canceled.");
		}
		HttpRequestMessage? newReq = null;
		if (BeforeSendAsync is not null) {
			newReq = await BeforeSendAsync(req, cancellationToken);
		}

		// 按理这两个互斥, 但简单起见就这样吧
		if (BeforeSend is not null) {
			newReq = BeforeSend(req, cancellationToken);
		}

		var resp = await base.SendAsync(newReq ?? req, cancellationToken);

		HttpResponseMessage? newResp = null;
		if (AfterResponseAsync is not null) {
			newResp = await AfterResponseAsync(resp, cancellationToken);
		}
		
		// 按理这两个互斥, 但简单起见就这样吧
		if (AfterResponse is not null) {
			newResp = AfterResponse(resp, cancellationToken);
		}
		
		return newResp ?? resp;
	}
}
