using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace MiniAOP.TestConsole
{
    public class ExceptionAdvice : IExceptionAdvice
    {
        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <param name="target"></param>
        public ExceptionAdviceResult Notice(Exception ex, MethodBase method, object[] args, object target)
        {
            ExceptionAdviceResult exResult = new ExceptionAdviceResult() { Ex = ex };
            //判断类或者方法使用应用了这个标签
            ExceptionAdviceAttribute methodAttr = method.GetCustomAttribute(typeof(ExceptionAdviceAttribute)) as ExceptionAdviceAttribute;
            ExceptionAdviceAttribute classAttr = target.GetType().GetCustomAttribute(typeof(ExceptionAdviceAttribute)) as ExceptionAdviceAttribute;
            //判断类或者方法使用应用了这个标签
            if ((methodAttr != null ||
                classAttr != null)
                && method.GetCustomAttribute(typeof(NOExceptionAdviceAttribute)) == null)
            {
                ExceptionAdviceAttribute attr = methodAttr ?? classAttr;
                exResult.Handled = attr.Handled;
                exResult.HandledReturnDefaultValue = attr.HandledReturnDefaultValue;
                String logStr = String.Format("method:{0},exMessage:{1},exStackTrace:{2}", method.Name, ex.Message, ex.StackTrace);
                Console.WriteLine(logStr);
            }
            return exResult;
        }
    }
}
