using System.Security.Cryptography;
using System.Text;


namespace Bot.OKEXApi;


public sealed class OKEXApi(string apiKey, string secret, string passphrase)
{
	public const string BaseAddress = "https://www.okx.com";

	private readonly Request _client = new() {
		AfterSendDeserializeSyncHookNoReturn = (content, req, cancellationToken) => {
			if (cancellationToken.IsCancellationRequested) {
				return;
			}

			var utcTimeStr = DateTime.UtcNow.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffK");
			var preSignedStr = string.IsNullOrEmpty(req.Content?.ToString()) ?
				$"{utcTimeStr}{req.Method}{req.RequestUri!.PathAndQuery}" :
				$"{utcTimeStr}{req.Method}{req.RequestUri!.PathAndQuery}{content}";

			// var encoding = new ASCIIEncoding();
			var secretBytes = Encoding.ASCII.GetBytes(secret);
			var preSignedStrBytes = Encoding.UTF8.GetBytes(preSignedStr);

			using var hash = new HMACSHA256(secretBytes);

			var signedBytes = hash.ComputeHash(preSignedStrBytes);
			var sign = Convert.ToBase64String(signedBytes);


			req.Headers.Add("OK-ACCESS-KEY", apiKey);
			req.Headers.Add("OK-ACCESS-SIGN", sign);
			req.Headers.Add("OK-ACCESS-TIMESTAMP", utcTimeStr);
			req.Headers.Add("OK-ACCESS-PASSPHRASE", passphrase);
		},
		AfterResponseDeserializeSyncHookNoReturn = (content, req, res, cancellationToken) => {
			// TODO 如果接口是起始配置接口则需要抛出异常
			Console.WriteLine((content as IOKEXResponse)?.Code);
		}
	};
	// new(new HttpClient(new RequestHandler() {
	// 	BeforeSend = (req, cancellationToken) => {
	// 	},
	// 	AfterResponse = (resp, cancellationToken) => {
	// 		// TODO HTTP code和okex code处理
	// 		Console.WriteLine(resp.StatusCode);
	// 		return null;
	// 	}
	// }));
	
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
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), orderState },
			})
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
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, PositionResponseContext.Default.PositionsResponse, cancellationToken);
	}
	
	// 获取历史持仓信息
	public async Task<HistoryPositionsResponse?> GetHistoryPositions(string tradeType, string MgnMode, CancellationToken cancellationToken) {
		var path = "/api/v5/account/positions-history";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.state), MgnMode },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, HistoryPositionResponseContext.Default.HistoryPositionsResponse, cancellationToken);
	}
	
	// 查看账户余额
	public async Task<BalanceResponse?> GetAccountBalance(string currency, CancellationToken cancellationToken) {
		var path = "/api/v5/account/balance";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.ccy), currency },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, BalanceResponseContext.Default.BalanceResponse, cancellationToken);
	}
	
	// 查看账户配置
	public async Task<AccountConfigurationResponse?> GetAccountConfiguration(CancellationToken cancellationToken) {
		var path = "/api/v5/account/config";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage();
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, AccountConfigurationResponseContext.Default.AccountConfigurationResponse, cancellationToken);
	}
	// 设置持仓模式
	public async Task<DontCareAboutBodyResponse?> SetAccountPositionMode(AccountPositionModeDto positionMode, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-position-mode";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<AccountPositionModeDto>() {
			Content = positionMode
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg, AccountPositionModeContext.Default.AccountPositionModeDto, DontCareAboutBodyResponseContext.Default.DontCareAboutBodyResponse, cancellationToken);
	}
	// 设置杠杆倍数
	public async Task<DontCareAboutBodyResponse?> SetAccountLeverage(AccountLeverageDto leverage, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-leverage";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<AccountLeverageDto>() {
			Content = leverage
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg, AccountLeverageContext.Default.AccountLeverageDto, DontCareAboutBodyResponseContext.Default.DontCareAboutBodyResponse, cancellationToken);
	}

	// 获取最大可开仓数量
	public async Task<AccountMaxSizeResponse?> GetAccountMaxSize(string productID, string tradeMode, double price, CancellationToken cancellationToken) {
		var path = "/api/v5/account/max-size";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new()
		};
		reqMsg.Query
			.Add(nameof(OKEXOrderKeys.instId), productID)
			.Add(nameof(OKEXOrderKeys.tdMode), tradeMode)
			.Add(nameof(OKEXOrderKeys.px), price);
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, AccountMaxSizeResponseContext.Default.AccountMaxSizeResponse, cancellationToken);
	}
	// // TODO
	// // 获取最大可用数量
	public async Task<AccountAvailableMaxSizeResponse?> GetAccountMaxAvailableSize(string productID, string tradeMode, CancellationToken cancellationToken) {
		var path = "/api/v5/account/max-avail-size";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new()
		};
		reqMsg.Query
			.Add(nameof(OKEXOrderKeys.instId), productID)
			.Add(nameof(OKEXOrderKeys.tdMode), tradeMode);
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, AccountAvailableMaxSizeResponseContext.Default.AccountAvailableMaxSizeResponse, cancellationToken);
	}
	// 调整保证金
	public async Task<MarginBalanceResponse?> SetMarginBalance(MarginBalanceArgDto marginBalance, CancellationToken cancellationToken) {
		var path = "/api/v5/account/position/margin-balance";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<MarginBalanceArgDto>() {
			Content = marginBalance
		};

		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg, MarginBalanceArgContext.Default.MarginBalanceArgDto, MarginBalanceResponseContext.Default.MarginBalanceResponse, cancellationToken);
	}

	// 获取杠杆倍数
	public async Task<LeverageInfoResponse?> GetAccountLeverage(string productID, string tradeMode, CancellationToken cancellationToken) {
		var path = "/api/v5/account/leverage-info";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instId), productID },
				{ nameof(OKEXOrderKeys.mgnMode), tradeMode },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, LeverageInfoResponseContext.Default.LeverageInfoResponse, cancellationToken);
	}
		
	// 逐仓交易设置
	public async Task<DontCareAboutBodyResponse?> SetIsolatedMode(IsolatedModeArgDto isolatedMode, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-isolated-mode";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<IsolatedModeArgDto>() {
			Content = isolatedMode
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg, IsolatedModeArgContext.Default.IsolatedModeArgDto, DontCareAboutBodyResponseContext.Default.DontCareAboutBodyResponse, cancellationToken);
	}
	
	// 设置账户模式
	public async Task<DontCareAboutBodyResponse?> SetAccountMode(AccountModeArgDto mode, CancellationToken cancellationToken) {
		var path = "/api/v5/account/set-account-level";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<AccountModeArgDto>() {
			Content = mode
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg, AccountModeArgDtoContext.Default.AccountModeArgDto, DontCareAboutBodyResponseContext.Default.DontCareAboutBodyResponse, cancellationToken);
	}

	// 下单
	public async Task<OKEXResponse<OrderOperationDto>?> newOrder(OKEXOrderArgDto order, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<OKEXOrderArgDto>() {
			Content = order
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg, OKEXOrderArgContext.Default.OKEXOrderArgDto, OrderOperationResponseContext.Default.OKEXResponseOrderOperationDto, cancellationToken);
	}
	

	
	// 批量下单
	public async Task<OKEXResponse<OrderOperationDto>?> newBatchOrder(OKEXOrderArgDto[] orders, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<OKEXOrderArgDto[]>() {
			Content = orders
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg,
		OKEXBatchOrderArgContext.Default.OKEXOrderArgDtoArray,
		OrderOperationResponseContext.Default.OKEXResponseOrderOperationDto, cancellationToken);
	}
	

	// 撤单
	public async Task<OKEXResponse<OrderOperationDto>?> CancelOrderByID(CancelOrderArgDto order, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/cancel-order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<CancelOrderArgDto>() {
			Content = order
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg, CancelOrderArgContext.Default.CancelOrderArgDto, OrderOperationResponseContext.Default.OKEXResponseOrderOperationDto, cancellationToken);
	}
	//
	// 批量撤单
	public async Task<OKEXResponse<OrderOperationDto>?> CancelBatchOrders(CancelOrderArgDto[] orders, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/cancel-batch-orders";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<CancelOrderArgDto[]>() {
			Content = orders
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Post, uriBuilder.Uri, reqMsg,
			CancelBatchOrderArgContext.Default.CancelOrderArgDtoArray,
			OrderOperationResponseContext.Default.OKEXResponseOrderOperationDto, cancellationToken);
	}
	//
	// 修改订单
	public async Task<OKEXResponse<OrderOperationDto>?> ModifyOrder(ModifiedOrderArgDto order, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/amend-order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<ModifiedOrderArgDto>() {
			Content = order
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Get, uriBuilder.Uri, reqMsg,
			ModifiedOrderArgContext.Default.ModifiedOrderArgDto,
			OrderOperationResponseContext.Default.OKEXResponseOrderOperationDto, cancellationToken);
	}

	// 修改订单
	public async Task<OKEXResponse<OrderOperationDto>?> ModifyBatchOrders(ModifiedOrderArgDto[] orders, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/amend-order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage<ModifiedOrderArgDto[]>() {
			Content = orders
		};
		
		return await _client.FetchWithJsonBody(HttpMethod.Get, uriBuilder.Uri, reqMsg,
			ModifiedBatchOrdersArgContext.Default.ModifiedOrderArgDtoArray,
			OrderOperationResponseContext.Default.OKEXResponseOrderOperationDto, cancellationToken);
	}

	// 获取订单信息
	public async Task<OKEXOrderListResponse?> GetOrderByID(string productID, string platformOrderID, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/order";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instId), productID },
				{ nameof(OKEXOrderKeys.ordId), platformOrderID },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}

	// 获取历史订单,七天
	public async Task<OKEXOrderListResponse?> GetFilledOrderList(string tradeType, string orderCategory, string orderState, CancellationToken cancellationToken) {
		var path = "/api/v5/trade/orders-history";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.instFamily), orderCategory },
				// { nameof(OKEXOrderKeys.instId), productID },
				{ nameof(OKEXOrderKeys.state), orderState },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, OKEXOrderListResponseContext.Default.OKEXOrderListResponse, cancellationToken);
	}
	// 获取指数行情
	public async Task<OKEXResponse<IndexTickersDto>?> GetIndexTickers(string productID, string unit, CancellationToken cancellationToken) {
		var path = "/api/v5/market/index-tickers";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instId), productID },
				{ nameof(OKEXOrderKeys.quoteCcy), unit },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, IndexTickersResponseContext.Default.OKEXResponseIndexTickersDto, cancellationToken);
	}
	// 获取系统时间
	public async Task<OKEXResponse<SystemTimeDto>?> GetSystemTime(CancellationToken cancellationToken) {
		var path = "/api/v5/public/time";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage();
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, SystemTimeResponseContext.Default.OKEXResponseSystemTimeDto, cancellationToken);
	}

	// 获取标记价格
	public async Task<OKEXResponse<MarkPriceDto>?> GetMarkPrice(string tradeType, string productID, CancellationToken cancellationToken) {
		var path = "/api/v5/public/mark-price";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instType), tradeType },
				{ nameof(OKEXOrderKeys.instId), productID },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, MarkPriceResponseContext.Default.OKEXResponseMarkPriceDto, cancellationToken);
	}

	
	// 获取永续当前资金费率
	public async Task<OKEXResponse<FundingRateDto>?> GetFundingRate(string productID, CancellationToken cancellationToken) {
		var path = "/api/v5/public/funding-rate";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage() {
			Query = new(new Dictionary<string, string> {
				{ nameof(OKEXOrderKeys.instId), productID },
			})
		};
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, FundingRateResponseContext.Default.OKEXResponseFundingRateDto, cancellationToken);
	}

	// 获取法币汇率
	public async Task<OKEXResponse<ExchangeRateDto>?> GetExchangeRate(CancellationToken cancellationToken) {
		var path = "/api/v5/market/exchange-rate";
		var uriBuilder = new UriBuilder(BaseAddress) {
			Path = path
		};

		if (cancellationToken.IsCancellationRequested) {
			return default;
		}
		
		var reqMsg = new RequestMessage();
		
		return await _client.FetchWithGenericBody(HttpMethod.Get, uriBuilder.Uri, reqMsg, ExchangeRateResponseContext.Default.OKEXResponseExchangeRateDto, cancellationToken);
	}
}