using dapper_model.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_model.dto
{
	public class LoginInfo : TokenData
	{
		public string RealName { get; set; }

		public int UserState { get; set; }

		public DateTime CreateTime { get; set; }

		public int RoleId { get; set; }
	}
}
