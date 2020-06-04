using dapper_model.dto;
using dapper_model.model;
using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_core.Application.Interface
{
	public interface IAdministratorService
	{
		/// <summary>
		/// 登陆
		/// </summary>
		/// <param name="name"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		Token<LoginInfo> Login(string name, string password);

		/// <summary>
		/// 获取管理员集合
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		List<AdminViewDto> GetAdminList(AdminModel model);

		/// <summary>
		/// 添加管理员
		/// </summary>
		/// <param name="administrator"></param>
		/// <returns></returns>
		bool AddAdmin(Administrator administrator);

		/// <summary>
		/// 修改管理员
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		bool UpdateAdmin(Administrator model);

		/// <summary>
		/// 获取单个
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		AdminViewDto GetAdmin(int userId);

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		bool DeleteAdmin(int userId);
	}
}
