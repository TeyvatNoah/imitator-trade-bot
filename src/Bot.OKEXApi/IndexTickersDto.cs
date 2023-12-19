using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class IndexTickersDto {
	// 指数
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 最新 指数价格
	[JsonPropertyName(nameof(OKEXOrderKeys.idxPx))]
	public string? LatestIndexPrice { get; set; }

	// 24小时最高价
	[JsonPropertyName(nameof(OKEXOrderKeys.high24h))]
	public string? High24hPrice { get; set; }

	// 24小时最低价
	[JsonPropertyName(nameof(OKEXOrderKeys.low24h))]
	public string? Low24hPrice { get; set; }

	// 24小时开盘价
	[JsonPropertyName(nameof(OKEXOrderKeys.open24h))]
	public string? Open24hPrice { get; set; }

	// UTC+0开盘价
	[JsonPropertyName(nameof(OKEXOrderKeys.sodUtc0))]
	public string? UTC0OpenPrice { get; set; }

	// UTC+8开盘价
	[JsonPropertyName(nameof(OKEXOrderKeys.sodUtc8))]
	public string? UTC8OpenPrice { get; set; }

	// 指数价格更新时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.ts))]
	public string? UpdateTimestamp { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(IndexTickersDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class IndexTickersContext: JsonSerializerContext {}