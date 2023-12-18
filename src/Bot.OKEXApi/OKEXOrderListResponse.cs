using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class OKEXOrderListResponse: IOKEXResponse<OEKXOrderDto[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; init; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public OEKXOrderDto[] Data { get; init; } = [];
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXOrderListResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OKEXOrderListResponseContext: JsonSerializerContext {}