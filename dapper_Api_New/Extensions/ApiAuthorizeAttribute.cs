using dapper_common;
using dapper_model.dto;
using dapper_model.model;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_Api_New.Extensions
{
	/// <summary>
	/// 验证属性
	/// </summary>
	public class ApiAuthorizeAttribute: ActionFilterAttribute
	{
		/// <summary>
		/// 是否验证Token
		/// </summary>
		public bool VerifyToken { get; set; }
		/// <summary>
		/// 是否验证权限
		/// </summary>
		public bool VerifyRight { get; set; }

		/// <summary>
		/// 是否验证图片验证码
		/// </summary>
		public bool VerifyCode { get; set; }

		/// <summary>
		/// OnActionExecutionAsync
		/// </summary>
		/// <param name="context"></param>
		/// <param name="next"></param>
		/// <returns></returns>
		public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (VerifyCode)//登陆验证码
			{
				context.HttpContext.Request.VerifyImageCode();
			}

			if (VerifyToken)//验证Token (传入签名)
			{
				var token = context.HttpContext.Request.Headers["X-Token"].FirstOrDefault();
				if (string.IsNullOrEmpty(token))
				{
					throw new CustomException("login_timeout");
				}
				var loginInfo = VerifyLoginInfo(token);
				if (loginInfo == null)
				{
					throw new CustomException("login_timeout");
				}
				if (VerifyRight)
				{					
					if (loginInfo.Data.UserName != "administrator")
					{
						throw new CustomException("没有访问权限");
					}
				}
			}			
			return base.OnActionExecutionAsync(context, next);
		}

		/// <summary>
		/// 验证登录信息
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		private Token<LoginInfo> VerifyLoginInfo(string token)
		{
			return TokenHelper.Verify<LoginInfo>(token, "User");
		}
	}
}
