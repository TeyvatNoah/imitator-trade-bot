using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountConfigurationResponse: IOKEXResponse<AccountConfigurationDto[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; set; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public AccountConfigurationDto[] Data { get; set; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(AccountConfigurationResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountConfigurationResponseContext: JsonSerializerContext {}