using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class LoginResponse
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Role { get; set; }
    }
}
