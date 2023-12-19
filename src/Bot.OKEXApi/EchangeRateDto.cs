using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class ExchangeRateDto {
	// 汇率
	[JsonPropertyName(nameof(SystemStatusKeys.usdCny))]
	public string? USDCNY { get; set; }
	

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(ExchangeRateDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class ExchangeRateContext: JsonSerializerContext {}