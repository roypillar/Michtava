using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Models;
using System.ComponentModel.DataAnnotations;


namespace Domain.ModelView
{
    public class UserViewModel
    {
        [Required]
        public User user { get; set; }

        public List<User> users { get; set; }
    }
}