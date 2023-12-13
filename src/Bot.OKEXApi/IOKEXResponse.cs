using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public interface IOKEXResponse<T> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	string Message { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	T Data { get; set; }
}

// [JsonSerializable(typeof(IOKEXResponse<>), GenerationMode = JsonSourceGenerationMode.Metadata)]
// public partial class IOKEXResponseContext: JsonSerializerContext {}