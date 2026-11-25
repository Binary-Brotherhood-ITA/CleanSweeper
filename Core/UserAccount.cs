using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}