using System;
using System.Collections.Generic;

namespace MiniAOP
{
    public class ProxyFactory : IProxyFactory
    {
        /// <summary>
        /// 可以通过AOP注入
        /// </summary>
        private static ProxyFactory _current = new ProxyFactory();
        private ProxyFactory()
        { }

        public static ProxyFactory Current
        {
            get { return _current; }
        }

        /// <summary>
        /// 创建代理对象
        /// </summary>
        /// <param name="targetInstance">需要创建代理的对象实例</param>
        /// <param name="adviceList">需要注入的通知（前置、后置、异常三类通知）</param>
        /// <param name="methodInterceptor">需要注入的方法（环绕注入）</param>
        /// <param name="exceptionAdvice">异常注入</param>
        /// <returns></returns>
        public virtual Object CreateProxy(Object targetInstance, IEnumerable<IAdvice> adviceList, IMehodInterceptor methodInterceptor, IExceptionAdvice exceptionAdvice)
        {
            AOPProxy proxy = new AOPProxy(targetInstance);
            proxy.AddAdvice(adviceList);
            proxy.MehodInterceptor = methodInterceptor;
            proxy.ExceptionAdvice = exceptionAdvice;
            return proxy.GetTransparentProxy();
        }
    }
}
