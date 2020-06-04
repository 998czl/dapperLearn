using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using dapper_Api_New.Extensions;
using dapper_Api_New.Model;
using dapper_common;
using dapper_common.Utile;
using dapper_core;
using dapper_core.Application.Interface;
using dapper_model.dto;
using dapper_model.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dapper_Api_New.Controllers
{
	/// <summary>
	/// 管理员
	/// </summary>
	[Produces(HttpContentType.Json)]
	public class AdminController : ApiController
	{
		/// <summary>
		/// 登陆
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[ApiAuthorize(VerifyCode = true)]
		[HttpPost("/api/Admin/Login")]
		public ReturnResult<Token<LoginInfo>> Login([FromBody] LoginModel model)
		{
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var token = _adminservice.Login(model.Username, model.Password);
			return Json(token);
		}

		/// <summary>
		/// 获取列表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[ApiAuthorize(VerifyToken = true)]
		[HttpPost("/api/Admin/GetAdminList")]
		public ReturnResult<List<AdminViewDto>> GetAdminList([FromBody] AdminModel model)
		{
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var list = _adminservice.GetAdminList(model);
			return Json(list);
		}

		/// <summary>
		/// 添加
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[ApiAuthorize(VerifyToken = true)]
		[HttpPost("/api/Admin/AddAdmin")]
		public ReturnResult<string> AddAdmin([FromBody] Administrator model)
		{
			model.CreateTime = DateTime.Now;
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var result = _adminservice.AddAdmin(model);
			if (result)
			{
				return Ok();
			}
			return Ok("添加失败");
		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[ApiAuthorize(VerifyToken = true)]
		[HttpPost("/api/Admin/UpdateAdmin")]
		public ReturnResult<string> UpdateAdmin([FromBody] Administrator model)
		{
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var result = _adminservice.UpdateAdmin(model);
			if (result)
			{
				return Ok();
			}
			return Ok("修改失败!");
		}

		/// <summary>
		/// 获取单个
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[ApiAuthorize(VerifyToken = true)]
		[HttpGet("/api/Admin/GetAdmin")]
		public ReturnResult<AdminViewDto> GetAdmin(int userId)
		{
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var admin = _adminservice.GetAdmin(userId);
			return Json(admin);
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		[ApiAuthorize(VerifyToken = true)]
		[HttpGet("/api/Admin/DeleteAdmin")]
		public ReturnResult<string> DeleteAdmin(int userId)
		{
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var result = _adminservice.DeleteAdmin(userId);
			if (result)
			{
				return Ok();
			}
			return Ok("删除失败");
		}		
	}
}
