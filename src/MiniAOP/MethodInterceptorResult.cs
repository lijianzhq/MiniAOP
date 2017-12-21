using System;
using System.Runtime.Remoting.Messaging;

namespace MiniAOP
{
    /// <summary>
    /// 注入方法内部返回的结果对象
    /// </summary>
    public class MethodInterceptorResult
    {
        private Boolean _breakMethod = false;

        /// <summary>
        /// 是否终止内部继续执行
        /// true：终止；false：不终止
        /// </summary>
        public Boolean BreakMethod
        {
            get { return _breakMethod; }
            set { _breakMethod = value; }
        }

        /// <summary>
        /// 必须提供这个对象，这个对象就是目标对象方法执行后返回的值
        /// </summary>
        public Object MethodReturnVal
        {
            get; set;
        }

        /// <summary>
        /// 必须返回这个对象，这个对象就是调用目标方法传入的参数数组（如果方法有out参数，会通过此参数返回，所有用户必须要返回这个参数给底层）
        /// </summary>
        //public Object[] MethodArgs
        //{
        //    get; set;
        //}

        /// <summary>
        /// 当breakmethod为true时，必须要提供这个异常对象，只有抛出异常对象才会终止方法执行
        /// </summary>
        public Exception Ex
        {
            get; set;
        }
    }
}
