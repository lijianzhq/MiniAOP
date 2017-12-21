using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace MiniAOP.TestConsole
{
    public class BeforeAdvice_Authorize : IBeforeAdvice
    {
        private Int32 _tokenParamIndex = 0;

        /// <summary>
        /// token参数在方法参数中的顺序，从0开始
        /// </summary>
        public Int32 TokenParamIndex
        {
            get
            {
                return _tokenParamIndex;
            }
            set
            {
                if (value < 0) throw new ArgumentException("TokenParamIndex");
                _tokenParamIndex = value;
            }
        }

        public BeforeAdviceResult Notice(MethodBase method, object[] args, object target)
        {
            //判断类或者方法使用应用了这个标签
            BeforeAdvice_AuthorizeAttribute methodAttr = method.GetCustomAttribute(typeof(BeforeAdvice_AuthorizeAttribute)) as BeforeAdvice_AuthorizeAttribute;
            BeforeAdvice_AuthorizeAttribute classAttr = target.GetType().GetCustomAttribute(typeof(BeforeAdvice_AuthorizeAttribute)) as BeforeAdvice_AuthorizeAttribute;
            if ((methodAttr != null || classAttr != null)
                && method.GetCustomAttribute(typeof(NOBeforeAdvice_AuthorizeAttribute)) == null)
            {
                try
                {
                    Int32 tokenParamIndex = methodAttr != null ? methodAttr.TokenParamIndex : classAttr.TokenParamIndex;
                    if (tokenParamIndex >= args.Length)
                        throw new ArgumentException("TokenParamIndex");
                    String token = Convert.ToString(args[tokenParamIndex]);
                    if (!User.IsUserLogin(token))
                        throw new ArgumentException("token");
                }
                catch (Exception ex)
                {
                    return new BeforeAdviceResult() { BreakMethod = true, Ex = ex };
                }
            }
            return new BeforeAdviceResult() { BreakMethod = false };
        }
    }
}
