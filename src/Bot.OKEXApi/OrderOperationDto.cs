using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class OrderOperationDto {
	// 订单ID
	[JsonPropertyName(nameof(OKEXOrderKeys.ordId))]
	public string? PlatformOrderID { get; set; }

	// 自定义ID
	[JsonPropertyName(nameof(OKEXOrderKeys.clOrdId))]
	public string? UserOrderID { get; set; }

	// Tag
	[JsonPropertyName(nameof(OKEXOrderKeys.tag))]
	public string? Tag { get; set; }

	// reqid
	[JsonPropertyName(nameof(OKEXOrderKeys.reqId))]
	public string? ReqID { get; set; }

	// 执行状态信息
	[JsonPropertyName(nameof(OKEXOrderKeys.sMsg))]
	public string? StatusMessage { get; set; }

	// 执行状态码
	[JsonPropertyName(nameof(OKEXOrderKeys.sCode))]
	public string? StatusCode { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OrderOperationDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OrderOperationContext: JsonSerializerContext {}
