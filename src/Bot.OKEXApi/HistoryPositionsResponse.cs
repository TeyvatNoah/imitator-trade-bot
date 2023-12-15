using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class HistoryPositionsResponse: IOKEXResponse<HistoryPositionsDto[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; set; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public HistoryPositionsDto[] Data { get; set; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(HistoryPositionsResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class HistoryPositionResponseContext: JsonSerializerContext {}