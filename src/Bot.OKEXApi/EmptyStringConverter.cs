using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bot.OKEXApi;

public sealed class EmptyStringConverter: JsonConverter<double?> {
	public override double? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
		string? token = reader.GetString();
		return string.IsNullOrEmpty(token) ? null : Convert.ToDouble(token);
	}


	public override void Write(Utf8JsonWriter writer, double? value, JsonSerializerOptions options) {
		writer.WriteStringValue(value is null ? "null" : Convert.ToString(value));
	}
}