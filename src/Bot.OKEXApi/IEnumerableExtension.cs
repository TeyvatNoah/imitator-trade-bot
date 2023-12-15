public static class IEnumerableExtensions {
	public static void Foreach<T>(this IEnumerable<T> ien, Action<int> cb) {
		var i = 0;
		foreach (var item in ien) {
			cb(i++);
		}
	}

	public static void ForEach<T>(this IEnumerable<T> ien, Action<int, T> cb) {
		var i = 0;
		foreach (var item in ien) {
			cb(i, item);
		}
	}
}