using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniAOP.TestConsole
{
    [BeforeAdvice_Log]
    [AfterAdvice_Log]
    [BeforeAdvice_Authorize]
    [MehodInterceptor_Audit]
    [ExceptionAdvice]
    class User : MarshalByRefObject
    {
        private static List<User> allUsers = new List<User>() {
            new User { Password = "1234", UserName = "lj" },
            new User { Password ="45678",UserName ="zhq" } };

        public static List<String> loginUserToken = new List<string>();

        public String UserName
        {
            get; set;
        }
        public String Password
        {
            get; set;
        }

        [NOBeforeAdvice_Authorize]
        public String Login(String userName, String password)
        {
            foreach (User u in allUsers)
            {
                if (String.Compare(u.UserName, userName, false) == 0 &&
                    String.Compare(u.Password, password, false) == 0)
                {
                    String token = Guid.NewGuid().ToString("N");
                    loginUserToken.Add(token);
                    return token;
                }
            }
            return String.Empty;
        }

        public static Boolean IsUserLogin(String token)
        {
            return loginUserToken.Contains(token);
        }

        public void GetUserDetails(String token, String userName, out String un, String pp, out String pw, ref String unref, ref String pwref)
        {
            un = "";
            pw = "";
            foreach (User u in allUsers)
            {
                if (String.Equals(u.UserName, userName))
                {
                    un = "un" + u.UserName;
                    unref = "unref" + u.UserName;
                    pw = "pw" + u.Password;
                    pwref = "pwref" + u.Password;
                }
            }
        }

        public void GetMyPassword(String token, String passwordIn, out String passwordOut)
        {
            this.Password = passwordIn;
            passwordOut = this.Password;
        }

        [ExceptionAdvice(Handled = true, HandledReturnDefaultValue = -1f)]
        public float GetCount(String token)
        {
            Int32 n = 0;
            return 1 / n;
        }

        public List<User> GetAllUser(String token)
        {
            if (IsUserLogin(token))
            {
                return allUsers;
            }
            return null;
        }
    }
}
