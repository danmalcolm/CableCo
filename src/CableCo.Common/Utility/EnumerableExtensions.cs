using System;
using System.Collections.Generic;
using System.Linq;

namespace CableCo.Common.Utility
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Concatenates sequence of strings
		/// </summary>
		/// <param name="sequence"></param>
		/// <param name="separator"></param>
		/// <returns></returns>
		public static string ConcatToString(this IEnumerable<string> sequence, string separator = "")
		{
			return string.Join(separator, sequence.ToArray());
		} 

		public static IEnumerable<Guid> ExcludingEmpty(this IEnumerable<Guid> sequence)
		{
			return sequence.Where(x => x != Guid.Empty);
		}

		public static void Each<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			foreach (var item in sequence)
			{
				action(item);
			}
		}

		public static void EachWithIndex<T>(this IEnumerable<T> sequence, Action<T,int> action)
		{
			int index = 0;
			foreach(var item in sequence)
			{
				action(item, index++);
			}
		}

		/// <summary>
		/// Returns items at every odd or even position in the sequence depending on whether the supplied
		/// index is an even number
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sequence"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static IEnumerable<T> Alternating<T>(this IEnumerable<T> sequence, int index)
		{
			return sequence.Where((x, i) => index%2 == 0 ? i%2 == 0 : i%2 != 0);
		}

		/// <summary>
		/// Appends a single item to the end of the sequence
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sequence"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static IEnumerable<T> Append<T>(this IEnumerable<T> sequence, T item)
		{
			return sequence.Concat(new[] {item});
		}

		/// <summary>
		/// Creates read-only list from a collection
		/// </summary>
		/// <returns></returns>
		public static IList<T> ToReadOnlyList<T>(this IEnumerable<T> collection)
		{
			return collection.ToList().AsReadOnly();
		}

		/// <summary>
		/// Indicates whether a collection is empty, the opposite of Any()
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection"></param>
		/// <returns></returns>
		public static bool None<T>(this IEnumerable<T> collection)
		{
			return !collection.Any();
		}
		
		/// <summary>
		/// Indicates whether a collection does not contain any matching elements, the opposite of Any()
		/// </summary>
		public static bool None<T>(this IEnumerable<T> sequence, Func<T,bool> predicate)
		{
			return !sequence.Any(predicate);
		}
	}
}