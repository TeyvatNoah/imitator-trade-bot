using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountPositionModeDto {
	// 持仓方式
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.posMode))]
	public string? PosMode { get; set; }
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountPositionModeDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountPositionModeContext: JsonSerializerContext {}