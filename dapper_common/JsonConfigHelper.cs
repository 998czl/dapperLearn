using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace dapper_common
{
	/// <summary>
	/// 读取配置文件类（appsettings.json）
	/// </summary>
	public static class JsonConfigHelper
	{
		/// <summary>
		/// 最后写入时间
		/// </summary>
		public static DateTime LastWriteTime { get; private set; }
		/// <summary>
		/// 文件路径
		/// </summary>
		public static readonly string Path = AppDomain.CurrentDomain.BaseDirectory + "appsettings.json";


		/// <summary>
		/// _ConfigurationCollection
		/// </summary>
		private static IDictionary<string, object> _ConfigurationCollection;
		/// <summary>
		/// ConfigurationCollection
		/// </summary>
		public static IDictionary<string, object> ConfigurationCollection
		{
			get
			{
				var file = new FileInfo(Path);
				if (!file.Exists)
				{
					throw new Exception("文件不存在");
				}
				if (LastWriteTime < file.LastWriteTime)
				{
					LastWriteTime = file.LastWriteTime;
					var json = string.Empty;
					using (var sr = new StreamReader(Path, Encoding.UTF8))
					{
						json = sr.ReadToEnd();
					}
					_ConfigurationCollection = JsonHelper.DeserializeObject<IDictionary<string, object>>(json);
					if (_ConfigurationCollection == null)
					{
						throw new Exception("配置文件解析错误");
					}
				}
				return _ConfigurationCollection;
			}
		}

		/// <summary>
		/// ConnectionStrings
		/// </summary>
		public static IDictionary<string, string> ConnectionStrings => Get<string>("ConnectionStrings");


		/// <summary>
		/// ConnectionStrings
		/// </summary>
		public static IDictionary<string, string> Settngs => Get<string>("Setting");
		/// <summary>
		/// 读取
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public static IDictionary<string, T> Get<T>(string key, IDictionary<string, object> source = null)
		{
			if (key.IndexOf('.') > -1)
			{
				var keys = key.Split('.');
				source = Get<object>(keys[0]);
				var childKey = string.Join(".", keys, 1, keys.Length - 1);
				return Get<T>(childKey, source);
			}
			if (source == null)
			{
				source = ConfigurationCollection;
			}
			if (!source.ContainsKey(key))
			{
				throw new Exception("指定键值的配置不存在");
			}
			return JsonHelper.DeserializeObject<IDictionary<string, T>>(source[key].ToString());
		}
	}
}
