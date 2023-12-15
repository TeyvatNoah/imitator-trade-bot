using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class OKEXOrderListResponse: IOKEXResponse<OEKXOrderDto[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; set; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public OEKXOrderDto[] Data { get; set; } = [];
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(OKEXOrderListResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OKEXOrderListResponseContext: JsonSerializerContext {}