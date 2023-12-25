using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class OKEXResponse<T>: IOKEXResponse<T> {
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.code))]
	public int Code { get; init; }
	[JsonRequired]
	[JsonPropertyName(nameof(OKEXResponseKeys.msg))]
	public string Message { get; init; } = "";
	[JsonPropertyName(nameof(OKEXResponseKeys.data))]
	public T[] Data { get; init; } = default!;
}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<SystemTimeDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class SystemTimeResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<IndexTickersDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class IndexTickersResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<MarkPriceDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class MarkPriceResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<FundingRateDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class FundingRateResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<ExchangeRateDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class ExchangeRateResponseContext: JsonSerializerContext {}

// [JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
// [JsonSerializable(typeof(OKEXResponse<CancelOrderDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
// public partial class CancelOrderResponseContext: JsonSerializerContext {}


[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<OrderOperationDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OrderOperationResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<AccountConfigurationDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class AccountConfigurationResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<LeverageInfoDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class LeverageInfoResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<InstrumentsDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class InstrumentsResponseContext: JsonSerializerContext {}

[JsonSourceGenerationOptions(NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)] 
[JsonSerializable(typeof(OKEXResponse<OEKXOrderDto>), GenerationMode = JsonSourceGenerationMode.Metadata)]
public partial class OKEXOrderResponseContext: JsonSerializerContext {}
