namespace Bot.OKEXApi;

public enum OKEXResponseKeys {
	code,
	msg,
	data
}


public enum OKEXOrderKeys {
	instType,
	instId,
	tgtCcy,
	ccy,
	ordId,
	clOrdId,
	tag,
	px,
	pxUsd,
	pxVol,
	pxType,
	sz,
	pnl,
	ordType,
	side,
	posSide,
	tdMode,
	accFillSz,
	fillPx,
	tradeId,
	fillSz,
	fillTime,
	avgPx,
	state,
	lever,
	attachAlgoClOrdId,
	tpTriggerPx,
	tpTriggerPxType,
	tpOrdPx,
	slTriggerPx,
	slTriggerPxType,
	slOrdPx,
	attachAlgoOrds,
	stpId,
	stpMode,
	feeCcy,
	fee,
	rebateCcy,
	source,
	rebate,
	category,
	reduceOnly,
	cancelSource,
	cancelSourceReason,
	quickMgnType,
	algoClOrdId,
	algoId,
	uTime,
	cTime,
	posId,
	mgnMode,
	pos,
	availPos,
	upl,
	uplRatio,
	uplLastPx,
	uplRatioLastPx,
	liqPx,
	markPx,
	imr,
	margin,
	mgnRatio,
	mmr,
	interest,
	notionalUsd,
	adl,
	last,
	idxPx,
	usdPx,
	bePx,
	spotInUseAmt,
	spotInUseCcy,
	realizedPnl,
	fundingFee,
	liqPenalty,
	closeOrderAlgo,
	openAvgPx,
	closeAvgPx,
	openMaxPos,
	closeTotalPos,
	direction,
	triggerPx,
	uly,
	pnlRatio,
	details,
	totalEq,
	isoEq,
	adjEq,
	ordFroz

}


public enum OKEXAccountConfigurationKeys {
	uid,
	mainUid,
	acctLv,
	posMode,
	autoLoan,
	greeksType,
	level,
	levelTmp,
	ctIsoMode,
	mgnIsoMode,
	spotOffsetType,
	upSpotOffset,
	roleType,
	traderInsts,
	spotRoleType,
	spotTraderInsts,
	opAuth,
	kycLv,
	label,
	ip,
	perm,
	maxBuy,
	maxSell,
	availBuy,
	availSell,
	type,
	amt,
	auto,
	isoMode,
}



// public enum OrderType {
// 	SPOT,
// 	MARGIN,
// 	SWAP,
// 	FUTURES,
// 	OPTION
// }

public sealed class OKEXOrderType {
	// 币币
	public const string SPOT = "SPOT";
	// 币币杠杆
	public const string MARGIN = "MARGIN";
	// 永续合约
	public const string SWAP = "SWAP";
	// 交割合约
	public const string FUTURES = "FUTURES";
	// 期权
	public const string OPTION = "OPTION";
	
}


public enum OKEXErrorCode {
	// 操作全部失败
	AllOperationFailed = 1,
	// 超出调用频率限制
	RateLimit = 50111,
	// 服务挂了
	RemoteDown = 50001,
	// 请求参数的json错误
	InvalidJSONArgument = 50002,
	// 非application/json的MIME Type错误
	MIMETypeError = 50006,
	// 用户处于爆仓冻结
	Liquidation = 50009,
	// 系统繁忙
	SystemBusy = 50013,
	// 必填参数为空错误
	RequiredArguments = 50014,
	// 参数冲突错误
	ConflictArguments = 50024,
	// 系统错误
	SystemError = 50026,
	// 操作频繁请重试
	TooManyOperation = 50040,
	// ordId或clOrdId至少填一个
	OidOrCOid = 51003,
	// 下单失败
	OrderFailed = 51004,
	// 委托失败, 委托数量不能小于1
	SizeError = 51007,
	// 委托失败
	ConsignFailed = 51008,
	// 委托数量必须超过单笔下限
	MinimumSizeError = 51020,
}

public enum SystemStatusKeys {
	title,
	state,
	begin,
	end,
	maintType,
	href,
}