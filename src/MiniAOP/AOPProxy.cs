using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace MiniAOP
{
    /// <summary>
    /// AOP代理对象
    /// </summary>
    public class AOPProxy : RealProxy
    {
        private Object _proxyTarget;
        private IList<IBeforeAdvice> beforeAdviceList = new List<IBeforeAdvice>();
        private IList<IAfterAdvice> afterAdviceList = new List<IAfterAdvice>();
        private IExceptionAdvice exceptionAdvice = null;
        private IMehodInterceptor mehodInterceptor = null;

        public IMehodInterceptor MehodInterceptor
        {
            get { return mehodInterceptor; }
            set { mehodInterceptor = value; }
        }
        public IExceptionAdvice ExceptionAdvice
        {
            get { return exceptionAdvice; }
            set { exceptionAdvice = value; }
        }

        public AOPProxy(Object instance) : base(instance.GetType())
        {
            this._proxyTarget = instance;
        }

        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage methodCallMsg = msg as IMethodCallMessage;
            try
            {
                //对于属性的一些get/set方法不要进行拦截
                if (methodCallMsg.MethodBase.IsSpecialName)
                {
                    Boolean breakMethod = true;
                    return InvokeMethod(methodCallMsg.MethodBase, methodCallMsg, out breakMethod);
                }
                return InvokeMethodWithAdvice(methodCallMsg);
            }
            catch (Exception ex)
            {
                return Exception(ex.InnerException ?? ex, methodCallMsg.MethodBase, methodCallMsg);
                //return new ReturnMessage(ex.InnerException ?? ex, methodCallMsg);
            }
        }

        /// <summary>
        /// 直接执行方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="methodCallMsg"></param>
        /// <param name="breakMethod"></param>
        /// <returns></returns>
        public virtual ReturnMessage InvokeMethod(MethodBase method, IMethodCallMessage methodCallMsg, out Boolean breakMethod)
        {
            try
            {
                breakMethod = false;
                Object[] methodArgs = GetMethodArgs(methodCallMsg, out int outParamCount);
                Object returnVal = method.Invoke(this._proxyTarget, methodArgs);
                return new ReturnMessage(returnVal, methodArgs, outParamCount, methodCallMsg.LogicalCallContext, methodCallMsg);
            }
            catch (Exception ex)
            {
                breakMethod = true;
                return Exception(ex.InnerException ?? ex, method, methodCallMsg);
                //捕获到异常后，直接返回方法异常，不再处理后续通知
                //return new ReturnMessage(ex.InnerException ?? ex, methodCallMsg);
            }
        }

        public virtual ReturnMessage InvokeMethodWithAdvice(IMethodCallMessage methodCallMsg)
        {
            //MethodInfo method = this._proxyTarget.GetType().GetMethod(methodCallMsg.MethodName);
            MethodBase method = methodCallMsg.MethodBase;
            //方法前注入处理
            ReturnMessage breakReturnMsg = Before(method, methodCallMsg);
            if (breakReturnMsg != null) return breakReturnMsg;
            //环绕注入
            Object returnVal = null;
            Boolean breakMethod = true;
            ReturnMessage methodReturnMsg = null;
            if (this.mehodInterceptor != null)
            {
                methodReturnMsg = InvokeMethodInterceptor(this.mehodInterceptor, method, methodCallMsg, out breakMethod);
                if (breakMethod) return methodReturnMsg;
            }
            else
            {
                methodReturnMsg = InvokeMethod(method, methodCallMsg, out breakMethod);
                if (breakMethod) return methodReturnMsg;
            }
            //后置通知
            breakReturnMsg = After(returnVal, method, methodReturnMsg.OutArgs, methodCallMsg);
            if (breakReturnMsg != null) return breakReturnMsg;
            //返回方法执行的结果
            return methodReturnMsg;
        }

        public virtual void AddAdvice(IAdvice advice)
        {
            IBeforeAdvice beforeAd = advice as IBeforeAdvice;
            if (beforeAd != null)
            {
                beforeAdviceList.Add(beforeAd);
                return;
            }
            IAfterAdvice afterAd = advice as IAfterAdvice;
            if (afterAd != null)
            {
                afterAdviceList.Add(afterAd);
                return;
            }
        }

        public virtual void AddAdvice(IEnumerable<IAdvice> advice)
        {
            foreach (IAdvice item in advice)
                this.AddAdvice(item);
        }

        /// <summary>
        /// 调用方法注入回调
        /// </summary>
        /// <param name="mehodInterceptor"></param>
        /// <param name="method"></param>
        /// <param name="methodCallMsg"></param>
        /// <param name="breakMethod">是否终止方法</param>
        /// <returns></returns>
        public virtual ReturnMessage InvokeMethodInterceptor(IMehodInterceptor mehodInterceptor, MethodBase method, IMethodCallMessage methodCallMsg, out Boolean breakMethod)
        {
            ReturnMessage returnMsg = null;
            MethodInterceptorResult reVal = null;
            breakMethod = true;
            try
            {
                Object[] methodArgs = GetMethodArgs(methodCallMsg, out int outParamCount);
                reVal = mehodInterceptor.Notice(method, methodArgs, this._proxyTarget);
                if (reVal != null && reVal.BreakMethod)
                {
                    Exception ex = reVal.Ex ?? new Exception(String.Format("{0} break method!", mehodInterceptor.GetType().ToString()));
                    returnMsg = new ReturnMessage(ex, methodCallMsg);
                }
                else
                {
                    returnMsg = new ReturnMessage(reVal.MethodReturnVal, methodArgs, outParamCount, methodCallMsg.LogicalCallContext, methodCallMsg);
                    breakMethod = false;
                }
            }
            catch (Exception ex)
            {
                //returnMsg = new ReturnMessage(ex.InnerException ?? ex, methodCallMsg);
                return Exception(ex.InnerException ?? ex, method, methodCallMsg);
            }
            return returnMsg;
        }

        /// <summary>
        /// 前置通知
        /// </summary>
        /// <param name="method"></param>
        /// <param name="methodCallMsg"></param>
        /// <returns></returns>
        public virtual ReturnMessage Before(MethodBase method, IMethodCallMessage methodCallMsg)
        {
            ReturnMessage returnMsg = null;
            BeforeAdviceResult reVal = null;
            try
            {
                if (beforeAdviceList == null || beforeAdviceList.Count == 0) return returnMsg;
                foreach (IBeforeAdvice ad in beforeAdviceList)
                {
                    reVal = ad.Notice(method, methodCallMsg.Args, this._proxyTarget);
                    //如果一旦前置通知返回false，则直接退出所有执行，直接返回了
                    if (reVal != null && reVal.BreakMethod)
                    {
                        Exception ex = reVal.Ex ?? new Exception(String.Format("{0} break method!", ad.GetType().ToString()));
                        returnMsg = new ReturnMessage(ex, methodCallMsg);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //returnMsg = new ReturnMessage(ex.InnerException ?? ex, methodCallMsg);
                return Exception(ex.InnerException ?? ex, method, methodCallMsg);
            }
            return returnMsg;
        }

        /// <summary>
        /// 后置通知
        /// </summary>
        /// <param name="returnValue"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <param name="methodCallMsg"></param>
        /// <returns></returns>
        public virtual ReturnMessage After(object returnValue, MethodBase method, object[] args, IMethodCallMessage methodCallMsg)
        {
            ReturnMessage returnMsg = null;
            try
            {
                if (afterAdviceList != null && afterAdviceList.Count > 0)
                {
                    foreach (IAfterAdvice ad in afterAdviceList)
                    {
                        ad.Notice(returnValue, method, args, this._proxyTarget);
                    }
                }
            }
            catch (Exception ex)
            {
                //returnMsg = new ReturnMessage(ex.InnerException ?? ex, methodCallMsg);
                return Exception(ex.InnerException ?? ex, method, methodCallMsg);
            }
            return returnMsg;
        }

        /// <summary>
        /// 异常切入处理
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        /// <param name="methodCallMsg"></param>
        public virtual ReturnMessage Exception(Exception ex, MethodBase method, IMethodCallMessage methodCallMsg)
        {
            if (ExceptionAdvice != null)
            {
                ExceptionAdviceResult exResult = ExceptionAdvice.Notice(ex, method, methodCallMsg.Args, this._proxyTarget);
                if (exResult.Handled)
                {
                    return new ReturnMessage(exResult.HandledReturnDefaultValue, new Object[0], 0, methodCallMsg.LogicalCallContext, methodCallMsg);
                }
                else
                {
                    return new ReturnMessage(exResult.Ex.InnerException ?? exResult.Ex, methodCallMsg);
                }
            }
            return new ReturnMessage(ex.InnerException ?? ex, methodCallMsg);
        }

        /// <summary>
        /// 获取传入的方法参数
        /// 如果目标方法有out参数，必须要重新创建一个参数数组，因为原来的args属性无法写入（可能属于底层的bug）
        /// </summary>
        /// <param name="methodCallMsg"></param>
        /// <param name="outParamCount">目标方法的输出参数个数</param>
        /// <returns></returns>
        protected virtual Object[] GetMethodArgs(IMethodCallMessage methodCallMsg, out Int32 outParamCount)
        {
            outParamCount = MyUtility.GetMethodOutParameterCount(methodCallMsg.MethodBase);
            Object[] methodArgs = null;
            if (outParamCount > 0)
            {
                methodArgs = new Object[methodCallMsg.Args.Length];
                Array.Copy(methodCallMsg.Args, methodArgs, methodCallMsg.Args.Length);
            }
            return methodArgs ?? methodCallMsg.Args;
        }

        /// <summary>
        /// 获取方法是输出参数值
        /// </summary>
        /// <param name="inputParams">方法的输入参数</param>
        /// <param name="method">方法对象</param>
        /// <param name="outParamCount">输出（方法的输出参数个数）</param>
        /// <returns></returns>
        //protected Object[] GetMethodOutParameterValues(Object[] inputParams, MethodBase method, out Int32 outParamCount)
        //{
        //    outParamCount = MyUtility.GetMethodOutParameterCount(method);
        //    Object[] outParams = new Object[0];
        //    if (outParamCount > 0)
        //    {
        //        outParams = new Object[outParamCount];
        //        inputParams.CopyTo(outParams, inputParams.Length - outParamCount);
        //    }
        //    return outParams;
        //}
    }
}
