using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class MarginBalanceArgDto {
	// 产品ID
	[JsonPropertyName(nameof(OKEXOrderKeys.instId))]
	public string? ProductID { get; set; }
	
	// 持仓方向
	[JsonPropertyName(nameof(OKEXOrderKeys.posSide))]
	public string? PosSide { get; set; }
	
	// 增加或减少
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.type))]
	public string? Type { get; set; }
	
	// 变更的保证金数量
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.amt))]
	public double? Amount { get; set; }
	
	// 币种
	[JsonPropertyName(nameof(OKEXOrderKeys.ccy))]
	public string? Currency { get; set; }
	
	//  自动借币
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.auto))]
	public bool? Auto { get; set; }
	
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(MarginBalanceArgDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class MarginBalanceArgContext: JsonSerializerContext {}