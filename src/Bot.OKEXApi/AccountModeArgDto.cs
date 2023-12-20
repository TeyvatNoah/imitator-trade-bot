using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountModeArgDto {
	// 账户模式
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.acctLv))]
	public required string Mode { get; set; }
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountModeArgDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountModeArgDtoContext: JsonSerializerContext {}