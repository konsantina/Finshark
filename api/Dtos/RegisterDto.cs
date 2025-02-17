using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{  
    //Αυτό το αρχείο περιέχει τα δεδομένα που λαμβάνει το API από τον χρήστη κατά την εγγραφή:
        public class RegisterDto
    { 
        [Required]
        public string? Username {get; set;}

        [Required]
        [EmailAddress]
        public string? Email {get; set;}

        [Required]
       public string? Password { get; set; }
        
    }
}