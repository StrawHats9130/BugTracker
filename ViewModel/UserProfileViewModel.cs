﻿using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.ViewModel
{
    public class UserProfileViewModel
    {
        #region user atributes
        public string Id { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        #endregion

        #region collections user may need access to
        public ICollection<Project> ProjectsIn { get; set; }
        public ICollection<Project> ProjectsOut { get; set; }
        public ICollection<Ticket> TicketsIn { get; set; }
        public ICollection<Ticket> TicetsOut { get; set; }
        public ICollection<TicketIndexViewModel> TicketsVM { get; set; }
        public ICollection<ProjectViewModel> ProjectVM { get; set; }

        public IEnumerable<SelectListItem> ProjectDevelopers { get; set; }
        public IEnumerable<SelectListItem> ProjectSubmitter { get; set; }
        public IEnumerable<SelectListItem> ProjectPojectManagers { get; set; } 
        #endregion

        #region In Drews View Model not sure If I will need it so I put it here
        public string AvitarPath { get; set; }

        #endregion

        #region Empty


        #endregion

    }
}