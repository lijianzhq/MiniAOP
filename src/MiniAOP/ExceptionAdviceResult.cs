using System;
using System.Runtime.Remoting.Messaging;

namespace MiniAOP
{
    public class ExceptionAdviceResult
    {
        private Boolean _handled = false;
        private Object _handledReturnDefaultValue = null;

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

        /// <summary>
        /// 当breakmethod为true时，必须要提供这个异常对象，只有抛出异常对象才会终止方法执行
        /// </summary>
        public Exception Ex
        {
            get; set;
        }
    }
}
