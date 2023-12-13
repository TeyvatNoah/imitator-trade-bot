using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class OKEXOrderListResponse: IOKEXResponse<OEKXOrderDto[]> {
	public int Code { get; set; }
	public required string Message { get; set; }
	public required OEKXOrderDto[] Data { get; set; }
}

[JsonSerializable(typeof(OKEXOrderListResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OKEXOrderListResponseContext: JsonSerializerContext {}