using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProjectViewModel
    {

        public Project Project { get; set; }

        public IEnumerable<SelectListItem> Submitter { get; set; }
        public IEnumerable<SelectListItem> Developer { get; set; }

        public IEnumerable<SelectListItem> ProjectManger { get; set; }

       
       

       
    }
}