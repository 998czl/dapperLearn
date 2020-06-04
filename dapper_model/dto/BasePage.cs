using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_model.dto
{
	public class BasePage
	{
		public BasePage()
		{
			PageIndex = 1;
			PageSize = 10;
		}

		public int PageIndex { get; set; }

		public int PageSize { get; set; }
	}
}
