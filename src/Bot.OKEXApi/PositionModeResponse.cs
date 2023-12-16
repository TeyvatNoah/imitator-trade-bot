using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class PositionModeResponse: IOKEXResponse<PositionMode[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; set; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public PositionMode[] Data { get; set; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(PositionModeResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class PositionModeResponseContext: JsonSerializerContext {}