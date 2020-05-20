using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class ProjectsHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
       

        public bool IsUserOnProject(string userId, int projectId)
        {
            var project = db.Projects.Find(projectId);
            var flag = project.Users.Any(u => u.Id == userId);
            return( flag);
        }

        public ICollection<Project> ListUserProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);

            var projects = user.Projects.ToList();
            return (projects);
        }
        public List<Project> ListMyProjects()
        {
            var myProjects = new List<Project>();
            // getting the user Id outside of a controller
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                    myProjects.AddRange(db.Projects);
                    break;
                case "PM":

                    // myTickets.AddRange(user.Projects.Where(p => p.IsArchived == false).SelectMany(p => p.Tickets));
                    myProjects.AddRange(db.Projects.Where(p => p.ProjectManagerId == userId));
                    break;
                case "Dev":
                    myProjects.AddRange(db.Projects.Where(t => t.IsArchived == false).Where(t => t.DeveloperId == userId));
                    break;
                case "Sub":
                    myProjects.AddRange(db.Projects.Where(t => t.IsArchived == false).Where(t=>t.SubmitterId == userId));
                    break;

            }

            return (myProjects);
        }


        //public ICollection<Project> ListNonUserProjects(string userId )
        //{
        //    ApplicationUser user = db.Users.Find(userId);

        //    var projects = db.Projects.AddRange(db.Projects.Where(p => p.ProjectManagerId != userId).ToList());

        //   // var developers = WTF
        //    var onProject = projectHelper.UsersOnProject(1).ToList();
        //    // does this take the first object and remove the anything from the second object 
        //    // return developers.Intersect(onProject).ToList();
        //    return user.Projects;
        //}

        public void AddUserToProject(string userId, int projectId)
        {
            if (!IsUserOnProject(userId,projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var newUser = db.Users.Find(userId);

                proj.Users.Add(newUser);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            if (IsUserOnProject(userId,projectId))
            {
                Project proj = db.Projects.Find(projectId);
                var delUsers = db.Users.Find(userId);

                proj.Users.Remove(delUsers);
                //may not be needed
                //db.Entry(proj).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<ApplicationUser>UsersOnProject(int projectId)
        {
            return db.Projects.Find(projectId).Users;
        }

        public ICollection<ApplicationUser> UsersNotOnProject(int projectId)
        {
            return db.Users.Where(u => u.Projects.All(p => p.Id != projectId)).ToList();

        }

    }
}