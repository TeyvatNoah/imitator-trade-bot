using System.Text.Json;

using RestEase;

public sealed class ApiResponseBodyDeserializer: ResponseDeserializer {
	public override T Deserialize<T>(string? content, HttpResponseMessage response, ResponseDeserializerInfo info) {
		if (content is null) {
			throw new ArgumentNullException("Response content is null");
		} else {
			var obj = JsonSerializer.Deserialize<T>(content);
			if (obj is null) {
				throw new JsonException($"Deserialize JSON content failed. content: {content}");
			} else {
				return obj;
			}
		}
	}
}