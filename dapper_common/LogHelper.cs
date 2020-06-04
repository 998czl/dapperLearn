using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dapper_common
{
	public class LogHelper
	{
		private static ILoggerRepository _Repository;
		/// <summary>
		/// Repository
		/// </summary>
		public static ILoggerRepository Repository
		{
			get
			{
				if (_Repository == null)
				{

					_Repository = LogManager.CreateRepository("Log4net");
					XmlConfigurator.Configure(_Repository, new FileInfo("log4net.config"));
				}
				return _Repository;
			}
		}

		/// <summary>
		/// LogInfo
		/// </summary>
		private static readonly ILog _LogInfo = LogManager.GetLogger(Repository.Name, "LogInfo");
		/// <summary>
		/// LogError
		/// </summary>
		private static readonly ILog _LogError = LogManager.GetLogger(Repository.Name, "LogError");

		/// <summary>
		/// InfoFormat
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void InfoFormat(string format, params object[] args)
		{
			_LogInfo.InfoFormat(format, args);
		}

		/// <summary>
		/// Error
		/// </summary>
		/// <param name="message"></param>
		/// <param name="exception"></param>
		public static void Error(object message, Exception exception)
		{
			_LogError.Error(message, exception);
		}

		/// <summary>
		/// ErrorFormat
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <param name="exception"></param>
		public static void ErrorFormat(string format, params object[] args)
		{
			_LogError.ErrorFormat(format, args);
		}
	}
}
