using System.Text.Json.Serialization;
using Bot.OKEXApi;

public sealed class DontCareAboutBodyResponse: IOKEXResponse {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; init; } = "";
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(DontCareAboutBodyResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class DontCareAboutBodyResponseContext: JsonSerializerContext {}