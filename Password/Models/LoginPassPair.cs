using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password.Models
{
    public class LoginPassPair
    {
        public int Id;
        public string Login;
        public string Pass;

        public LoginPassPair(int id, string login, string pass)
        {
            Id = id;
            Login = login;
            Pass = pass;
        }
        
        public LoginPassPair(string login, string pass)
        {
            Login = login;
            Pass = pass;
        }
    }
}
