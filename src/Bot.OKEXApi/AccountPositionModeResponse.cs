using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountPositionModeResponse: IOKEXResponse<AccountPositionModeDto> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; init; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public AccountPositionModeDto[] Data { get; init; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountPositionModeResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountPositionModeResponseContext: JsonSerializerContext {}