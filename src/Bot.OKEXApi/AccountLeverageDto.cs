using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountLeverageDto {
	// 产品ID
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }
	
	// 保证金币种
	[JsonPropertyName(nameof(OKEXOrderKeys.ccy))]
	public string? Currency { get; set; }
	
	// 杠杆倍数
	[JsonPropertyName(nameof(OKEXOrderKeys.lever))]
	public float? Leverage { get; set; }
	
	// 保证金模式
	[JsonPropertyName(nameof(OKEXOrderKeys.mgnMode))]
	public string? TradeMode { get; set; }
	
	//  持仓方向
	[JsonPropertyName(nameof(OKEXOrderKeys.posSide))]
	public string? PosSide { get; set; }
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountLeverageDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountLeverageContext: JsonSerializerContext {}