using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class LeverageInfoResponse: IOKEXResponse<LeverageInfoDto[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; init; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public LeverageInfoDto[] Data { get; init; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(LeverageInfoResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class LeverageInfoResponseContext: JsonSerializerContext {}