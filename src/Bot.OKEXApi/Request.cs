using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

// 源生成器又不会, 运行时反射又不想用, 只能先搓个这东西凑合用
public sealed class Request: IDisposable {
	public BeforeSendSerializeAsyncHookReturn? BeforeSendSerializeAsyncHookReturn { get; init; }
	public BeforeSendSerializeAsyncHookNoReturn? BeforeSendSerializeAsyncHookNoReturn { get; init; }
	public BeforeSendSerializeSyncHookReturn? BeforeSendSerializeSyncHookReturn { get; init; }
	public BeforeSendSerializeSyncHookNoReturn? BeforeSendSerializeSyncHookNoReturn { get; init; }
	public AfterSendDeserializeAsyncHookReturn? AfterSendDeserializeAsyncHookReturn { get; init; }
	public AfterSendDeserializeAsyncHookNoReturn? AfterSendDeserializeAsyncHookNoReturn { get; init; }
	public AfterSendDeserializeSyncHookReturn? AfterSendDeserializeSyncHookReturn { get; init; }
	public AfterSendDeserializeSyncHookNoReturn? AfterSendDeserializeSyncHookNoReturn { get; init; }
	

	public BeforeResponseSerializeAsyncHookReturn? BeforeResponseSerializeAsyncHookReturn { get; init; }
	public BeforeResponseSerializeAsyncHookNoReturn? BeforeResponseSerializeAsyncHookNoReturn { get; init; }
	public BeforeResponseSerializeSyncHookReturn? BeforeResponseSerializeSyncHookReturn { get; init; }
	public BeforeResponseSerializeSyncHookNoReturn? BeforeResponseSerializeSyncHookNoReturn { get; init; }
	public AfterResponseDeserializeAsyncHookReturn? AfterResponseDeserializeAsyncHookReturn { get; init; }
	public AfterResponseDeserializeAsyncHookNoReturn? AfterResponseDeserializeAsyncHookNoReturn { get; init; }
	public AfterResponseDeserializeSyncHookReturn? AfterResponseDeserializeSyncHookReturn { get; init; }
	public AfterResponseDeserializeSyncHookNoReturn? AfterResponseDeserializeSyncHookNoReturn { get; init; }

	private readonly HttpClient _client = new() {
		// 30秒超时
		Timeout = new(30 * TimeSpan.TicksPerSecond)
	};
	
	public Request() {}
	
	public Request(HttpClient client) {
		_client = client;
	}

