using dapper_common.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace dapper_common
{
	/// <summary>
	/// VerifyCodeHelper
	/// </summary>
	public static class VerifyCodeHelper
	{
		private const string Bound = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz2345678";
		private const string BoundOfDigital = "0123456789";
		private static readonly string[] Fonts = new string[] { "Helvetica", "Geneva", "sans-serif", "Verdana", "Times New Roman", "Courier New", "Arial" };
		private static readonly Random Random = new Random();
		/// <summary>
		/// 有效时间
		/// </summary>
		private static readonly TimeSpan Expiry = new TimeSpan(0, 3, 0);
		/// <summary>
		/// 缓存键
		/// </summary>
		private static readonly string CacheKey = "VerifyCode";

		/// <summary>
		/// 生成图片验证码
		/// </summary>
		/// <param name="httpRequest"></param>
		/// <param name="guid"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static byte[] GenerateImageCode(string guid, int length = 4)
		{
			if (string.IsNullOrEmpty(guid))
			{
				throw new Exception("唯一代码不可为空");
			}
			//var redis = RedisHelper.Database;
			//if (redis.HashExists(CacheKey, guid))
			//{
			//	throw new Exception("唯一代码已存在");
			//}
			var bytes = GenerateImageCode(length, out var code);
			var data = new VerifyCode(guid, code);
			//redis.HashSet(CacheKey, guid, data);
			return bytes;
		}

		/// <summary>
		/// 验证图片验证码
		/// </summary>
		/// <param name="guid"></param>
		/// <param name="code"></param>
		public static void VerifyImageCode(string guid, string code)
		{
			if (string.IsNullOrEmpty(guid))
			{
				throw new Exception("唯一代码不可为空");
			}
			var data = RedisHelper.Database.HashGet<VerifyCode>(CacheKey, guid.ToString());
			if (data == null)
			{
				throw new Exception("验证码错误");
			}
			if (data.CreateTime.Add(Expiry) < DateTime.UtcNow)
			{
				throw new Exception("验证码超时");
			}
			if (string.Compare(data.Code, code, StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new Exception("验证码错误");
			}
		}

		/// <summary>
		/// 生成指定长度的图片验证码
		/// </summary>
		/// <param name="length"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public static byte[] GenerateImageCode(int length, out string code)
		{
			code = GenerateCode(length);
			var width = code.Length * 22;

			var bitmap = new Bitmap(width + 6, 38);
			var graphics = Graphics.FromImage(bitmap);

			graphics.Clear(Color.White);

			for (var i = 0; i < 10; i++)
			{
				var x1 = Random.Next(bitmap.Width);
				var y1 = Random.Next(bitmap.Height);
				var x2 = Random.Next(bitmap.Width);
				var y2 = Random.Next(bitmap.Height);
				graphics.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
			}

			graphics.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

			for (var i = 0; i < code.Length; i++)
			{
				var x = new Matrix();
				x.Shear((float)Random.Next(0, 300) / 1000 - 0.25f, (float)Random.Next(0, 100) / 1000 - 0.05f);
				graphics.Transform = x;
				var str = code.Substring(i, 1);
				var brush = new LinearGradientBrush(new Rectangle(0, 0, bitmap.Width, bitmap.Height), Color.Blue, Color.DarkRed, 1.2f, true);
				var point = new Point(i * 21 + 1 + Random.Next(3), 1 + Random.Next(13));
				var font = new Font(Fonts[Random.Next(Fonts.Length - 1)], Random.Next(14, 18), FontStyle.Bold);
				graphics.DrawString(str, font, brush, point);
			}

			for (var i = 0; i < 30; i++)
			{
				var x = Random.Next(bitmap.Width);
				var y = Random.Next(bitmap.Height);
				bitmap.SetPixel(x, y, Color.FromArgb(Random.Next()));
			}

			var ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Png);

			graphics.Dispose();
			bitmap.Dispose();

			return ms.ToArray();
		}

		/// <summary>
		/// 生成指定长度的数字验证码
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string GenerateDigitalCode(int length)
		{
			return GenerateCode(BoundOfDigital, length);
		}

		/// <summary>
		/// 生成指定长度的验证码
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string GenerateCode(int length)
		{
			return GenerateCode(Bound, length);
		}

		/// <summary>
		/// 生成验证码
		/// </summary>
		/// <param name="bound"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		private static string GenerateCode(string bound, int length)
		{
			var code = string.Empty;
			for (var i = 0; i < length; i++)
			{
				code += bound[Random.Next(bound.Length - 1)];
			}
			return code;
		}
	}
}
