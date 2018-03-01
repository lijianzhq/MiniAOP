using System;
using System.Reflection;

namespace MiniAOP
{
    /// <summary>
    /// 方法后通知
    /// </summary>
    public interface IAfterAdvice : IAdvice
    {
        /// <summary>
        /// 方法执行后置通知
        /// </summary>
        /// <param name="returnValue">方法返回值</param>
        /// <param name="method"></param>
        /// <param name="outAndrefArgs">传入给方法的out和ref参数</param>
        /// <param name="args">传入给方法的参数（包含out和ref的参数）</param>
        /// <param name="target"></param>
        void Notice(object returnValue, MethodBase method, object[] outAndrefArgs, object[] args, object target);
    }
}
