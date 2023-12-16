using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class PositionMode {
	// 持仓方式
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.posMode))]
	public string? PosMode { get; set; }
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(PositionMode), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class PositionModeContext: JsonSerializerContext {}