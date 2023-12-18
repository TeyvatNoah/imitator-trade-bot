using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class AccountConfigurationDto {
	// UID
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.uid))]
	public string? UID { get; set; }
	
	// 父账户UID
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.mainUid))]
	public string? MainUID { get; set; }

	// 账户层级
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.acctLv))]
	public string? AccountLevel { get; set; }

	// 持仓方式
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.posMode))]
	public string? PositionMode { get; set; }

	// 是否自动借币
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.autoLoan))]
	public bool? AutoLoan { get; set; }

	// 币本位or美元本位
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.greeksType))]
	public string? GreeksType { get; set; }

	// 用户等级
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.level))]
	public string? UserLevel { get; set; }

	// 临时体验等级
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.levelTmp))]
	public string? TempLevel { get; set; }

	// 逐仓保证金划转模式
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.ctIsoMode))]
	public string? CTIsoMode { get; set; }

	// 币币杠杆的逐仓保证金划转模式
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.mgnIsoMode))]
	public string? MgnIsoMode { get; set; }

	// 现货对冲类型
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.spotOffsetType))]
	public string? SpotOffsetType { get; set; }

	// 用户角色
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.roleType))]
	public string? RoleType { get; set; }

	// 带单合约
	// [JsonPropertyName(nameof(OKEXAccountConfigurationKeys.traderInsts))]
	// public string? TraderInsts { get; set; }

	// 现货跟单角色
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.spotRoleType))]
	public string? SpotRoleType { get; set; }

	// 是否已开通期权交易
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.opAuth))]
	public string? OpAuth { get; set; }

	// KYC等级
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.kycLv))]
	public string? KYCLevel { get; set; }

	// 当前API Key备注
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.label))]
	public string? APIKeyComments { get; set; }

	// API Key绑定IP
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.ip))]
	public string? BindingIP { get; set; }

	// API Key权限
	[JsonPropertyName(nameof(OKEXAccountConfigurationKeys.perm))]
	public string? Permission { get; set; }

}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(AccountConfigurationDto), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountConfigurationContext: JsonSerializerContext {}