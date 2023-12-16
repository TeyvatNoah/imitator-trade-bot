using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class BalanceDto {
	// 账户状态更新时间
	[JsonPropertyName(nameof(OKEXOrderKeys.uTime))]
	public string? StatusModifiedTime { get; set; }
	
	// 占用保证金,美金
	[JsonPropertyName(nameof(OKEXOrderKeys.imr))]
	public string? IMR { get; set; }

	// 维持保证金,美金
	[JsonPropertyName(nameof(OKEXOrderKeys.mmr))]
	public string? MMR { get; set; }

	// 保证金率
	[JsonPropertyName(nameof(OKEXOrderKeys.mgnRatio))]
	public string? MarginRatio { get; set; }

	// 美元为单位的持仓数量
	[JsonPropertyName(nameof(OKEXOrderKeys.notionalUsd))]
	public string? USDSize { get; set; }

	// 各币种资产详情
	// [JsonPropertyName(nameof(OKEXOrderKeys.details))]
	// public string? Details { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString)] 
[JsonSerializable(typeof(BalanceDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class BalanceContext: JsonSerializerContext {}