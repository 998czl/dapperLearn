using dapper_core.Application.Interface;
using dapper_model.dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.Linq;
using dapper_model.model;
using dapper_common;
using MySql.Data.MySqlClient;

namespace dapper_core.Application.Imp
{
	public class AdministratorService : IAdministratorService
	{
		/// <summary>
		/// 添加管理员
		/// </summary>
		/// <param name="administrator"></param>
		/// <returns></returns>
		public bool AddAdmin(Administrator administrator)
		{
			using (IDbConnection conn = DapperHelper.MySqlCon())
			{
				using (var trans = conn.BeginTransaction())
				{
					try
					{
						string sql = @"INSERT INTO `administrator`
                                    (
                                    UserName,
                                    Mobile,
                                    RealName,
                                    UserState,
                                    RoleId,
                                    UserPassword,
                                    CreateTime,
                                    ShowMobile                                              
                                    )
                                VALUES
                                    (
                                    @UserName,
                                    @Mobile,
                                    @RealName,
                                    @UserState,
                                    @RoleId,
                                    @UserPassword,
                                    @CreateTime,
                                    @ShowMobile                                              
                                    )";
						var rlt = conn.Execute(sql, administrator, trans) > 0;
						if (!rlt)
						{
							trans.Rollback();
							return rlt;
						}
						trans.Commit();
						return rlt;
					}
					catch (Exception ex)
					{
						trans.Rollback();
						LogHelper.Error("AddAdmin=>添加出错", ex);
						return false;
					}
				}
			}
		}

		/// <summary>
		/// 获取单个
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public AdminViewDto GetAdmin(int userId)
		{
			using (IDbConnection conn = DapperHelper.MySqlCon())
			{
				string sql = "select * from administrator where UserId=@userId";
				var result = conn.Query<Administrator>(sql, new { UserId = userId }).FirstOrDefault();
				if (result == null)
				{
					throw new Exception("参数错误!");
				}
				AdminViewDto adminViewDto = new AdminViewDto()
				{
					UserName = result.UserName,
					RealName = result.RealName,
					RoleName = result.RoleId == 0 ? "管理员" : "冒泡",
					UserState = result.UserState == 1 ? "在线" : "退出",
					Mobile = result.Mobile,
					CreateTime = result.CreateTime
				};
				return adminViewDto;

			}
		}

		/// <summary>
		/// 获取管理员集合
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public List<AdminViewDto> GetAdminList(AdminModel model)
		{
			using (IDbConnection conn = DapperHelper.MySqlCon())
			{
				int PageStart = (model.PageIndex - 1) * model.PageSize;
				DynamicParameters dy = new DynamicParameters();
				dy.Add("PageStart", PageStart);
				dy.Add("PageSize", model.PageSize);
				if (!string.IsNullOrEmpty(model.UserName))
				{
					dy.Add("UserName", model.UserName);
				}
				if (!string.IsNullOrEmpty(model.UserState))
				{
					dy.Add("UserState", Convert.ToInt32(model.UserState));
				}
				string sql = "select * from administrator {0} limit @PageStart ,@PageSize";
				StringBuilder where = new StringBuilder();
				if (!string.IsNullOrEmpty(model.UserName))
				{
					where.Append(" where UserName = @UserName ");
				}
				if (!string.IsNullOrEmpty(model.UserState))
				{
					if (where.Length > 0)
					{
						where.Append(" AND UserState = @UserState ");
					}
					else
					{
						where.Append(" where UserState = @UserState ");
					}
				}
				sql = string.Format(sql, where);
				var result = conn.Query<Administrator>(sql, dy).ToList();
				List<AdminViewDto> list = new List<AdminViewDto>();
				foreach (var item in result)
				{
					AdminViewDto adminViewDto = new AdminViewDto()
					{
						UserName = item.UserName,
						RealName = item.RealName,
						RoleName = item.RoleId == 0 ? "管理员" : "冒泡",
						UserState = item.UserState == 1 ? "在线" : "退出",
						Mobile = item.Mobile,
						CreateTime = item.CreateTime
					};
					list.Add(adminViewDto);
				}
				return list;
			}
		}

		/// <summary>
		/// 登陆
		/// </summary>
		/// <param name="name"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public Token<LoginInfo> Login(string name, string password)
		{
			using (IDbConnection conn = DapperHelper.MySqlCon())
			{
				string sql = "select * from administrator where UserName=@UserName AND UserPassword=@UserPassword";
				var result = conn.Query<Administrator>(sql, new { UserName = name, UserPassword = password }).FirstOrDefault();
				if (result == null)
				{
					throw new MTSException("用户名或密码错误");
				}
				var loginInfo = new LoginInfo()
				{
					UserId = result.UserId,
					UserName = result.UserName,
					Mobile = result.Mobile,
					RealName = result.RealName,
					RoleId = result.RoleId,
					CreateTime = result.CreateTime,
				};

				var token = new Token<LoginInfo>()
				{
					Id = loginInfo.UserId,
					Data = loginInfo
				};
				//var token = TokenHelper.Set(loginInfo, "User");
				return token;
			}
		}

		/// <summary>
		/// 修改管理员
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public bool UpdateAdmin(Administrator model)
		{
			using (IDbConnection conn = DapperHelper.MySqlCon())
			{
				using (var trans = conn.BeginTransaction())
				{
					try
					{
						string sql = @"UPDATE administrator
									SET UserName = @UserName,
									Mobile = @Mobile,
									RealName =@RealName,
									RoleId =@RoleId,
									UserPassword =@UserPassword
									WHERE
									UserId =@UserId";
						var rlt = conn.Execute(sql, model, trans) > 0;
						if (!rlt)
						{
							trans.Rollback();
							return rlt;
						}
						trans.Commit();
						return rlt;
					}
					catch (Exception ex)
					{
						trans.Rollback();
						LogHelper.Error("UpdateAdmin=>更新出错", ex);
						return false;
					}
				}
			}
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public bool DeleteAdmin(int userId)
		{
			using (IDbConnection conn = DapperHelper.MySqlCon())
			{
				using (var trans = conn.BeginTransaction())
				{
					try
					{
						string sql = "delete from administrator where UserId=@UserId";
						var rlt = conn.Execute(sql, new { UserId = userId }, trans) > 0;
						if (!rlt)
						{
							trans.Rollback();
							return rlt;
						}
						trans.Commit();
						return rlt;
					}
					catch (Exception ex)
					{
						trans.Rollback();
						LogHelper.Error("DeleteAdmin=>更新出错", ex);
						return false;
					}
				}
			}
		}


	}
}