	public async Task<T?> FetchWithGenericBody<T>(HttpMethod method, Uri uri, RequestMessage msg, JsonTypeInfo<T> jsonTypeInfo, CancellationToken cancelToken) {

		var tempUri = new UriBuilder(uri) {
			// 考虑StringBuilder?
			// 不支持重复key
			// tempUri.Query = string.Join("&", msg.Query.Select(kv => $"{UrlEncoder.Default.Encode(kv.Key)}={UrlEncoder.Default.Encode(kv.Value)}"));
			Query = msg.Query?.ToString() ?? ""
		};

		using var req = new HttpRequestMessage(method, tempUri.Uri);

		msg.Headers?.ForEach(v => req.Headers.Add(v.Key, v.Value));
		

		HttpRequestMessage? newReq = null;
		// 无论是否有body,这几个都是要调用的
		BeforeSendSerializeSyncHookNoReturn?.Invoke(req, cancelToken);
		// 考虑到不完全等价还是if算了
		if (BeforeSendSerializeAsyncHookNoReturn is not null) {
			await BeforeSendSerializeAsyncHookNoReturn(req, cancelToken);
		}
		// BeforeSendSerializeAsyncHookNoReturn?.Invoke(req, cancelToken).GetAwaiter().GetResult();
		newReq = BeforeSendSerializeSyncHookReturn?.Invoke(req, cancelToken);
		if (BeforeSendSerializeAsyncHookReturn is not null) {
			newReq = await BeforeSendSerializeAsyncHookReturn(req, cancelToken);
		}
		// newReq = BeforeSendSerializeAsyncHookReturn?.Invoke(req, cancelToken).GetAwaiter().GetResult();
		

		var nnewReq = newReq ?? req;

		// 这里要么不带body要么不需要序列化但是为了保证一致所以还是执行此hook了
		// 首先multipart和steram不需要管序列化排除掉,至于ByteArrayContent,这里也没办法序列化
		AfterSendDeserializeSyncHookNoReturn?.Invoke("", nnewReq, cancelToken);
		if (AfterSendDeserializeAsyncHookNoReturn is not null) {
			await AfterSendDeserializeAsyncHookNoReturn("", nnewReq, cancelToken);
			
		}
		// AfterSendDeserializeAsyncHookNoReturn?.Invoke("", nnewReq, cancelToken).GetAwaiter().GetResult();
		nnewReq = AfterSendDeserializeSyncHookReturn?.Invoke("", nnewReq, cancelToken) ?? nnewReq;
		if (AfterSendDeserializeAsyncHookReturn is not null) {
			nnewReq = await AfterSendDeserializeAsyncHookReturn("", nnewReq, cancelToken);
		}
		// nnewReq = AfterSendDeserializeAsyncHookReturn?.Invoke("", nnewReq, cancelToken).GetAwaiter().GetResult();

		
		// 这里要么不带body要么不需要序列化所以不执行此hook了
		// 首先multipart和steram不需要管序列化排除掉,至于ByteArrayContent,这里也没办法序列化
		// if (nnewReq.Content is JsonContent) {
		// 	if (AfterSendDeserializeSyncHookNoReturn is not null) {
		// 		// TODO 安全吗? 讲道理到这里content肯定是有的, 至于Value是Object是不是string需要确认
		// 		AfterSendDeserializeSyncHookNoReturn(((nnewReq.Content as JsonContent)!.Value as string)!, req, cancelToken);
		// 	}
		// 	if (AfterSendDeserializeAsyncHookNoReturn is not null) {
		// 		await AfterSendDeserializeAsyncHookNoReturn(((nnewReq.Content as JsonContent)!.Value as string)!, req, cancelToken);
		// 	}
		// 	if (AfterSendDeserializeSyncHookReturn is not null) {
		// 		newReq = AfterSendDeserializeSyncHookReturn(((nnewReq.Content as JsonContent)!.Value as string)!, req, cancelToken);
		// 	}
		// 	if (AfterSendDeserializeSyncHookReturn is not null) {
		// 		newReq = AfterSendDeserializeSyncHookReturn(((nnewReq.Content as JsonContent)!.Value as string)!, req, cancelToken);
		// 	}
		// }
		
		
		HttpResponseMessage? newResp = null;
		try {
			using var resp = await _client.SendAsync(nnewReq, cancelToken);

			newResp = resp;
			
			BeforeResponseSerializeSyncHookNoReturn?.Invoke(req, resp, cancelToken);
			if (BeforeResponseSerializeAsyncHookNoReturn is not null) {
				await BeforeResponseSerializeAsyncHookNoReturn(req, resp, cancelToken);
			}
			newResp = BeforeResponseSerializeSyncHookReturn?.Invoke(req, resp, cancelToken) ?? newResp;
			if (BeforeResponseSerializeAsyncHookReturn is not null) {
				newResp = await BeforeResponseSerializeAsyncHookReturn(req, resp, cancelToken);
			}
			
			var result = await HttpContentJsonExtensions.ReadFromJsonAsync(newResp.Content, jsonTypeInfo, cancelToken);

			AfterResponseDeserializeSyncHookNoReturn?.Invoke(result, req, newResp, cancelToken);
			if (AfterResponseDeserializeAsyncHookNoReturn is not null) {
				await AfterResponseDeserializeAsyncHookNoReturn(result, req, newResp, cancelToken);
			}
			if (AfterResponseDeserializeSyncHookReturn is not null) {
				result = (T)AfterResponseDeserializeSyncHookReturn(result, req, newResp, cancelToken);
			}
			if (AfterResponseDeserializeAsyncHookReturn is not null) {
				result = (T)await AfterResponseDeserializeAsyncHookReturn(result, req, newResp, cancelToken);
			}
			return result;
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
		finally {
			// 只能手动一下了
			newReq?.Dispose();
			newResp?.Dispose();
		}
		
		
		// var respContent = await resp.Content.ReadAsStreamAsync();
		// return await JsonSerializer.DeserializeAsync<T>(respContent, jsonTypeInfo, cancelToken);
	}

	public async Task<RS?> FetchWithJsonBody<RQ, RS>(HttpMethod method, Uri uri, RequestMessage<RQ> msg, JsonTypeInfo<RQ> reqJsonTypeInfo, JsonTypeInfo<RS> resJsonTypeInfo, CancellationToken cancelToken) {

		var tempUri = new UriBuilder(uri) {
			// if (msg.Query is not null) {
			// 考虑StringBuilder?
			// 不支持重复key
			// tempUri.Query = string.Join("&", msg.Query.Select(kv => $"{UrlEncoder.Default.Encode(kv.Key)}={UrlEncoder.Default.Encode(kv.Value)}"));
			Query = msg.Query?.ToString() ?? ""
		};
		// }

		using var req = new HttpRequestMessage(method, tempUri.Uri);

		msg.Headers?.ForEach((KeyValuePair<string, IEnumerable<string>> v) => req.Headers.Add(v.Key, v.Value));

		
		HttpRequestMessage? newReq = null;
		// 无论是否有body,这几个都是要调用的
		// if (BeforeSendSerializeSyncHookNoReturn is not null) {
		BeforeSendSerializeSyncHookNoReturn?.Invoke(req, cancelToken);
		// }
		if (BeforeSendSerializeAsyncHookNoReturn is not null) {
			await BeforeSendSerializeAsyncHookNoReturn(req, cancelToken);
		}
		// if (BeforeSendSerializeSyncHookReturn is not null) {
		newReq = BeforeSendSerializeSyncHookReturn?.Invoke(req, cancelToken);
		// }
		if (BeforeSendSerializeAsyncHookReturn is not null) {
			newReq = await BeforeSendSerializeAsyncHookReturn(req, cancelToken);
		}

		
		var nnewReq = newReq ?? req;
		
		if (msg.Content is not null) {
			var content = JsonSerializer.SerializeToUtf8Bytes(msg.Content, reqJsonTypeInfo);
			// nnewReq.Content = JsonContent.Create(msg.Content, reqJsonTypeInfo, new("application/json"));
			nnewReq.Content = new ByteArrayContent(content);
			// 不是,这你妈会抛异常?怎么管这么宽啊,不让人自定义请求头的意思
			// nnewReq.Headers.Add("Content-Type", "application/json");
			// var test = nnewReq.Headers.TryAddWithoutValidation("Content-Type", ["application/json"]);
			nnewReq.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			
			var contentStr = Encoding.UTF8.GetString(content);

			AfterSendDeserializeSyncHookNoReturn?.Invoke(contentStr, nnewReq, cancelToken);
			if (AfterSendDeserializeAsyncHookNoReturn is not null) {
				await AfterSendDeserializeAsyncHookNoReturn(contentStr, nnewReq, cancelToken);
			}
			nnewReq = AfterSendDeserializeSyncHookReturn?.Invoke(contentStr, nnewReq, cancelToken) ?? nnewReq;
			if (AfterSendDeserializeAsyncHookReturn is not null) {
				nnewReq = await AfterSendDeserializeAsyncHookReturn(contentStr, nnewReq, cancelToken);
			}
		}
		
		
		HttpResponseMessage? newResp = null;
		try {
			using var resp = await _client.SendAsync(nnewReq ?? req, cancelToken);
			
			newResp = resp;
			
			BeforeResponseSerializeSyncHookNoReturn?.Invoke(nnewReq!, resp, cancelToken);
			if (BeforeResponseSerializeAsyncHookNoReturn is not null) {
				await BeforeResponseSerializeAsyncHookNoReturn(nnewReq!, resp, cancelToken);
			}
			newResp = BeforeResponseSerializeSyncHookReturn?.Invoke(nnewReq!, resp, cancelToken) ?? newResp;
			if (BeforeResponseSerializeAsyncHookReturn is not null) {
				newResp = await BeforeResponseSerializeAsyncHookReturn(nnewReq!, resp, cancelToken);
			}
			
			
			var result = await HttpContentJsonExtensions.ReadFromJsonAsync(newResp.Content, resJsonTypeInfo, cancelToken);

			AfterResponseDeserializeSyncHookNoReturn?.Invoke(result, nnewReq!, newResp, cancelToken);
			if (AfterResponseDeserializeAsyncHookNoReturn is not null) {
				await AfterResponseDeserializeAsyncHookNoReturn(result, nnewReq!, newResp, cancelToken);
			}
			if (AfterResponseDeserializeSyncHookReturn is not null) {
				result = (RS)AfterResponseDeserializeSyncHookReturn(result, nnewReq!, newResp, cancelToken);
			}
			if (AfterResponseDeserializeAsyncHookReturn is not null) {
				result = (RS)await AfterResponseDeserializeAsyncHookReturn(result, nnewReq!, newResp, cancelToken);
			}
			return result;
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
		finally {
			newReq?.Dispose();
			newResp?.Dispose();
		}
		
		// var respContent = await resp.Content.ReadAsStreamAsync();
		// return await JsonSerializer.DeserializeAsync(respContent, resJsonTypeInfo, cancelToken);
	}
	
	public void Dispose() {
		_client?.Dispose();
	}
	
}

public sealed class RequestMessage<T>: IDisposable {
	public HttpHeaders? Headers { get; init; }
	public T? Content { get; init; }
	// public ICollection<KeyValuePair<string, string>>? Query { get; init; }
	public QueryString? Query { get; init; }
	
	public void Dispose() {
		Query?.Dispose();
	}
}

public sealed class RequestMessage: IDisposable {
	public HttpHeaders? Headers { get; init; }
	// public ICollection<KeyValuePair<string, string>>? Query { get; init; }
	public QueryString? Query { get; init; }

	public void Dispose() {
		Query?.Dispose();
	}
}


// 没有union type...
public delegate Task<HttpRequestMessage> BeforeSendSerializeAsyncHookReturn(HttpRequestMessage req, CancellationToken cancellationToken);
public delegate Task BeforeSendSerializeAsyncHookNoReturn(HttpRequestMessage req, CancellationToken cancellationToken);
public delegate HttpRequestMessage BeforeSendSerializeSyncHookReturn(HttpRequestMessage req, CancellationToken cancellationToken);
public delegate void BeforeSendSerializeSyncHookNoReturn(HttpRequestMessage req, CancellationToken cancellationToken);


public delegate Task<HttpRequestMessage> AfterSendDeserializeAsyncHookReturn(string content, HttpRequestMessage req, CancellationToken cancellationToken);
public delegate Task AfterSendDeserializeAsyncHookNoReturn(string content, HttpRequestMessage req, CancellationToken cancellationToken);
public delegate HttpRequestMessage AfterSendDeserializeSyncHookReturn(string content, HttpRequestMessage req, CancellationToken cancellationToken);
public delegate void AfterSendDeserializeSyncHookNoReturn(string content, HttpRequestMessage req, CancellationToken cancellationToken);

public delegate Task<HttpResponseMessage> BeforeResponseSerializeAsyncHookReturn(HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);
public delegate Task BeforeResponseSerializeAsyncHookNoReturn(HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);
public delegate HttpResponseMessage BeforeResponseSerializeSyncHookReturn(HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);
public delegate void BeforeResponseSerializeSyncHookNoReturn(HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);


// 不希望Request泛型只能object,调用处自行保证运行时类型一致
public delegate Task<object> AfterResponseDeserializeAsyncHookReturn(object? content, HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);
public delegate Task AfterResponseDeserializeAsyncHookNoReturn(object? content, HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);
public delegate object AfterResponseDeserializeSyncHookReturn(object? content, HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);
public delegate void AfterResponseDeserializeSyncHookNoReturn(object? content, HttpRequestMessage req, HttpResponseMessage res, CancellationToken cancellationToken);
