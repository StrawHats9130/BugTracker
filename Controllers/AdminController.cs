using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        // GET: Admin

        public ActionResult ManageRoles()
        {
            var viewData = new List<CustomUserData>();
            var users = db.Users.ToList();
            foreach (var user in users)
            {
                //preferd way to add data to a list of UserRoleData
                viewData.Add(new CustomUserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleName = rolesHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "UnAssigned"
                });
            }
            //right hand side control: This data will be used to power a Dropdown List in the View
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");

            //Left hand side control: This data will be used to power ListBox in the view
            //using viewbag named UserIds of type MultiSelectList with a list of User, "use this property when submitting to action","display this property "
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");


            return View(viewData);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            //Useing roleHElper to assign their role
            if (userIds != null)
            {
                //remove role from users
                foreach (var userId in userIds)
                {
                    var userRole = rolesHelper.ListUserRoles(userId).FirstOrDefault();

                    if (!string.IsNullOrEmpty(userRole))
                    {
                        rolesHelper.RemoveUserFromRole(userId, userRole);
                    }

                    //add new role to user
                    if (!string.IsNullOrEmpty(roleName))
                    {
                    rolesHelper.AddUserToRole(userId, roleName);
                    }
                }
            }
            return RedirectToAction("ManageRoles");
        }



    }
}