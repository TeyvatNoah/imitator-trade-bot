using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class PositionsResponse: IOKEXResponse<PositionsDto[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; set; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public PositionsDto[] Data { get; set; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(PositionsResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class PositionResponseContext: JsonSerializerContext {}