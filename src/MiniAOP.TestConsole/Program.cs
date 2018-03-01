using System;
using System.Collections.Generic;
using System.Reflection;

using MiniAOP;

namespace MiniAOP.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
            //TestReflection();
            Console.Read();
        }

        static void TestReflection()
        {
            User u = new User() { Password = "jjj", UserName = "kkk" };
            Type uType = u.GetType();
            MethodInfo method = uType.GetMethod("GetMyPassword");
            Object[] parameters = new Object[2];
            method.Invoke(u, parameters);
        }

        static void Test2()
        {
            User u = new User() { Password = "jjj", UserName = "kkk" };
            u.Password = "fdsfda";
            List<IAdvice> advice = new List<IAdvice>() {
                 new BeforeAdvice_Log(),
                 new AfterAdvice_Log(),
                  new BeforeAdvice_Authorize(){  TokenParamIndex = 0}
            };
            IMehodInterceptor methodInterceptor = new MehodInterceptor_Audit();
            ExceptionAdvice exAdvice = new ExceptionAdvice();
            User userProxy = (User)ProxyFactory.Current.CreateProxy(u, advice, methodInterceptor, exAdvice);
            userProxy.UserName = "kkkk";
            String token = userProxy.Login("lijian", "1234");
            Console.WriteLine("user:{0},password:{1},loginToken:{2}", "lijian", "1234", token);
            token = userProxy.Login("lj", "1234");
            Console.WriteLine("user:{0},password:{1},loginToken:{2}", "lj", "1234", token);
            String password;
            userProxy.GetMyPassword(token, "jjjjjjjjj", out password);
            String un;
            String pw;
            String unref = "";
            String pwref = "";
            userProxy.GetUserDetails(token, "lj", out un, "", out pw, ref unref, ref pwref);
            Console.WriteLine("password:{0}", password);
            try
            {
                userProxy.GetCount(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("client got exception!!!!!");
            }
        }

        static void Test1()
        {
            //Student stu = ProxyBuilder.Current.CreateProxy<Student>();
            //stu.Name = "lijian";
            //stu.ID = "123456";
            //Console.WriteLine(stu.Name);
            //Console.WriteLine(stu.GetStudentName("09876"));
            //Student newStu = stu.GetStudent("zhq", "567890");
        }
    }
}
