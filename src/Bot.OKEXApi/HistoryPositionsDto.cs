using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class HistoryPositionsDto {
	// 交易类型, eg. 币币or合约
	[JsonPropertyName(nameof(OKEXOrderKeys.instType))]
	public string? TradeType { get; set; }

	// 产品ID, eg. 合约ID BTC-USD-200329
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 保证金模式
	[JsonPropertyName(nameof(OKEXOrderKeys.mgnMode))]
	public string? TradeMode { get; set; }

	// 仓位状态更新时间
	[JsonPropertyName(nameof(OKEXOrderKeys.uTime))]
	public string? PositionsModifiedTime { get; set; }

	// 仓位创建时间
	[JsonPropertyName(nameof(OKEXOrderKeys.cTime))]
	public string? PositionsCreatedTime { get; set; }

	// 持仓ID
	[JsonPropertyName(nameof(OKEXOrderKeys.posId))]
	public string? PositionID { get; set; }

	// 已实现收益
	[JsonPropertyName(nameof(OKEXOrderKeys.realizedPnl))]
	public string? RealizedPnl { get; set; }

	// 累计手续费
	[JsonPropertyName(nameof(OKEXOrderKeys.fee))]
	public string? Fee { get; set; }

	// 累计资金费用
	[JsonPropertyName(nameof(OKEXOrderKeys.fundingFee))]
	public string? FundingFee { get; set; }
	
	// 累计爆仓金额
	[JsonPropertyName(nameof(OKEXOrderKeys.liqPenalty))]
	public string? LiqPenalty { get; set; }
	
	// 平仓订单累计收益
	[JsonPropertyName(nameof(OKEXOrderKeys.pnl))]
	public string? Pnl { get; set; }

	// 杠杆倍数
	[JsonPropertyName(nameof(OKEXOrderKeys.lever))]
	public string? Leverage { get; set; }

	// 持仓方向
	[JsonPropertyName(nameof(OKEXOrderKeys.direction))]
	public string? PosSide { get; set; }

	// 开仓均价
	[JsonPropertyName(nameof(OKEXOrderKeys.openAvgPx))]
	public string? OpenAveragePrice { get; set; }
	
	// 平仓均价
	[JsonPropertyName(nameof(OKEXOrderKeys.closeAvgPx))]
	public string? CloseAveragePrice { get; set; }

	// 最大持仓量
	[JsonPropertyName(nameof(OKEXOrderKeys.openMaxPos))]
	public string? OpenMaxPosition { get; set; }

	// 累计平仓量
	[JsonPropertyName(nameof(OKEXOrderKeys.closeTotalPos))]
	public string? CloseTotalPosition { get; set; }

	// 平仓收益率
	[JsonPropertyName(nameof(OKEXOrderKeys.pnlRatio))]
	public string? PnlRatio { get; set; }
	
	// 触发标记价格
	[JsonPropertyName(nameof(OKEXOrderKeys.triggerPx))]
	public string? TriggerPrice { get; set; }

	// 标的指数
	[JsonPropertyName(nameof(OKEXOrderKeys.uly))]
	public string? ULY { get; set; }

	// 占用保证金币种
	[JsonPropertyName(nameof(OKEXOrderKeys.ccy))]
	public string? CCY { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(HistoryPositionsDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class HistoryPositionContext: JsonSerializerContext {}