using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_model.dto
{
	public class Administrator
	{
		public int UserId { get; set; }

		public string UserName { get; set; }

		public string Mobile { get; set; }

		public string RealName { get; set; }

		public int UserState { get; set; }

		public string UserPassword { get; set; }

		public DateTime CreateTime { get; set; }

		public int RoleId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int ShowMobile { get; set; }
	}
}
