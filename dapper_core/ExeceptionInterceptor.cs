using Castle.DynamicProxy;
using dapper_common;
using System;
using System.Collections.Generic;
using System.Text;

namespace dapper_core
{
	public class ExeceptionInterceptor : IInterceptor
	{
		/// <summary>
		/// 日志实例
		/// </summary>	
		public void Intercept(IInvocation invocation)
		{
			try
			{
				invocation.Proceed();
			}
			catch (MTSException e)
			{
				//当遇到该类型异常不做处理，抛出给调用层去处理
				throw e;
			}
			catch (Exception e)
			{
				LogHelper.Error(e.Message, e);
			}
		}
	}
}
