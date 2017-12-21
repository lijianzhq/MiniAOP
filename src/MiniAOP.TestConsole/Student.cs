using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAOP.TestConsole
{
    class Student : MarshalByRefObject
    {
        public String Name
        {
            get; set;
        }

        public String ID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public String GetStudentName(String id)
        {
            return String.Format("{0};{1}", Name, id);
        }

        public Student GetStudent(String name, String id)
        {
            return new Student() { Name = name, ID = id };
        }
    }
}
