using System;
using System.Runtime.Remoting.Messaging;

namespace MiniAOP
{
    public class BeforeAdviceResult
    {
        private Boolean _breakMethod = false;

        /// <summary>
        /// 是否终止内部继续执行
        /// true：终止；false：不终止
        /// </summary>
        public Boolean BreakMethod
        {
            get { return _breakMethod; }
            set { _breakMethod = value; }
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
