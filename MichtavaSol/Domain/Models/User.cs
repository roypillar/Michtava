using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Models
{
    public class User
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "length must be between 2 and 50 characters")]
        public string LastName { get; set; }

        [Key]           //must say who is the key in the db table..
        [Required]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "User Number must Contain 4 digits")]
        public string UserNumber { get; set; }

        /*    public User(string fname,string lname,string num){
                FirstName = fname;
                LastName = lname;
                UserNumber = num;
            }
         */
    }
}