using Entities.Models;
using Frontend.Areas.Administration.Models.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Areas.Administration.Models.SchoolClasses
{
    public class SchoolClassStudentsView
    {
        public SchoolClass schoolClass { get; set; }
        public IEnumerable<StudentListViewModel> studentsListViews { set; get; }
    }
}