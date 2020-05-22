using BugTracker.Helpers;
using BugTracker.Models;
using BugTracker.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Dashboard()
        {
            /* //creating an application user for the Icollection of Projects
             var user = new ApplicationUser();
             //Getting the current user Id
             var userId = User.Identity.GetUserId();
             //populatinf Projects with a list of current users projects
             user.Projects = projHelper.ListUserProjects(userId).ToList();*/

           

            return View(projHelper.ListMyProjects().ToList());
        }

        public ActionResult UserProfile()
        {
            var model = new UserProfileViewModel();
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
        //    model.AvitarPath = user.AvatarPath;
        //    model.FullName = user.FullName;
            model.Id = userId;
            model.ProjectsIn = projHelper.ListUserProjects(userId);
           
            model.TicketsIn = ticketHelper.ListMyTickets();
            model.TicetsOut = ticketHelper.ListTicketsNotBelongingToUser();
            model.Role = roleHelper.ListUserRoles(userId).FirstOrDefault();

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactAsync ( EmailModel model)
        {

            try { 

            var emailAddress = WebConfigurationManager.AppSettings["emailto"];
            var emailFrom = $"{model.From}<{emailAddress}>";
            var finalBody = $"{model.Name} has sent you the following message <br /> {model.Body} <hr /> {WebConfigurationManager.AppSettings["LegalText"]}";
            var email = new MailMessage(emailFrom, emailAddress)
            {
                Subject = model.Subject,
                Body = finalBody,
                IsBodyHtml = true
            };

            var emailSvc = new EmailService();
            await emailSvc.SendAsync(email);

            ViewBag.Message = "Your contact page.";

           
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }

            return View(new EmailModel());
        }
    }
}