using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class FundingRateDto {
	// 产品类型
	[JsonPropertyName(nameof(OKEXOrderKeys.instType))]
	public string? TradeType { get; set; }
	
	// 产品ID
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 标记价格
	[JsonPropertyName(nameof(OKEXOrderKeys.fundingRate))]
	public string? FundingRate { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.nextFundingRate))]
	public string? NextFundingRate { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.fundingTime))]
	public string? FundingTime { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.minFundingRate))]
	public string? MinFundingRate { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.maxFundingRate))]
	public string? MaxFundingRate { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.settState))]
	public string? settledState { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.settFundingRate))]
	public string? settledFundingRate { get; set; }

	// 时间戳
	[JsonPropertyName(nameof(OKEXOrderKeys.ts))]
	public string? Timestamp { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(FundingRateDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class FundingRateContext: JsonSerializerContext {}