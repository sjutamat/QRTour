using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Domain.ViewModel
{
    public class UserViewModel
    {
        public int id { get; set; }
        public string username { get; set; }
    }

    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
