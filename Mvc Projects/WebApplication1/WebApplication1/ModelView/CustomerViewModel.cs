using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.ModelView
{
    public class CustomerViewModel
    {
        [Required]
        public Customer customer { get; set; }

        public List<Customer> customers { get; set; }
    }
}