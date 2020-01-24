using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAuth.Models
{
    public class UserCredentials
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Boolean IsAdmin { get; set; }

    }
}