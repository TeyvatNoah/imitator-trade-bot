using System.Numerics;
using System.Runtime.CompilerServices;
using Cysharp.Text;

// 非线程安全
// 需要支持remove吗? 不需要吧
public sealed class QueryString: IDisposable {
	private const string _and = "&";
	private const string _eq = "=";
	private Utf16ValueStringBuilder _query = ZString.CreateStringBuilder();
	
	public QueryString() {}
	
	public QueryString(string q) {
		_query.Append(q);
	}
	
	public QueryString(ICollection<KeyValuePair<string, string?>> dict): this() {
		foreach (var (key, value) in dict) {
			Add(key, value);
		}
	}

	// 未来考虑源生成器支持自定义类型序列化为qs
	// public static QueryString From<T>(T obj) {
		
	// }
	
	public QueryString Add(string key, string? value) {
		if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) {
			return this;
		}

		_query.Append(_query.Length != 0 ? ZString.Concat(_and, key, _eq, value) : ZString.Concat(key, _eq, value));
		return this;
	}
	
	public QueryString Add<T>(string key, T? value) where T: INumber<T> {
		if (string.IsNullOrWhiteSpace(key) || value is null) {
			return this;
		}

		_query.Append(_query.Length != 0 ? ZString.Concat(_and, key, _eq, value.ToString()) : ZString.Concat(key, _eq, value.ToString()));
		return this;
	}

	public QueryString Add<T>(string key, T? value, Func<T, string> cb) {
		if (string.IsNullOrWhiteSpace(key) || value is null) {
			return this;
		}

		_query.Append(_query.Length != 0 ? ZString.Concat(_and, key, _eq, cb(value)) : ZString.Concat(key, _eq, cb(value)));
		return this;
	}

	public QueryString Add(string key, string[] arr) {
		if (string.IsNullOrWhiteSpace(key)) {
			return this;
		}
		
		for (var i = 0; i < arr.Length; ++i) {
			if (arr[i] is null) {
				continue;
			}

			_query.Append(_query.Length != 0 ? ZString.Concat(_and, key, _eq, arr[i]) : ZString.Concat(key, _eq, arr[i]));
		}
		return this;
	}

	public QueryString Add<T>(string key, T[] arr) where T: INumber<T> {
		if (string.IsNullOrWhiteSpace(key)) {
			return this;
		}

		for (var i = 0; i < arr.Length; ++i) {
			if (arr[i] is null) {
				continue;
			}
			_query.Append(_query.Length != 0 ? ZString.Concat(_and, key, _eq, arr[i].ToString()) : ZString.Concat(key, _eq, arr[i]).ToString());
		}
		return this;
	}

	public QueryString Add<T>(string key, T[] arr, Func<T, string> cb) {
		if (string.IsNullOrWhiteSpace(key)) {
			return this;
		}
		
		for (var i = 0; i < arr.Length; ++i) {
			if (arr[i] is null) {
				continue;
			}

			_query.Append(_query.Length != 0 ? ZString.Concat(_and, key, _eq, cb(arr[i])) : ZString.Concat(key, _eq, cb(arr[i])));
		}
		return this;
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() {
		return _query.ToString();
	}
	
	public void Dispose() {
		_query.Dispose();
	}
}
