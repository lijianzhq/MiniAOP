using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniAOP.TestConsole
{
    public class BeforeAdvice_Log : IBeforeAdvice
    {
        /// <summary>
        /// 在调用方法前，把输入参数打印出来
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        BeforeAdviceResult IBeforeAdvice.Notice(MethodBase method, object[] args, object target)
        {
            //判断类或者方法使用应用了这个标签
            if ((method.GetCustomAttribute(typeof(BeforeAdvice_LogAttribute)) != null ||
                target.GetType().GetCustomAttribute(typeof(BeforeAdvice_LogAttribute)) != null)
                && method.GetCustomAttribute(typeof(NOBeforeAdvice_LogAttribute)) == null)
            {
                ParameterInfo[] methodParams = method.GetParameters();
                IEnumerable<String> methodParamNames = methodParams.Select((p, i) => { return p.Name; });
                Dictionary<String, Object> methodParamAndValues = new Dictionary<string, object>();
                for (var i = 0; i < methodParamNames.Count(); i++)
                {
                    methodParamAndValues.Add(methodParamNames.ElementAt(i), args[i]);
                }
                String logStr = String.Format("method:{0},args:{1}", method.Name, MyJson.Serialize(methodParamAndValues));
                Console.WriteLine(logStr);
            }
            return new BeforeAdviceResult() { BreakMethod = false };
        }
    }
}
