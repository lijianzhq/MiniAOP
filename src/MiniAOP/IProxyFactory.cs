using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAOP
{
    public interface IProxyFactory
    {
        Object CreateProxy(Object instance, IEnumerable<IAdvice> adviceList, IMehodInterceptor methodInterceptor, IExceptionAdvice exceptionAdvice);
    }
}
