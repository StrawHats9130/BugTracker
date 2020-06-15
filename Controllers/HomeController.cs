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
        public ActionResult TestDashBoardView()
        {
            var model = new UserProfileViewModel();
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var ticketIndexVMs = new List<TicketIndexViewModel>();
            var projectVMs = new List<ProjectViewModel>();
            var allTickets = db.Tickets.ToList();
            var allPojects = db.Projects.ToList();
            var myTickets = ticketHelper.ListMyTickets();
            var developers = roleHelper.UsersInRole("Dev").ToList();
            var submitters = roleHelper.UsersInRole("Sub").ToList();
            var projectManagers = roleHelper.UsersInRole("PM").ToList();

            if (User.IsInRole("Dev") || User.IsInRole("Sub"))
            {
                foreach (var ticket in myTickets)
                {
                    var ticketComments = new List<TicketComment>();

                    foreach (var comment in ticket.Comments)
                    {
                        if (comment.UserId == userId)
                        {
                            ticketComments.Add(comment);
                        }
                    }
                    ticketIndexVMs.Add(new TicketIndexViewModel
                    {
                        Ticket = ticket,
                        TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId),
                        TicketPriority = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId),
                        TicketType = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId),
                        Developer = new SelectList(developers, "Id", "Email", ticket.DeveloperId),
                        MyTicketComments = ticketComments
                    });
                }
            }

            if (User.IsInRole("Admin") || User.IsInRole("PM"))
            {
                foreach (var project in allPojects)
                {
                    projectVMs.Add(new ProjectViewModel
                    {
                        Project = project,
                        Developer = new SelectList(developers, "Id", "Email", project.DeveloperId),
                        Submitter = new SelectList(submitters, "Id", "Email", project.SubmitterId),
                        ProjectManger = new SelectList(projectManagers, "Id","Email",project.ProjectManagerId)

                    });



                }

                foreach (var ticket in allTickets)
                {


                    ticketIndexVMs.Add(new TicketIndexViewModel
                    {
                        Ticket = ticket,
                        TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId),
                        TicketPriority = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId),
                        TicketType = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId),
                        Developer = new SelectList(developers, "Id", "Email", ticket.DeveloperId),

                    });
                }
            }





            //var model = new UserProfileViewModel();
            //var userId = User.Identity.GetUserId();
            //var user = db.Users.Find(userId);
            //    model.AvitarPath = user.AvatarPath;
            //    model.FullName = user.FullName;
            model.Id = userId;
            model.ProjectsIn = projHelper.ListUserProjects(userId);
            model.ProjectsOut = db.Projects.ToList();
            model.TicketsIn = ticketHelper.ListMyTickets();
            model.TicetsOut = ticketHelper.ListTicketsNotBelongingToUser();
            model.Role = roleHelper.ListUserRoles(userId).FirstOrDefault();
            model.TicketsVM = ticketIndexVMs;
            model.ProjectVM = projectVMs;
            //foreach (var project in model.ProjectsOut)
            //{

            //}



            return View(model);
        }


        
        public ActionResult Dashboard()
        {


            var model = new UserProfileViewModel();
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var ticketIndexVMs = new List<TicketIndexViewModel>();
            var projectVMs = new List<ProjectViewModel>();
            var allTickets = db.Tickets.ToList();
            var allPojects = db.Projects.ToList();
            var myTickets = ticketHelper.ListMyTickets();
            var developers = roleHelper.UsersInRole("Dev").ToList();
            var submitters = roleHelper.UsersInRole("Sub").ToList();
            var projectManagers = roleHelper.UsersInRole("PM").ToList();

            if (User.IsInRole("Dev") || User.IsInRole("Sub"))
            {
                foreach (var ticket in myTickets)
                {
                    var ticketComments = new List<TicketComment>();
                    
                    foreach (var comment in ticket.Comments)
                    {
                        if (comment.UserId == userId)
                        {
                            ticketComments.Add(comment);
                        }
                    }
                    ticketIndexVMs.Add(new TicketIndexViewModel
                    {
                        Ticket = ticket,
                        TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId),
                        TicketPriority = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId),
                        TicketType = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId),
                        Developer = new SelectList(developers, "Id", "Email", ticket.DeveloperId),
                        MyTicketComments = ticketComments
                    });
                }
            }

            if (User.IsInRole("Admin") || User.IsInRole("PM"))
            {
                foreach (var project in allPojects)
                {
                    projectVMs.Add(new ProjectViewModel
                    {
                        Project = project,
                        Developer = new SelectList(developers, "Id","FullName",project.DeveloperId),
                        Submitter = new SelectList(submitters,"Id","FullName",project.SubmitterId),
                        ProjectManger = new SelectList(projectManagers,"Id","FullName",project.ProjectManagerId)

                    }); 



                }

                foreach (var ticket in allTickets)
                {


                    ticketIndexVMs.Add(new TicketIndexViewModel
                    {
                        Ticket = ticket,
                        TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId),
                        TicketPriority = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId),
                        TicketType = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId),
                        Developer = new SelectList(developers, "Id", "Email", ticket.DeveloperId),

                    });
                }
            }





           
            model.Id = userId;
            model.ProjectsIn = projHelper.ListMyProjects();
            model.ProjectsOut = db.Projects.ToList();
            model.TicketsIn = ticketHelper.ListMyTickets();
            model.TicetsOut = ticketHelper.ListTicketsNotBelongingToUser();
            model.Role = roleHelper.ListUserRoles(userId).FirstOrDefault();
            model.TicketsVM = ticketIndexVMs;
            model.ProjectVM = projectVMs;
            //foreach (var project in model.ProjectsOut)
            //{

            //}



            return View(model);
        }



        public ActionResult Dismiss()
        {
            return View();
        }

        public ActionResult UserProfile()
        {
            var model = new UserProfileViewModel();
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            var ticketIndexVMs = new List<TicketIndexViewModel>();
            var projectVMs = new List<ProjectViewModel>();
            var allTickets = db.Tickets.ToList();
            var allPojects = db.Projects.ToList();
            var myTickets = ticketHelper.ListMyTickets();
            var developers = roleHelper.UsersInRole("Dev").ToList();
            var submitters = roleHelper.UsersInRole("Sub").ToList();
            var projectManagers = roleHelper.UsersInRole("PM").ToList();

            if (User.IsInRole("Dev") || User.IsInRole("Sub"))
            {
                foreach (var ticket in myTickets)
                {
                    var ticketComments = new List<TicketComment>();

                    foreach (var comment in ticket.Comments)
                    {
                        if (comment.UserId == userId)
                        {
                            ticketComments.Add(comment);
                        }
                    }
                    ticketIndexVMs.Add(new TicketIndexViewModel
                    {
                        Ticket = ticket,
                        TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId),
                        TicketPriority = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId),
                        TicketType = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId),
                        Developer = new SelectList(developers, "Id", "Email", ticket.DeveloperId),
                        MyTicketComments = ticketComments
                    });
                }
            }

            if (User.IsInRole("Admin") || User.IsInRole("PM"))
            {
                foreach (var project in allPojects)
                {
                    projectVMs.Add(new ProjectViewModel
                    {
                        Project = project,
                        Developer = new SelectList(developers, "Id", "Email", project.DeveloperId),
                        Submitter = new SelectList(submitters, "Id", "Email", project.SubmitterId),
                        ProjectManger = new SelectList(projectManagers, "Id","Email" , project.ProjectManagerId)

                    });



                }

                foreach (var ticket in allTickets)
                {


                    ticketIndexVMs.Add(new TicketIndexViewModel
                    {
                        Ticket = ticket,
                        TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId),
                        TicketPriority = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId),
                        TicketType = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId),
                        Developer = new SelectList(developers, "Id", "Email", ticket.DeveloperId),

                    });
                }
            }





            //var model = new UserProfileViewModel();
            //var userId = User.Identity.GetUserId();
            //var user = db.Users.Find(userId);
            //    model.AvitarPath = user.AvatarPath;
            //    model.FullName = user.FullName;
            model.Id = userId;
            model.ProjectsIn = projHelper.ListUserProjects(userId);
            model.ProjectsOut = db.Projects.ToList();
            model.TicketsIn = ticketHelper.ListMyTickets();
            model.TicetsOut = ticketHelper.ListTicketsNotBelongingToUser();
            model.Role = roleHelper.ListUserRoles(userId).FirstOrDefault();
            model.TicketsVM = ticketIndexVMs;
            model.ProjectVM = projectVMs;
            //foreach (var project in model.ProjectsOut)
            //{

            //}



            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SpecialEdit(int ticketId, int ticketStatusId, string developerId, int ticketTypeId, int ticketPriorityId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);


            var newTicket = db.Tickets.Find(ticketId);
            if (developerId == "")
            {
                newTicket.DeveloperId = null;

            }
            else
            {
                newTicket.DeveloperId = developerId;
            }
            newTicket.TicketTypeId = ticketTypeId;
            newTicket.TicketPriorityId = ticketPriorityId;
            newTicket.TicketStatusId = ticketStatusId;
            newTicket.Updated = DateTime.Now;
            db.SaveChanges();

            //historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);
            //notificationHelper.ManageNotifications(oldTicket, newTicket);

            return RedirectToAction("Index");
        }

        public ActionResult ProjectSpecialEdit(int projectId,  string developerId, string submitterId, string projectManagerId)
        {
            var oldProject = db.Projects.AsNoTracking().FirstOrDefault(t => t.Id == projectId);


            var newProject = db.Projects.Find(projectId);
            if (developerId == "")
            {
                newProject.DeveloperId = null;

            }
            else
            {
                newProject.DeveloperId = developerId;
            }
            newProject.DeveloperId= developerId;
            newProject.SubmitterId = submitterId;
            newProject.ProjectManagerId = projectManagerId;
            newProject.Updated = DateTime.Now;
            db.SaveChanges();

            //historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);
            //notificationHelper.ManageNotifications(oldTicket, newTicket);

            return RedirectToAction("Dashboard");
        }
    }
}