using System.Text.Json.Serialization;


namespace Bot.OKEXApi;

public sealed class ModifiedOrderArgDto {
	
	// 产品ID, eg. 合约ID BTC-USD-200329
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public required string ProductID { get; set; }

	// 是否自动撤单
	[JsonPropertyName(nameof(OKEXOrderKeys.cxlOnFail))]
	public bool autoCancel { get; set; } = false;

	// 平台内部订单ID
	[JsonPropertyName(nameof(OKEXOrderKeys.ordId))]
	public string? PlatformOrderID { get; set; }

	// 自定义修改事件
	[JsonPropertyName(nameof(OKEXOrderKeys.reqId))]
	public string? ReqID { get; set; }

	// 委托数量
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.newSz))]
	public required string Size { get; set; }

	// 委托价格,单位币
	[JsonPropertyName(nameof(OKEXOrderKeys.newPx))]
	public string? ConsignmentPrice { get; set; }


}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(ModifiedOrderArgDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class ModifiedOrderArgContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(ModifiedOrderArgDto[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class ModifiedBatchOrdersArgContext: JsonSerializerContext {}