// using System.Text.Json;
// using System.Text.Json.Serialization;

// using RestEase;

// public sealed class ApiResponseBodyDeserializer: ResponseDeserializer {
// 	public readonly static JsonSerializerOptions Options = new() {
// 		NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
// 	};
// 	public override T Deserialize<T>(string? content, HttpResponseMessage response, ResponseDeserializerInfo info) {
// 		if (content is null) {
// 			throw new ArgumentNullException("Response content is null");
// 		} else {
// 			var obj = JsonSerializer.Deserialize<T>(content, Options);
// 			if (obj is null) {
// 				throw new JsonException($"Deserialize JSON content failed. content: {content}");
// 			} else {
// 				return obj;
// 			}
// 		}
// 	}
// }