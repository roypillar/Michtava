using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Frontend.Models
{
    public class TextsNotificationsViewModel
    {

        public List<Guid> TextsIDList { get; set; }

        public List<Text> Texts { get; set; }
    }
}