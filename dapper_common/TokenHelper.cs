using dapper_model.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_common
{
	// <summary>
	/// TokenHelper
	/// </summary>
	public static class TokenHelper
	{
		/// <summary>
		/// CacheKey
		/// </summary>
		public static readonly string CacheKey = "_Token";

		/// <summary>
		/// 有效时间
		/// </summary>
		public static readonly TimeSpan Expiry = new TimeSpan(0, 30, 0);

		/// <summary>
		/// 设置
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="prefix"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public static Token<T> Set<T>(string prefix, T data) where T : TokenData
		{
			var sign = string.Format("{0}:{1}_{2}", prefix, data.UserName, data.Mobile);			
			var token = new Token<T>()
			{
				Id = data.UserId,
				Signature = MD5Helper.Encrypt(sign),
				Expiry = DateTime.UtcNow.Add(Expiry),
				Data = data
			};
			RedisHelper.Database.HashSet(CacheKey, token.Signature, token);
			return token;
		}

		/// <summary>
		/// 设置
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static Token<T> Set<T>(T data, string suffix = "") where T : TokenData
		{
			var key = string.Format("{0}-{1}", CacheKey, suffix);
			var sign = string.Format("{0}-{1}", data.UserName, data.Mobile);
			var token = new Token<T>()
			{
				Id = data.UserId,
				Signature = MD5Helper.Encrypt(sign),
				Expiry = DateTime.UtcNow.Add(Expiry),
				Data = data
			};
			RedisHelper.Database.HashSet(key, token.Signature, token);
			return token;
		}

		/// <summary>
		/// 获取
		/// </summary>
		/// <param name="signature">签名</param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static Token<T> Get<T>(string signature, string suffix = "") where T : TokenData
		{
			var key = string.Format("{0}-{1}", CacheKey, suffix);
			return RedisHelper.Database.HashGet<Token<T>>(key, signature);
		}

		/// <summary>
		/// 验证
		/// </summary>
		/// <param name="signature">签名</param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static Token<T> Verify<T>(string signature, string suffix = "") where T : TokenData
		{
			var redis = RedisHelper.Database;
			var key = string.Format("{0}-{1}", CacheKey, suffix);
			var token = redis.HashGet<Token<T>>(key, signature);
			if (token == null || token.Expiry < DateTime.UtcNow)
			{
				return null;
			}
			//续时
			token.Expiry = DateTime.UtcNow.Add(Expiry);
			redis.HashSet(key, token.Signature, token);
			return token;
		}
	}
}
