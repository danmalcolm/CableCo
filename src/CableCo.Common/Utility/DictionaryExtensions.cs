using System;
using System.Collections;
using System.Collections.Generic;

namespace CableCo.Common.Utility
{
	/// <summary>
	/// Extension methods for working with generic IDictionary instances
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Returns value from dictionary with specified key. If the value does not exist, then the 
		/// a new value is created using the supplied delegate and added to the dictionary.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="source"></param>
		/// <param name="key"></param>
		/// <param name="createValue"></param>
		/// <returns></returns>
		public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, Func<TValue> createValue)
		{
			TValue value;
			if (source.TryGetValue(key, out value))
				return value;
			value = createValue();
			source.Add(key, value);
			return value;
		}

		/// <summary>
		/// Returns value from dictionary with specified key. If the value does not exist, then the 
		/// a new value is created using the supplied delegate and added to the dictionary.
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="source"></param>
		/// <param name="key"></param>
		/// <param name="createValue"></param>
		/// <returns></returns>
		public static TValue GetOrAdd<TValue>(this IDictionary source, object key, Func<TValue> createValue)
		{
			TValue value;
			if (source.Contains(key))
				return (TValue)source[key];
			value = createValue();
			source.Add(key, value);
			return value;
		}

		/// <summary>
		/// Returns value from dictionary with specified key. If the value does not exist, then the 
		/// a default value for the type is returned
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="source"></param>
		/// <param name="key"></param>
		/// <param name="createValue"></param>
		/// <returns></returns>
		public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
		{
			return source.GetOrDefault(key, default(TValue));
		}

		/// <summary>
		/// Returns value from dictionary with specified key. If the value does not exist, then the 
		/// a default value for the type is returned
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="source"></param>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue defaultValue)
		{
			TValue value;
			if (source.TryGetValue(key, out value))
				return value;
			return defaultValue;
		}

		/// <summary>
		/// Returns value from dictionary with specified key. If the value does not exist, then the 
		/// a default value for the type is returned
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="source"></param>
		/// <param name="key"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, Func<TValue> create)
		{
			TValue value;
			if (source.TryGetValue(key, out value))
				return value;
			return create();
		}
	}
}