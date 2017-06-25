using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Areas.Administration.Models.SchoolClasses
{
    public class AddStudentToClassViewModel
    {
        public string schoolClassId { get; set; }
        public string userName { get; set; }
    }
}