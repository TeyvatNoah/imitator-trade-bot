using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class PositionsDto {
	// 交易类型, eg. 币币or合约
	[JsonPropertyName(nameof(OKEXOrderKeys.instType))]
	public string? TradeType { get; set; }

	// 保证金模式
	[JsonPropertyName(nameof(OKEXOrderKeys.mgnMode))]
	public string? TradeMode { get; set; }

	// 持仓ID
	[JsonPropertyName(nameof(OKEXOrderKeys.posId))]
	public string? PositionID { get; set; }

	// 持仓方向
	[JsonPropertyName(nameof(OKEXOrderKeys.posSide))]
	public string? PosSide { get; set; }
	
	// 持仓数量
	[JsonPropertyName(nameof(OKEXOrderKeys.pos))]
	public string? PositionSize { get; set; }

	// 可平仓数量
	[JsonPropertyName(nameof(OKEXOrderKeys.availPos))]
	public string? AvailablePosition { get; set; }

	// 开仓均价
	[JsonPropertyName(nameof(OKEXOrderKeys.avgPx))]
	public string? AverageFillPrice { get; set; }

	// 持仓状态更新时间
	[JsonPropertyName(nameof(OKEXOrderKeys.uTime))]
	public string? PositionsModifiedTime { get; set; }

	// 持仓创建时间
	[JsonPropertyName(nameof(OKEXOrderKeys.cTime))]
	public string? PositionsCreatedTime { get; set; }
	
	// 杠杆倍数
	[JsonPropertyName(nameof(OKEXOrderKeys.lever))]
	public string? Leverage { get; set; }

	// 产品ID, eg. 合约ID BTC-USD-200329
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 标记价格
	[JsonPropertyName(nameof(OKEXOrderKeys.markPx))]
	public string? markPrice { get; set; }
	

	// 初始保证金
	[JsonPropertyName(nameof(OKEXOrderKeys.imr))]
	public string? IMR { get; set; }

	// 保证金余额
	[JsonPropertyName(nameof(OKEXOrderKeys.margin))]
	public string? Margin { get; set; }

	// 保证金率
	[JsonPropertyName(nameof(OKEXOrderKeys.mgnRatio))]
	public string? MarginRatio { get; set; }

	// 维持保证金
	[JsonPropertyName(nameof(OKEXOrderKeys.mmr))]
	public string? MMR { get; set; }

	// 利息,未扣
	[JsonPropertyName(nameof(OKEXOrderKeys.interest))]
	public string? Interest { get; set; }

	// 最新成交ID
	[JsonPropertyName(nameof(OKEXOrderKeys.tradeId))]
	public string? LatestFillID { get; set; }

	// 美元为单位的持仓数量
	[JsonPropertyName(nameof(OKEXOrderKeys.notionalUsd))]
	public string? USDSize { get; set; }

	// 占用保证金币种
	[JsonPropertyName(nameof(OKEXOrderKeys.ccy))]
	public string? CCY { get; set; }

	// 信号区
	[JsonPropertyName(nameof(OKEXOrderKeys.adl))]
	public string? ADL { get; set; }


	// 订单状态
	[JsonPropertyName(nameof(OKEXOrderKeys.state))]
	public string? OrderState { get; set; }


	// 最新成交价
	[JsonPropertyName(nameof(OKEXOrderKeys.last))]
	public string? LatestFillPrice { get; set; }

	// 最新指数价格
	[JsonPropertyName(nameof(OKEXOrderKeys.idxPx))]
	public string? IDXPrice { get; set; }

	// 美元价格
	[JsonPropertyName(nameof(OKEXOrderKeys.usdPx))]
	public string? USDPrice { get; set; }

	// 盈亏平衡价
	[JsonPropertyName(nameof(OKEXOrderKeys.bePx))]
	public string? BEPrice { get; set; }

	// 现货对冲占用数量
	[JsonPropertyName(nameof(OKEXOrderKeys.spotInUseAmt))]
	public string? SpotInUseAmount { get; set; }

	// 现货对冲占用币种
	[JsonPropertyName(nameof(OKEXOrderKeys.spotInUseCcy))]
	public string? SpotInUseCurrency { get; set; }

	// 已实现收益
	[JsonPropertyName(nameof(OKEXOrderKeys.realizedPnl))]
	public string? RealizedPnl { get; set; }

	// 平仓订单累计收益
	[JsonPropertyName(nameof(OKEXOrderKeys.pnl))]
	public string? Pnl { get; set; }

	// 累计手续费
	[JsonPropertyName(nameof(OKEXOrderKeys.fee))]
	public string? Fee { get; set; }

	// 累计资金费用
	[JsonPropertyName(nameof(OKEXOrderKeys.fundingFee))]
	public string? FundingFee { get; set; }

	// 累计爆仓金额
	[JsonPropertyName(nameof(OKEXOrderKeys.liqPenalty))]
	public string? LiqPenalty { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(PositionsDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class PositionContext: JsonSerializerContext {}