using dapper_common;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_core
{
	public class DapperHelper
	{
		/// <summary>
		/// 数据库连接字符串
		/// </summary>
		public static string ConnectionString => JsonConfigHelper.ConnectionStrings["MySQL"];

		public static MySqlConnection MySqlCon()
		{		
			var connection = new MySqlConnection(ConnectionString);
			connection.Open();
			return connection;
		}
	}
}
