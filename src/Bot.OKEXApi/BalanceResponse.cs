using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class BalanceResponse: IOKEXResponse<BalanceDto[]> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; set; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; set; } = "";
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public BalanceDto[] Data { get; set; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(BalanceResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class BalanceResponseContext: JsonSerializerContext {}