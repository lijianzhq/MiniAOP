using System;
using System.Reflection;

namespace MiniAOP
{
    /// <summary>
    /// 方法注入式回调
    /// 一个方法代理只能设置一个
    /// 并且用户在自己的方法内部必须invoce method，否则原目标对象的方法得不到执行
    /// </summary>
    public interface IMehodInterceptor
    {
        MethodInterceptorResult Notice(MethodBase method, object[] args, object target);
    }
}
