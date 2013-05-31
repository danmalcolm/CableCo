using System;

namespace CableCo.Common.Utility
{
	/// <summary>
	/// Exception that is thrown when a value in the application configuration
	/// file is not valid. 
	/// </summary>
	public class InvalidConfigurationException : Exception
	{

		#region Constructors

		public InvalidConfigurationException() : base() {}
		
		public InvalidConfigurationException(string message) : base(message) {}

		public InvalidConfigurationException(string message, Exception innerException) : base(message, innerException) { }
        
		#endregion


	}
}