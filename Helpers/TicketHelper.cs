using BugTracker.Models;
using BugTracker.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectsHelper projectHelper = new ProjectsHelper();
        public List<Ticket> ListMyTickets()
        {
            var myTickets = new List<Ticket>();
            // getting the user Id outside of a controller
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case  "Admin" :
                    myTickets.AddRange(db.Tickets);
                    break;
                case "PM":
                   // myTickets.AddRange(user.Projects.Where(p => p.IsArchived == false).SelectMany(p => p.Tickets));
                    myTickets.AddRange(db.Projects.Where(p => p.ProjectManagerId == userId).SelectMany(p => p.Tickets));
                    break;
                case "Dev":
                    myTickets.AddRange(db.Tickets.Where(t =>t.IsArchived == false).Where(t=>t.DeveloperId == userId));
                    break;
                case "Sub":
                    myTickets.AddRange(db.Tickets.Where(t=>t.IsArchived==false).Where(t=>t.SubmitterId== userId));
                    break;

            }
 
            return (myTickets);
        }

        public List<Ticket> ListAllTickets()
        {

            return db.Tickets.ToList();
        }
        //Says it is not working as is
        //public ICollection<ApplicationUser> AssignablDevelopers (int projectId)
        //{
        //    var developers = roleHelper.UsersInRole("Dev");
        //    var onProject = projectHelper.UsersOnProject(projectId).ToList();
        //    // does this take the first object and remove the anything from the second object 
        //    return developers.Intersect(onProject).ToList();
        //}

            //Is repacement for the original
        public ICollection<ApplicationUser> AssignableDevelopers(int projectId)
        {
            var developers = roleHelper.UsersInRole("Developer");
            var available = new List<ApplicationUser>();
            foreach (var user in developers)
            {
                if (projectHelper.IsUserOnProject(user.Id, projectId))
                {
                    available.Add(user);
                }
            };
            return available;
        }

        public string  AssignColor()
        {
            return "";
        }

        public List<Ticket> ListMyTicketsByPriority(string priorty)
        {
            var myTickets = new List<Ticket>();
            // getting the user Id outside of a controller
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    myTickets.AddRange(db.Tickets.Where(t=>t.TicketPriorityId == 1));
                    break;
                case "PM":

                    // myTickets.AddRange(db.Projects.Where(p => p.IsArchived == false).SelectMany(p => p.Tickets.Where(t=> t.TicketPriority.Name == priority)));
                    myTickets.AddRange(db.Projects.Where(p => p.IsArchived== false).SelectMany(p => p.Tickets.Where(t=> t.TicketPriorityId == 1)));
                    break;
                    //case "Dev":
                    // myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).SelectMany(t => t.Tickets.Where(t=> t.TicketPriority.Name == priority).Where(t=>t.DeveloperID == userId));
                    // //   myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).Where(t => t.DeveloperId == userId));
                    //    break;
                    //case "Sub":
                    // myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).SelectMany(t => t.Tickets.Where(t=> t.TicketPriority.Name == priority).Where(t=>t.SubmitterId == userId));
                    //   // myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).Where(t => t.SubmitterId == userId));
                    //    break;

            }

            return (myTickets);



        }


    }
}