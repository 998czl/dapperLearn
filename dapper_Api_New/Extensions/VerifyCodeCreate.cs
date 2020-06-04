using dapper_common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dapper_Api_New.Extensions
{
	/// <summary>
	/// 验证码
	/// </summary>
	public static class VerifyCodeCreate
	{
		/// <summary>
		/// 生成图片验证码
		/// </summary>
		/// <param name="httpResponse"></param>
		/// <param name="guid"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static void GenerateImageCode(this HttpResponse httpResponse, string guid, int length = 4)
		{
			var bytes = VerifyCodeHelper.GenerateImageCode(guid, length);
			httpResponse.Headers.Add("Access-Control-Expose-Headers", "X-VGuid");
			httpResponse.Headers.Add("X-VGuid", guid);
			httpResponse.ContentType = "image/png";
			httpResponse.StatusCode = StatusCodes.Status200OK;
			httpResponse.Body.WriteAsync(bytes, 0, bytes.Length);
		}

		/// <summary>
		/// 验证图片验证码
		/// </summary>
		/// <param name="httpRequest"></param>
		public static void VerifyImageCode(this HttpRequest httpRequest)
		{
			httpRequest.Headers.TryGetValue("X-VGuid", out var guid);
			httpRequest.Headers.TryGetValue("X-VCode", out var code);
			VerifyCodeHelper.VerifyImageCode(guid, code);
		}
	}
}
