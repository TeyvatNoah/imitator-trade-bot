using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class LeverageInfoDto {
	// 产品ID
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }
	
	// 保证金模式
	[JsonPropertyName(nameof(OKEXOrderKeys.mgnMode))]
	public string? TradeMode { get; set; }
	
	// 持仓方向
	[JsonPropertyName(nameof(OKEXOrderKeys.posSide))]
	public string? PosSide { get; set; }
	
	// 杠杆倍数
	[JsonPropertyName(nameof(OKEXOrderKeys.lever))]
	public float Leverage { get; set; } = 0;
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(LeverageInfoDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class LeverageInfoContext: JsonSerializerContext {}