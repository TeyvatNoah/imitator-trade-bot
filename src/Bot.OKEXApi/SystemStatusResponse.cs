using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class SystemStatusResponse: IOKEXResponse<SystemStatusDto> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; init; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public SystemStatusDto[] Data { get; init; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(SystemStatusResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class SystemStatusResponseContext: JsonSerializerContext {}