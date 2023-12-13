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
