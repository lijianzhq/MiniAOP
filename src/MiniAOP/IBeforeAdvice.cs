using System;
using System.Reflection;

namespace MiniAOP
{
    /// <summary>
    /// 方法前通知
    /// </summary>
    public interface IBeforeAdvice : IAdvice
    {
        /// <summary>
        /// 方法执行前回调通知，如果切面通知执行返回false，则会终止执行操作（目标方法得不到执行）
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        BeforeAdviceResult Notice(MethodBase method, object[] args, object target);
    }
}
