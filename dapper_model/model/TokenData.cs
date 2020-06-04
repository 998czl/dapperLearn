using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_model.model
{
	public class TokenData
	{
		/// <summary>
		/// 用户Id
		/// </summary>
		public int UserId { get; set; }
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { get; set; }
		/// <summary>
		/// 号码
		/// </summary>
		public string Mobile { get; set; }
	}
}
