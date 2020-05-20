using BugTracker.Helpers;
using BugTracker.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();

        [Authorize(Roles = "PM, Admin")]
        public ActionResult ManageProjectAssignments()
        {
            var emptyCustomUserDataList = new List<CustomUserData>();

            //I have decided tat it should work this way......
            var users = db.Users.ToList();

            // Load a multi Select List of Users
            ViewBag.UserIds = new MultiSelectList(users, "Id", "FullName");

            //Load ip a Multi Select List of Projects
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            //Load up the View Model
            foreach (var user in users)
            {
                emptyCustomUserDataList.Add(new CustomUserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    ProjectNames = projHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                });
            }

            return View(emptyCustomUserDataList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectAssignments(List<string> userIds, List<int> projectIds, bool addUser)
        {

            if (userIds != null || projectIds != null)
            {
                if (addUser)
                {

                    foreach (var userId in userIds)
                    {
                        foreach (var projectId in projectIds)
                        {
                            projHelper.RemoveUserFromProject(userId, projectId);
                        }
                    }
                    foreach (var userId in userIds)
                    {
                        foreach (var projectId in projectIds)
                        {
                            projHelper.AddUserToProject(userId, projectId);

                        }
                    }

                }
                else
                {
                    foreach (var userId in userIds)
                    {
                        foreach (var projectId in projectIds)
                        {
                            projHelper.RemoveUserFromProject(userId, projectId);
                        }
                    }
                }

            }
            return RedirectToAction("ManageProjectAssignments");
        }



        public ActionResult ManageProjectLevelUsers(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var users = db.Users.ToList();
            // Load a multi Select List of Users
            ViewBag.UserIds = new MultiSelectList(users, "Id", "FullName");

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectLevelUsers(List<string> userIds, int projectId, bool addUser)
        {

            if (userIds != null)
            {
                if (addUser)
                {
                    foreach (var userId in userIds)
                    {   
                            projHelper.RemoveUserFromProject(userId, projectId);
                    }
                    foreach (var userId in userIds)
                    {   
                            projHelper.AddUserToProject(userId, projectId);
                    }
                }
                else
                {
                    foreach (var userId in userIds)
                    {                       
                            projHelper.RemoveUserFromProject(userId, projectId);
                    }
                }
            }
            return RedirectToAction("ManageProjectLevelUsers", new { id = projectId });
        }

        public ActionResult RemoveUsersFromProjects(List<string> userIds, List<int> projectIds)
        {
            if (userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectAssignments");
            }

            foreach (var userId in userIds)
            {
                foreach (var projectId in projectIds)
                {
                    projHelper.RemoveUserFromProject(userId, projectId);
                }
            }

            return RedirectToAction("ManageProjectAssignments");
        }

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "PM, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ProjectManagerId,")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Created = DateTime.Now;
                project.ProjectManagerId = project.ProjectManagerId;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "PM, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProjectManagerId,Created,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Updated = DateTime.Now;

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
