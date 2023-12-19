using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class SystemTimeDto {
	// 指数
	[JsonPropertyName(nameof(SystemStatusKeys.ts))]
	public string? Timestamp { get; set; }
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(SystemTimeDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class SystemTimeContext: JsonSerializerContext {}