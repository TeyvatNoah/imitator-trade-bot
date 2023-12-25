using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountAvailableMaxSizeResponse: IOKEXResponse<AccountAvailableMaxSizeDto> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; init; } = "";
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public AccountAvailableMaxSizeDto[] Data { get; init; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountAvailableMaxSizeResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountAvailableMaxSizeResponseContext: JsonSerializerContext {}