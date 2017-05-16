using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class SubjectsNotificationsViewModel
    {
        

        public List<Guid> subjectsIDList { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<Tuple<string,string,Text>> tmpTexts { get; set; }

    }
}