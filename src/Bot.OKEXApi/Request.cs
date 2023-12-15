using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

// 源生成器又不会, 运行时反射又不想用, 只能先搓个这东西凑合用
public sealed class Request {
	private readonly HttpClient _client = new() {
		// 30秒超时
		Timeout = new(30 * TimeSpan.TicksPerSecond)
	};
	
	public Request(HttpClient client) {
		_client = client;
	}

	public async Task<T?> FetchWithGenericBody<T>(HttpMethod method, Uri uri, RequestMessage msg, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancelToken) {
		if (cancelToken.IsCancellationRequested) {
			return default;
		}

		var tempUri = new UriBuilder(uri);
		if (msg.Query is not null) {
			// 考虑StringBuilder?
			// 不支持重复key
			tempUri.Query = string.Join("&", msg.Query.Select(kv => $"{UrlEncoder.Default.Encode(kv.Key)}={UrlEncoder.Default.Encode(kv.Value)}"));
		}

		using var req = new HttpRequestMessage(method, tempUri.Uri);

		msg.Headers?.ForEach((_, v) => req.Headers.Add(v.Key, v.Value));
		
		try {
			using var resp = await _client.SendAsync(req, cancelToken);
			
			if (resp is null) {
				return default;
			}
			return await HttpContentJsonExtensions.ReadFromJsonAsync(resp.Content, jsonTypeInfo, cancelToken);
		}
		// 网络, DNS, 证书校验异常
		catch (HttpRequestException e) {
			// TODO 考虑下异常怎么处理
			throw;
		}
		// 超时异常
		catch(TaskCanceledException e) { throw; }
		// 读取response的异常
		catch(HttpIOException e) { throw; }
		// JSON序列化异常
		catch(JsonException e) { throw; }
		
		// var respContent = await resp.Content.ReadAsStreamAsync();
		// return await JsonSerializer.DeserializeAsync<T>(respContent, jsonTypeInfo, cancelToken);
	}

	public async Task<RS?> FetchWithJsonBody<RQ, RS>(HttpMethod method, Uri uri, RequestMessage<RQ> msg, JsonTypeInfo<RQ> reqJsonTypeInfo, JsonTypeInfo<RS> resJsonTypeInfo, CancellationToken cancelToken) {
		if (cancelToken.IsCancellationRequested) {
			return default;
		}

		var tempUri = new UriBuilder(uri);
		if (msg.Query is not null) {
			// 考虑StringBuilder?
			// 不支持重复key
			tempUri.Query = string.Join("&", msg.Query.Select(kv => $"{UrlEncoder.Default.Encode(kv.Key)}={UrlEncoder.Default.Encode(kv.Value)}"));
		}

		using var req = new HttpRequestMessage(method, tempUri.Uri) {
			Content = JsonContent.Create(msg.Content, reqJsonTypeInfo, new("application/json"))
		};

		msg.Headers?.ForEach((_, v) => req.Headers.Add(v.Key, v.Value));
		
		try {
			using var resp = await _client.SendAsync(req, cancelToken);
			
			if (resp is null) {
				return default;
			}
			
			return await HttpContentJsonExtensions.ReadFromJsonAsync(resp.Content, resJsonTypeInfo, cancelToken);
		}
		// 网络, DNS, 证书校验异常
		catch (HttpRequestException e) {
			// TODO 考虑下异常怎么处理
			throw;
		}
		// 超时异常
		catch(TaskCanceledException e) { throw; }
		// 读取response的异常
		catch(HttpIOException e) { throw; }
		// JSON序列化异常
		catch(JsonException e) { throw; }
		
		// var respContent = await resp.Content.ReadAsStreamAsync();
		// return await JsonSerializer.DeserializeAsync(respContent, resJsonTypeInfo, cancelToken);
	}
	
}

public sealed class RequestMessage<T> {
	public HttpHeaders? Headers { get; init; }
	public T? Content { get; init; }
	public ICollection<KeyValuePair<string, string>>? Query { get; init; }
}

public sealed class RequestMessage {
	public HttpHeaders? Headers { get; init; }
	public ICollection<KeyValuePair<string, string>>? Query { get; init; }
}
