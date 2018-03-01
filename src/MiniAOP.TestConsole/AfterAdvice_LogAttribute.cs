using System;

namespace MiniAOP.TestConsole
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AfterAdvice_LogAttribute : Attribute
    {
    }

    /// <summary>
    /// 指示方法不要记录调用前日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NOAfterAdvice_LogAttribute : Attribute
    { }
}
