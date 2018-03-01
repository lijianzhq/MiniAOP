using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using MiniAOP;

namespace MiniAOP.TestConsole
{
    class AfterAdvice_Log : IAfterAdvice
    {
        public void Notice(object returnValue, System.Reflection.MethodBase method, object[] outAndrefArgs, object[] args, object target)
        {
            Type targetType = target.GetType();
            //判断类或者方法使用应用了这个标签
            if ((method.GetCustomAttribute(typeof(AfterAdvice_LogAttribute)) != null ||
                targetType.GetCustomAttribute(typeof(AfterAdvice_LogAttribute)) != null)
                && method.GetCustomAttribute(typeof(NOAfterAdvice_LogAttribute)) == null)
            {
                ParameterInfo[] methodParams = method.GetParameters();
                Dictionary<String, Object> methodParamAndValues = GetOutOrRefParameterNameAndVal(methodParams, outAndrefArgs);
                String logStr = String.Format("return,type:{0},method:{1},returnVal:{2},outParamValue:{3}", targetType.FullName, method.Name, MyJson.Serialize(returnValue), MyJson.Serialize(methodParamAndValues));
                //Log.Root.LogDebug(logStr);
                Console.WriteLine(logStr);
            }
        }

        /// <summary>
        /// 根据方法的outandref参数值数组以及方法的所有参数数组，获取out和ref参数值对应的参数名，并返回dic结构体
        /// </summary>
        /// <param name="methodParams">方法的所有参数数组</param>
        /// <param name="outAndrefArgsValues">方法的out和ref参数（值）数组</param>
        /// <returns></returns>
        private Dictionary<String, Object> GetOutOrRefParameterNameAndVal(ParameterInfo[] methodParams, object[] outAndrefArgsValues)
        {
            Dictionary<String, Object> methodParamAndValues = new Dictionary<string, object>();
            Int32 outParamCount = outAndrefArgsValues == null ? 0 : outAndrefArgsValues.Length;
            for (Int32 i = 0, k = 0; i < outParamCount; i++)
            {
                for (; k < methodParams.Length; k++)
                {
                    //是引用类型，则默认就是当前的out或ref参数的名字（是按顺序处理的）
                    if (methodParams[k].ParameterType.IsByRef)
                    {
                        methodParamAndValues.Add(methodParams[k].Name, outAndrefArgsValues[i]);
                        k++;
                        break;//找到当前的out或ref参数值对应的parameter对象之后，一定要先退出循环，以备下一次寻找
                    }
                }
            }
            return methodParamAndValues;
        }
    }
}
