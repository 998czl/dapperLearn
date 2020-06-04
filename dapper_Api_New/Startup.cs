using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dapper_core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace dapper_Api_New
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			//设置版本
			services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

			//配置httpcontext
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			//配置session
			services.AddSession();

			Container.RegisterAll();

			//配置允许跨域
			services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins",
						builder => //builder.AllowAnyOrigin()
						builder.WithOrigins("localhost:5000").AllowAnyHeader().AllowAnyMethod().AllowCredentials());
			});

			//注册Swagger生成器，定义一个和多个Swagger 文档
			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "API 文档",
					Version = "v1",
					Description = "项目所有接口说明"
				});

				//为 Swagger JSON and UI设置xml文档注释路径
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});


		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

			}
			//启用Https
			app.UseHttpsRedirection();
			//启用Session
			app.UseSession();
			//启用允许跨域
			app.UseCors("AllowAllOrigins");

			//使中间件服务生成Swagger作为JSON端点
			app.UseSwagger();
			////启用中间件以提供用户界面（HTML、js、CSS等），特别是指定JSON端点
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
			});

			//启用Mvc
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "api/{controller}/{action}/{id?}",
					defaults: new { controller = "home", action = "index" });
			});

			//注册编码
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}
	}
}
