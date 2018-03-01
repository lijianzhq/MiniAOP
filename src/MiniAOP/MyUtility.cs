using System;
using System.Reflection;

namespace MiniAOP
{
    public static class MyUtility
    {
        /// <summary>
        /// 统计方法输出参数个数
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        //public static Int32 GetMethodOutParameterCount(MethodInfo method)
        //{
        //    Int32 count = 0;
        //    if (method != null)
        //    {
        //        ParameterInfo[] parameters = method.GetParameters();
        //        if (parameters != null && parameters.Length > 0)
        //        {
        //            foreach (ParameterInfo p in parameters)
        //            {
        //                if (p.IsOut) count++;
        //            }
        //        }
        //    }
        //    return count;
        //}

        /// <summary>
        /// 统计方法输出参数个数
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static Int32 GetMethodOutParameterCount(MethodBase method)
        {
            Int32 count = 0;
            if (method != null)
            {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters != null && parameters.Length > 0)
                {
                    foreach (ParameterInfo p in parameters)
                    {
                        if (p.IsOut) count++;
                    }
                }
            }
            return count;
        }
    }
}
