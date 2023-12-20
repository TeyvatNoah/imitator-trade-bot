using System.Text.Json.Serialization;


namespace Bot.OKEXApi;

public sealed class OKEXOrderArgDto {
	
	// 产品ID, eg. 合约ID BTC-USD-200329
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public required string ProductID { get; set; }
	
	// 交易模式
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.tdMode))]
	public required string TradeMode { get; set; }

	// 保证金币种
	[JsonPropertyName(nameof(OKEXOrderKeys.ccy))]
	public string? Currency { get; set; }

	// 我们自定义的订单ID
	[JsonPropertyName(nameof(OKEXOrderKeys.clOrdId))]
	public string? UserOrderID { get; set; }

	// 订单标签
	[JsonPropertyName(nameof(OKEXOrderKeys.tag))]
	public string? Tag { get; set; }
	
	// 订单方向
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.side))]
	public required string Side { get; set; }

	// 持仓方向
	[JsonPropertyName(nameof(OKEXOrderKeys.posSide))]
	public string? PosSide { get; set; }

	// 订单类型, eg,市价or限价
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.ordType))]
	public required string OrderType { get; set; }

	// 委托数量
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.sz))]
	public required string Size { get; set; }

	// 委托价格,单位币
	[JsonPropertyName(nameof(OKEXOrderKeys.px))]
	public string? ConsignmentPrice { get; set; }

	// 是否只减仓
	[JsonPropertyName(nameof(OKEXOrderKeys.reduceOnly))]
	public bool ReduceOnly { get; set; } = false;

	// 一件借币类型
	[JsonPropertyName(nameof(OKEXOrderKeys.quickMgnType))]
	public string QuickMgnType { get; set; } = QuickMgnTypeEnum.Manual;

	// 自成交保护ID
	[JsonPropertyName(nameof(OKEXOrderKeys.stpId))]
	public string? StpID { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXOrderArgDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OKEXOrderArgContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXOrderArgDto[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OKEXBatchOrderArgContext: JsonSerializerContext {}