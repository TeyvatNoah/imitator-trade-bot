using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

// 非线程安全
// 需要支持remove吗? 不需要吧
public sealed class QueryString {
	private readonly StringBuilder _query;
	
	public QueryString() {
		_query = new();
	}
	
	public QueryString(string q) {
		_query = new(q);
	}
	
	public QueryString(ICollection<KeyValuePair<string, string>> dict): this() {
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

		_query.Append($"{key}={value}&");
		return this;
	}
	
	public QueryString Add<T>(string key, T? value) where T: INumber<T> {
		if (string.IsNullOrWhiteSpace(key) || value is null) {
			return this;
		}

		_query.Append($"{key}={value}&");
		return this;
	}

	public QueryString Add<T>(string key, T? value, Func<T, string> cb) {
		if (string.IsNullOrWhiteSpace(key) || value is null) {
			return this;
		}

		_query.Append($"{key}={cb(value)}&");
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

			_query.Append($"{key}={arr[i]}&");
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
			_query.Append($"{key}={arr[i]}&");
		}
		return this;
	}

	public QueryString Add<T>(string key, T[] arr, Func<T, string> cb) where T: INumber<T> {
		if (string.IsNullOrWhiteSpace(key)) {
			return this;
		}
		
		for (var i = 0; i < arr.Length; ++i) {
			if (arr[i] is null) {
				continue;
			}

			_query.Append($"{key}={cb(arr[i])}&");
		}
		return this;
	}
	
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override string ToString() {
		return _query.Remove(_query.Length - 1, 1).ToString();
	}
}
