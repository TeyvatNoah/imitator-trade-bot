using System.Security.Cryptography;
using System.Text;


namespace Bot.OKEXApi;


public sealed class OKEXApi(string apiKey, string secret, string passphrase)
{
	public const string BaseAddress = "https://www.okx.com";

	private readonly Request _client = new(new HttpClient(new RequestHandler() {
		BeforeSend = (req, cancellationToken) =>
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return null;
			}
			var utcTimeStr = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
			var preSignedStr = req.Content is null || req.Content.ToString() == "" ?
				$"{utcTimeStr}{req.Method}{req.RequestUri?.PathAndQuery}" :
				$"{utcTimeStr}{req.Method}{req.RequestUri?.PathAndQuery}{req.Content}";

			// var encoding = new ASCIIEncoding();
			var encoder = Encoding.ASCII;
			var secretBytes = encoder.GetBytes(secret);
			var preSignedStrBytes = encoder.GetBytes(preSignedStr);

			using var hash = new HMACSHA256(secretBytes);

			var signedBytes = hash.ComputeHash(preSignedStrBytes);
			var sign = Convert.ToBase64String(signedBytes);


			req.Headers.Add("OK-ACCESS-KEY", apiKey);
			req.Headers.Add("OK-ACCESS-SIGN", sign);
			req.Headers.Add("OK-ACCESS-TIMESTAMP", utcTimeStr);
			req.Headers.Add("OK-ACCESS-PASSPHRASE", passphrase);
			return null;
		},
		AfterResponse = (resp, cancellationToken) => {
			// TODO HTTP code和okex code处理
			return null;
		}
	}));
	
	public OKEXApi(string apiKey, string secret, string passphrase, Request client): this(apiKey, secret, passphrase) {
		_client = client;
	}
	

	// 获取未完成订单列表
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/orders-pending";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}


	// 获取系统状态
	public async Task<SystemStatusResponse?> GetSystemStatus(CancellationToken cancellationToken) {
		var path = "/api/v5/system/status";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage();
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, SystemStatusResponseContext.Default.SystemStatusResponse, cancellationToken);
	}

	// 获取持仓信息
	public async Task<PositionsResponse?> GetPositions(string tradeType, CancellationToken cancellationToken) {
		var path = "/api/v5/account/positions";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, PositionResponseContext.Default.PositionsResponse, cancellationToken);
	}
	
	// 获取历史持仓信息
	public async Task<HistoryPositionsResponse?> GetHistoryPositions(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/positions-history";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, HistoryPositionResponseContext.Default.HistoryPositionsResponse, cancellationToken);
	}
	
	// TODO
	// 查看账户余额
	public async Task<OKEXOrderListResponse?> GetAccountInfo(string currency, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/balance";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.ccy), currency },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	
	// TODO
	// 查看账户配置
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/config";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 设置持仓模式
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-position-mode";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 设置杠杆倍数
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-leverage";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取最大可开仓数量
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/max-size";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取最大可用数量
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/max-avail-size";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 调整保证金
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/position/margin-balance";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}

	// TODO
	// 获取杠杆倍数
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/leverage-info";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 调整保证金
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/position/margin-balance";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 逐仓交易设置
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-isolated-mode";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 设置账户模式
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-account-level";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 下单
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 撤单
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/cancel-order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 批量撤单
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/cancel-batch-orders";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 修改订单
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/amend-order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 批量修改订单
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/amend-batch-orders";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}

	// TODO
	// 获取订单信息
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取历史订单,七天
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/orders-history";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取指数行情
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/market/index-tickers";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取系统时间
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/public/time";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取标记价格
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/public/mark-price";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取永续当前资金费率
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/public/funding-rate";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 获取法币汇率
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/market/exchange-rate";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// TODO
	// 批量修改订单
	public async Task<OKEXOrderListResponse?> GetPendingOrderList(string tradeType, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/amend-batch-orders";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			}
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
}