using System;

namespace CableCo.Common.Utility
{

	// non-generic class used to improve creation syntax (fewer angle brackets)
	
	/// <summary>
	/// Provides methods for creation of ThreadSafeInitializer&lt;T&gt; objects
	/// </summary>
	public static class ThreadSafeInitializer
	{
		/// <summary>
		/// Creates a new instance of ThreadSafeInitializer&lt;T&gt;
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="create"></param>
		/// <returns></returns>
		public static ThreadSafeInitializer<T> Create<T>(Func<T> create)
		{
			return new ThreadSafeInitializer<T>(create);
		}
	}

	/// <summary>
	/// Provides access to a value that is initialized lazily (when the Value property
	/// is accessed) using the specified delegate. Locking is used to ensure that only 
	/// a single thread can initialise the value. If an exception is thrown during
	/// initialisation, subsequent reads of the Value property will result in further
	/// attempts to initialise the value.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ThreadSafeInitializer<T>
	{
		private T value;
		private readonly object @lock = new object();
		private readonly Func<T> create;
		private bool created;

		public ThreadSafeInitializer(Func<T> create)
		{
			if (create == null) throw new ArgumentNullException("create");
			this.create = create;
		}

		public T Value
		{
			get
			{
				if (!created)
				{
					lock (@lock)
					{
						if (!created)
						{
							value = create();
							created = true;
						}
					}
				}
				return value;
			}
		}
	}
}