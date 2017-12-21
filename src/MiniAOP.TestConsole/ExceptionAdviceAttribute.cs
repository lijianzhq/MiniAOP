using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniAOP.TestConsole
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ExceptionAdviceAttribute : Attribute
    {
        private Boolean _handled = false;
        private Object _handledReturnDefaultValue = false;

        /// <summary>
        /// 标志异常是否已经被处理，如果异常已经被处理，则后面不会再抛出异常了，慎重配置
        /// </summary>
        public Boolean Handled
        {
            get { return this._handled; }
            set { this._handled = value; }
        }

        /// <summary>
        /// 异常处理之后，返回的默认值
        /// </summary>
        public Object HandledReturnDefaultValue
        {
            get { return this._handledReturnDefaultValue; }
            set { this._handledReturnDefaultValue = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class NOExceptionAdviceAttribute : Attribute
    {

    }
}
