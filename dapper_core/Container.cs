using Autofac;
using Autofac.Extras.DynamicProxy;
using dapper_core.Application.Imp;
using dapper_core.Application.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_core
{
	public class Container
	{
		private static IContainer container;

		public static void RegisterAll()
		{
			var builder = new ContainerBuilder();

			#region Interceptor

			builder.Register(o => new ExeceptionInterceptor());

			#endregion

			#region Service
			builder.RegisterType<AdministratorService>()
			  .EnableInterfaceInterceptors()
			  .InterceptedBy(typeof(ExeceptionInterceptor))
			  .As<IAdministratorService>();
			#endregion

			container = builder.Build();
		}

		public static IContainer Instance
		{
			get
			{
				return container;
			}
		}
	}
}
