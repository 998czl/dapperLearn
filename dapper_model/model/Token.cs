using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_model.model
{
	public class Token<T>
	{
		/// <summary>
		/// 数据
		/// </summary>
		public T Data { get; set; }

		/// <summary>
		/// 用户名对应签名Token
		/// </summary>
		public string Signature { get; set; }

		/// <summary>
		/// Token过期时间
		/// </summary>
		public DateTime Expiry { get; set; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public int Id { get; set; }
	}
}
