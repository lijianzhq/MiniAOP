using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace MiniAOP.TestConsole
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class BeforeAdvice_AuthorizeAttribute : Attribute
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
    }

    /// <summary>
    /// 指示方法不要验证授权
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NOBeforeAdvice_AuthorizeAttribute : Attribute
    { }
}
