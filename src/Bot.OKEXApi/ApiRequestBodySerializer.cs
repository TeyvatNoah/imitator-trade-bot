
using System.Net.Http.Json;

using RestEase;

public sealed class ApiRequestBodyDeserializer: RequestBodySerializer {
	public override HttpContent? SerializeBody<T>(T body, RequestBodySerializerInfo info) {
		return JsonContent.Create(body);
	}
}