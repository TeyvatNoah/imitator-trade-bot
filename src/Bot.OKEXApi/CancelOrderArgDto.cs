using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class CancelOrderArgDto {
	// 产品ID
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public required string ProductID { get; set; }
	
	// 订单ID
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXOrderKeys.ordId))]
	public required string PlatformOrderID { get; set; }
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(CancelOrderArgDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class CancelOrderArgContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(CancelOrderArgDto[]), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class CancelBatchOrderArgContext: JsonSerializerContext {}