using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_core
{
	public class MTSException : ApplicationException
	{
		public MTSException(string message) : base(message)
		{
		}
	}
}
