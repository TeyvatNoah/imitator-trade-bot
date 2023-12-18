using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public interface IOKEXResponse {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	string Message { get; init; }
}

public interface IOKEXResponse<T>: IOKEXResponse {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	T Data { get; init; }
}

[JsonSerializable(typeof(IOKEXResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class IOKEXResponseContext: JsonSerializerContext {}