using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using dapper_core;
using dapper_core.Application.Interface;
using dapper_model.dto;
using dapper_model.model;
using dapper_webapi.Extensions;
using dapper_webapi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace dapper_webapi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AdminController : ApiController
	{
		/// 登陆
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost("/api/Admin/Login")]
		public ReturnResult<Token<LoginInfo>> Login([FromBody] LoginModel model)
		{
			var _adminservice = Container.Instance.Resolve<IAdministratorService>();
			var token = _adminservice.Login(model.Username, model.Password);
			return Json(token);
		}
	}
}
