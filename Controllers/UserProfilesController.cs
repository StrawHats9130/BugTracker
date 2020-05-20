using BugTracker.Models;
using BugTracker.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class UserProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: UserProfiles
        [Authorize]
        public ActionResult EditProfile()
        {
            //Store the Primary key of User in the currentUserId variable
            var currentUserId = User.Identity.GetUserId();

            //The currentUser variable represents the entire User record with all the users info
            var currentUser = db.Users.Find(currentUserId);

            // Creating an instance of UPVM and populating it
            var userProfileVM = new UserProfileViewModel();
            userProfileVM.Id = currentUser.Id;
            userProfileVM.FirstName = currentUser.FirstName;
            userProfileVM.LastName = currentUser.LastName;
            userProfileVM.DisplayName = currentUser.DisplayName;
            userProfileVM.Email = currentUser.Email;

            return View(userProfileVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileViewModel model)
        {
            //I want to tracked User record based on the incoming model.Id
            var currentUser = db.Users.Find(model.Id);
           
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.DisplayName = model.DisplayName;
            currentUser.Email = model.Email;
            currentUser.UserName = model.Email;

            db.SaveChanges();

            return RedirectToAction("EditProfile");
        }
    }
}