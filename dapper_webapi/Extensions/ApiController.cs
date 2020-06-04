using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dapper_common.Utile;
using dapper_webapi.Model;
using Microsoft.AspNetCore.Mvc;

namespace dapper_webapi.Extensions
{
	public class ApiController : ControllerBase
	{
		/// <summary>
		/// Token
		/// </summary>
		private string _Token;

		/// <summary>
		/// Token
		/// </summary>
		public string Token
		{
			get
			{
				if (_Token == null)
				{
					_Token = Request.Headers["X-Token"].FirstOrDefault();
				}
				return _Token;
			}
		}

		/// <summary>
		/// Ok
		/// </summary>
		/// <param name="message">消息</param>
		/// <returns></returns>
		protected ReturnResult<string> Ok(string message = null)
		{
			return Content("", HttpContentType.Json, message);
		}

		/// <summary>
		/// Json
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="content">内容</param>
		/// <param name="message">消息</param>
		/// <returns></returns>
		protected ReturnResult<T> Json<T>(T content, string message = null)
		{
			return Content(content, HttpContentType.Json, message);
		}

		/// <summary>
		/// Content
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="content">内容</param>
		/// <param name="contentType">内容类型</param>
		/// <param name="message">消息</param>
		/// <returns></returns>
		protected ReturnResult<T> Content<T>(T content, string contentType, string message = "success")
		{
			if (string.IsNullOrEmpty(message))
			{
				message = "success";
			}
			var code = message == "success" ? ReturnCode.OK : ReturnCode.CustomException;
			return new ReturnResult<T>(Request, code, message, content, contentType);
		}
	}
}
