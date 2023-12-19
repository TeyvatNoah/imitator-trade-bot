using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountModeArgDto {
	// 账户模式
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.acctLv))]
	public string? Mode { get; set; }
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountModeArgDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountModeArgDtoContext: JsonSerializerContext {}