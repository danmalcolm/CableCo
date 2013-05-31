using System;
using System.ComponentModel;
using System.Configuration;

namespace CableCo.Common.Utility
{
	public class ConfigurationUtility
	{
		private const string MissingExceptionMessageFmt =
			@"The required application setting ""{0}"" is missing from the application configuration file";

		private const string DirectoryDoesNotExistExceptionMessageFmt =
			@"The directory specified in application setting ""{0}"" in the application configuration file does not exist";

		private const string FileDoesNotExistExceptionMessageFmt =
					@"The file specified in the application setting ""{0}"" in the application configuration file does not exist";

		/// <summary>
		/// Returns value from AppSettings section of the currently running application's
		/// configuration file. An exception will be thrown if the setting does not exist.
		/// </summary>
		/// <param name="name">Name of item in configuration file's AppSettings section</param>
		/// <returns>Value in configuration file</returns>
		public static string ReadAppSetting(string name)
		{
			return FromAppSetting(name, value => value);
		}

		/// <summary>
		/// Returns value from AppSettings section of the currently running application's
		/// configuration file or an alternative value if the setting is not defined.
		/// </summary>
		/// <param name="name">Name of item in configuration file's AppSettings section</param>
		/// <param name="defaultValue">Value to return if item does not exist</param>
		/// <returns>Value in configuration file</returns>
		public static string ReadAppSettingOrDefault(string name, string defaultValue)
		{
			return FromAppSettingOrDefault(name, value => value, defaultValue);
		}

		/// <summary>
		/// Returns value converted to specified type from AppSettings section of the currently running 
		/// application's configuration file.
		/// </summary>
		/// <typeparam name="T">Type to which settig should be converted</typeparam>
		/// <param name="name">Name of item in configuration file's AppSettings section</param>
		/// <returns></returns>
		public static T ReadAppSetting<T>(string name)
		{
			Type destinationType = typeof(T);
			string value = ReadAppSetting(name);
			return ConvertSetting<T>(name, destinationType, value);
		}

		/// <summary>
		/// Returns value converted to specified type from AppSettings section of the currently running 
		/// application's configuration file.
		/// </summary>
		/// <typeparam name="T">Type to which settig should be converted</typeparam>
		/// <param name="name">Name of item in configuration file's AppSettings section</param>
		/// <param name="defaultValue">The value to return if no app setting is specified</param>
		/// <returns></returns>
		public static T ReadAppSettingOrDefault<T>(string name, T defaultValue)
		{
			return FromAppSettingOrDefault(name, value => ConvertSetting<T>(name, typeof(T), value), defaultValue);
		}

		private static T ConvertSetting<T>(string name, Type destinationType, string value)
		{
			TypeConverter convertor = TypeDescriptor.GetConverter(destinationType);
			if (convertor == null)
			{
				string message = string.Format
					(@"Cannot convert from string to type {0}.",
					 typeof(T));
				throw new ArgumentException(message, destinationType.ToString());
			}
			T convertedValue;
			try
			{
				convertedValue = (T)(convertor.ConvertFromInvariantString(value));
			}
			catch (Exception exception)
			{
				string message = string.Format
					(@"Cannot convert appSetting ""{0}"" to a value of type {1}.", name,
					 typeof(T));
				throw new InvalidConfigurationException(message, exception);
			}
			return convertedValue;
		}
		
		/// <summary>
		/// Reads a connection string from the &quot;connectionStrings&quot; section
		/// of the application configuration file. The name of the connection string is first looked up
		/// from an entry within the &quot;appSettings&quot; section of the configuration file.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string ReadConnectionStringFromNameInAppSetting(string name)
		{
			string connectionStringName = ReadAppSetting(name);
			return ReadConnectionString
				(connectionStringName);
		}

		/// <summary>
		/// Reads the connection string from the &quot;connectionStrings&quot; section
		/// of the application configuration file.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string ReadConnectionString(string name)
		{
			var connectionString = ConfigurationManager.ConnectionStrings[name];
			if (connectionString == null)
			{
				string message = string.Format(@"A connection string named ""{0}"" does not exist in the application's configuration file", name);
				throw new InvalidConfigurationException(message);
			}
			return connectionString.ConnectionString;
		}

		/// <summary>
		/// Utility method to do something with setting if it exists, or throw if not
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="convert"></param>
		/// <returns></returns>
		private static T FromAppSetting<T>(string name, Func<string, T> convert)
		{
			string value;
			if (!TryReadAppSetting(name, out value))
			{
				string message = string.Format(MissingExceptionMessageFmt, name);
				throw new InvalidConfigurationException(message);
			}
			return convert(value);
		}

		/// <summary>
		/// Utility method to do something with setting if it exists, or return default value if
		/// it doesn't
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <param name="convert"></param>
		/// <returns></returns>
		private static T FromAppSettingOrDefault<T>(string name, Func<string, T> convert, T @default)
		{
			string value = ConfigurationManager.AppSettings[name];
			if (value == null)
			{
				return @default;
			}
			return convert(value);
		}

		private static bool TryReadAppSetting(string name, out string value)
		{
			value = ConfigurationManager.AppSettings[name];
			return value != null;
		}
	}
}
