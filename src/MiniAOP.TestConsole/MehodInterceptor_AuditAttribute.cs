using System;
using System.Reflection;
using System.Diagnostics;

namespace MiniAOP.TestConsole
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MehodInterceptor_AuditAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NOMehodInterceptor_AuditAttribute : Attribute
    {
    }
}
