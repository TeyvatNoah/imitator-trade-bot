using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class MarkPriceDto {
	// 产品类型
	[JsonPropertyName(nameof(OKEXOrderKeys.instType))]
	public string? TradeType { get; set; }
	
	// 产品ID
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 标记价格
	[JsonPropertyName(nameof(OKEXOrderKeys.markPx))]
	public string? MarkPrice { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.ts))]
	public string? Timestamp { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(MarkPriceDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class MarkPriceContext: JsonSerializerContext {}