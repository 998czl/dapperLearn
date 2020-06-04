using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_common
{
	public static class JsonHelper
	{
		/// <summary>
		/// Settings
		/// </summary>
		public static JsonSerializerSettings Settings = new JsonSerializerSettings
		{
			ReferenceLoopHandling = ReferenceLoopHandling.Ignore
		};

		/// <summary>
		/// 序列化
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string SerializeObject<T>(T obj)
		{
			return JsonConvert.SerializeObject(obj, Settings);
		}

		/// <summary>
		/// 反序列化
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="json"></param>
		/// <returns></returns>
		public static T DeserializeObject<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, Settings);
		}
	}
}
