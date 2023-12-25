using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class InstrumentsDto {
	// 价格精度
	[JsonPropertyName(nameof(OKEXOrderKeys.tickSz))]
	public double PricePrecision { get; set; }
	
	// 数量精度
	[JsonPropertyName(nameof(OKEXOrderKeys.lotSz))]
	public double SizePrecision { get; set; }
	
	// 最小下单数量
	[JsonPropertyName(nameof(OKEXOrderKeys.minSz))]
	public double MinSize { get; set; }
	
	// 合约方向
	[JsonPropertyName(nameof(OKEXOrderKeys.ctType))]
	public string CTType { get; set; } = default!;
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(InstrumentsDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class InstrumentsContext: JsonSerializerContext {}