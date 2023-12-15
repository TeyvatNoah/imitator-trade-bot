// using System.Text.Json;
// using System.Net.Http.Json;
// using System.Text.Json.Serialization;

// using RestEase;
// using System.Net.Http.Headers;

// public sealed class ApiRequestBodyDeserializer: RequestBodySerializer {
// 	public readonly static JsonSerializerOptions Options = new() {
// 		NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
// 	};
	
// 	public readonly static MediaTypeHeaderValue MediaType = new("application/json", "utf-8");

// 	public override HttpContent? SerializeBody<T>(T body, RequestBodySerializerInfo info) {
// 		return JsonContent.Create<T>(body, MediaType, Options);
// 	}
// }