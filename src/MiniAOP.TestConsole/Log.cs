using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAOP.TestConsole
{
    class Log
    {
        public static void Debug(String msg)
        {
            Console.WriteLine(msg);
        }
    }
}
