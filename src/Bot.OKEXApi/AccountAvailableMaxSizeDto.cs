using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountAvailableMaxSizeDto {
	// 产品ID
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }

	// 最大买入可用数量
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.availBuy))]
	public string? AvailableBuy { get; set; }

	// 最大卖出可用数量
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.availSell))]
	public string? AvailableSell { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountAvailableMaxSizeDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountAvailableMaxSizeContext: JsonSerializerContext {}