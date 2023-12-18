using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountMaxSizeDto {
	// 保证金币种
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 保证金币种
	[JsonPropertyName(nameof(OKEXOrderKeys.ccy))]
	public string? Currency { get; set; }

	// 最大可买交易币数量
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.maxBuy))]
	public string? MaxBuy { get; set; }

	// 最大可卖交易币数量
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.maxSell))]
	public string? MaxSell { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountMaxSizeDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountMaxSizeContext: JsonSerializerContext {}