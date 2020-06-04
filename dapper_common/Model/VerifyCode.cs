using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_common.Model
{
	/// <summary>
	/// 验证码
	/// </summary>
	public class VerifyCode
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="code"></param>
		public VerifyCode(string guid = "", string code = "")
		{
			Guid = guid;
			Code = code;
			CreateTime = DateTime.UtcNow;
		}

		/// <summary>
		/// 唯一标识
		/// </summary>
		public string Guid { get; set; }
		/// <summary>
		/// 验证码
		/// </summary>
		public string Code { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }
	}
}
