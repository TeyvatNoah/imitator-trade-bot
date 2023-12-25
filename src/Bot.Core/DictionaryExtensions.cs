namespace Bot.Core;

public static class DictionaryExtensions {
	public static void AddMany<K, V>(this Dictionary<K, V> dict, K key, IEnumerable<V> set) where K: notnull {
		foreach (var item in set) {
			dict.Add(key, item);
		}
	}
}