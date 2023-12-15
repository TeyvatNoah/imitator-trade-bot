using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class SystemStatusDto {
	// 维护公告标题
	[JsonPropertyName(nameof(SystemStatusKeys.title))]
	public string? Title { get; set; }
	// 维护状态
	[JsonPropertyName(nameof(SystemStatusKeys.state))]
	public string? State { get; set; }
	// 维护开始时间
	[JsonPropertyName(nameof(SystemStatusKeys.begin))]
	public string? Begin { get; set; }
	// 维护结束时间
	[JsonPropertyName(nameof(SystemStatusKeys.end))]
	public string? End { get; set; }
	// 维护类型
	[JsonPropertyName(nameof(SystemStatusKeys.maintType))]
	public string? MainType { get; set; }
	// 维护公告链接
	[JsonPropertyName(nameof(SystemStatusKeys.href))]
	public string? Href { get; set; }


}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(SystemStatusDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class SystemStatusDtoContext: JsonSerializerContext {}