using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniAOP.TestConsole
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BeforeAdvice_LogAttribute : Attribute
    { }

    /// <summary>
    /// 指示方法不要记录调用前日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NOBeforeAdvice_LogAttribute : Attribute
    { }
}
