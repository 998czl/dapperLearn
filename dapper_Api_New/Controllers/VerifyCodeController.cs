using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dapper_Api_New.Extensions;
using dapper_common.Utile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dapper_Api_New.Controllers
{
	/// <summary>
	/// 图片验证码
	/// </summary>
	[Produces(HttpContentType.Url)]
	public class VerifyCodeController : ApiController
	{
		/// <summary>
		/// 生成图片验证码
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		[HttpGet("/api/VerifyCode/CreateVerifyCode")]
		public void CreateVerifyCode(string guid)
		{
			Response.GenerateImageCode(guid);
		}
	}
}
