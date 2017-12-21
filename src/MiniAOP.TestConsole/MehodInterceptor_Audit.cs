using System;
using System.Reflection;
using System.Diagnostics;

namespace MiniAOP.TestConsole
{
    public class MehodInterceptor_Audit : IMehodInterceptor
    {
        public MethodInterceptorResult Notice(MethodBase method, object[] args, object target)
        {
            Object reVal = null;
            if ((method.GetCustomAttribute(typeof(MehodInterceptor_AuditAttribute)) != null ||
              target.GetType().GetCustomAttribute(typeof(MehodInterceptor_AuditAttribute)) != null)
              && method.GetCustomAttribute(typeof(NOMehodInterceptor_AuditAttribute)) == null)
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                reVal = method.Invoke(target, args);
                watch.Stop();
                Console.WriteLine("method:{0},take times:{1} milliseconds", method.Name, watch.Elapsed.TotalMilliseconds);
                return new MethodInterceptorResult() { BreakMethod = false, MethodReturnVal = reVal };
            }
            else
            {
                reVal = method.Invoke(target, args);
            }
            return new MethodInterceptorResult() { BreakMethod = false, MethodReturnVal = reVal };
        }
    }
}
