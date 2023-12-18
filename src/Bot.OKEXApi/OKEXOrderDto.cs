using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class OEKXOrderDto {
	// 交易类型, eg. 币币or合约
	[JsonPropertyName(nameof(OKEXOrderKeys.instType))]
	public string? TradeType { get; set; }

	// 产品ID, eg. 合约ID BTC-USD-200329
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 平台内部订单ID
	[JsonPropertyName(nameof(OKEXOrderKeys.ordId))]
	public string? PlatformOrderID { get; set; }

	// 我们自定义的订单ID
	[JsonPropertyName(nameof(OKEXOrderKeys.clOrdId))]
	public string? UserOrderID { get; set; }

	// 订单标签
	[JsonPropertyName(nameof(OKEXOrderKeys.tag))]
	public string? Tag { get; set; }

	// 委托价格,单位币
	[JsonPropertyName(nameof(OKEXOrderKeys.px))]
	public string? ConsignmentPrice { get; set; }

	// 委托数量
	[JsonPropertyName(nameof(OKEXOrderKeys.sz))]
	public string? Size { get; set; }

	// 订单类型, eg,市价or限价
	[JsonPropertyName(nameof(OKEXOrderKeys.ordType))]
	public string? OrderType { get; set; }

	// 交易方向
	[JsonPropertyName(nameof(OKEXOrderKeys.side))]
	public string? Side { get; set; }

	// 持仓方向
	[JsonPropertyName(nameof(OKEXOrderKeys.posSide))]
	public string? PosSide { get; set; }

	// 交易模式
	[JsonPropertyName(nameof(OKEXOrderKeys.tdMode))]
	public string? TradeMode { get; set; }

	// 我们自定义的订单ID
	[JsonPropertyName(nameof(OKEXOrderKeys.accFillSz))]
	public string? PartialFillSize { get; set; }

	// 最新成交价, 注意此字段可能为""
	[JsonPropertyName(nameof(OKEXOrderKeys.fillPx))]
	public string? LatestFillPrice { get; set; }

	// 最新成交ID
	[JsonPropertyName(nameof(OKEXOrderKeys.tradeId))]
	public string? LatestFillID { get; set; }

	// 最新成交数量
	[JsonPropertyName(nameof(OKEXOrderKeys.fillSz))]
	public string? LatestFillSize { get; set; }

	// 最新成交时间
	[JsonPropertyName(nameof(OKEXOrderKeys.fillTime))]
	public string? LatestFillTime { get; set; }

	// 成交均价,可能为""
	[JsonPropertyName(nameof(OKEXOrderKeys.avgPx))]
	public string? AverageFillPrice { get; set; }

	// 订单状态
	[JsonPropertyName(nameof(OKEXOrderKeys.state))]
	public string? OrderState { get; set; }

	// 杠杆倍数
	[JsonPropertyName(nameof(OKEXOrderKeys.lever))]
	public string? Leverage { get; set; }

	// 手续费币种
	[JsonPropertyName(nameof(OKEXOrderKeys.feeCcy))]
	public string? FeeCurrencyType { get; set; }

	// 手续费
	[JsonPropertyName(nameof(OKEXOrderKeys.fee))]
	public string? Fee { get; set; }

	// 订单来源
	[JsonPropertyName(nameof(OKEXOrderKeys.source))]
	public string? OrderSource { get; set; }

	// 订单种类, 交割or强平
	[JsonPropertyName(nameof(OKEXOrderKeys.category))]
	public string? OrderCategory { get; set; }

	// 订单取消原因代码
	[JsonPropertyName(nameof(OKEXOrderKeys.cancelSource))]
	public string? CancelReasonCode { get; set; }

	// 订单取消原因描述
	[JsonPropertyName(nameof(OKEXOrderKeys.cancelSourceReason))]
	public string? CancelReasonDesc { get; set; }

	// 订单状态更新时间
	[JsonPropertyName(nameof(OKEXOrderKeys.uTime))]
	public string? OrderModifiedTime { get; set; }

	// 订单创建时间
	[JsonPropertyName(nameof(OKEXOrderKeys.cTime))]
	public string? OrderCreatedTime { get; set; }
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OEKXOrderDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OKEXOrderContext: JsonSerializerContext {}