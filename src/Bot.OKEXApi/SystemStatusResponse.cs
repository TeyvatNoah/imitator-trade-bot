using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class SystemStatusResponse: IOKEXResponse<SystemStatusDto> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; set; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public SystemStatusDto Data { get; set; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(SystemStatusResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class SystemStatusResponseContext: JsonSerializerContext {}