using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Models;
using System.ComponentModel.DataAnnotations;


namespace Domain.ModelView
{
    public class CustomerViewModel
    {
        [Required]
        public Customer customer { get; set; }

        public List<Customer> customers { get; set; }
    }
}