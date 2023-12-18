using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class IsolatedModeArgDto {
	// 产品ID
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.isoMode))]
	public string? IsolatedMode { get; set; }
	
	// 持仓方向
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.type))]
	public string? Type { get; set; }
	
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(IsolatedModeArgDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class IsolatedModeArgContext: JsonSerializerContext {}