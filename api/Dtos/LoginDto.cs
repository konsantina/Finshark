using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class LoginDto
    {
        [Required] 
        public string UserName { get;  set;}
        public string Password { get;  set;}
        public string aaa { get;  set;}
        public string 22 { get;  set;}

    }
}