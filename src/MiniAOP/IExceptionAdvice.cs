using System;
using System.Reflection;

namespace MiniAOP
{
    /// <summary>
    /// 异常通知
    /// </summary>
    public interface IExceptionAdvice
    {
        ExceptionAdviceResult Notice(Exception ex, MethodBase method, object[] args, object target);
    }
}
