using Entities.Models;
using Frontend.Areas.Administration.Models.Students;
using Frontend.Areas.Administration.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Areas.Administration.Models.SchoolClasses
{
    public class SchoolClassTeachersView
    {
        public SchoolClass schoolClass { get; set; }
        public IEnumerable<TeacherListViewModel> teachersListViews { set; get; }
    }
}