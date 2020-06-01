

using BugTracker.Helpers;
using BugTracker.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace BugTracker.Models
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ProjectsHelper projHelper = new ProjectsHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private UserRolesHelper rolesHelper = new UserRolesHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        // GET: Tickets


        public ActionResult Dashboard(int id)
        {
            var userId = User.Identity.GetUserId();
            var ticketDashboardVM = new TicketDashboardViewModel
            {
                Ticket = db.Tickets.Find(id),
                UserTicketNotifications = db.TicketNotifications.Where(tn => tn.RecipientId == userId).ToList(),
                UsersTicketAttachments = db.TicketAttachments.Where(ta=> ta.UserID == userId).ToList(),
                UsersTicketComments = db.TicketComments.Where(tc=>tc.UserId == userId).ToList()
            };


            return View(ticketDashboardVM);
        }




        public ActionResult Index()
        {
            //var tickets = db.Tickets.Include(t => t.Developer).Include(t => t.Project).Include(t => t.Submitter);
            //return View(tickets.ToList());
            var ticketIndexVMs = new List<TicketIndexViewModel>();
            var allTickets = db.Tickets.ToList();
            var myTickets = ticketHelper.ListMyTickets();
            var developers = rolesHelper.UsersInRole("Dev").ToList();
            if (User.IsInRole("Dev") || User.IsInRole("Sub"))
            {
                foreach (var ticket in myTickets)
                {
                    var ticketComments = new List<TicketComment>();
                    var userId = User.Identity.GetUserId();
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

            return View(ticketIndexVMs);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Sub")]
        public ActionResult Create(int? projectId)
        {
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName");

            var myUserId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(myUserId);
            if (projectId != null)
            {
                ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            }



            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            var newTicket = new Ticket();
            if (projectId != null)
            {
                newTicket.ProjectId = (int)projectId;
            }
            return View(newTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Sub")]
        public ActionResult Create([Bind(Include = "ProjectId,TicketTypeId,TicketPriorityId,Title,Description,")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.SubmitterId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "New").Id;
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName");

            var myUserId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(myUserId);
            if (ticket.ProjectId == 0)
            {
                ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            }



            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "Email");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            //ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            //ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            //ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            //ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TickeStatusId);
            //ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, Dev, Sub, PM")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = db.Tickets.Find(id);

            var currentUserId = User.Identity.GetUserId();

            //adding some additional security to determine if the user should have access to ticket
            var authorized = true;


            if ((User.IsInRole("Dev") && ticket.DeveloperId != currentUserId) ||
                (User.IsInRole("Sub") && ticket.SubmitterId != currentUserId))
            {
                authorized = false;
            }

            if (!authorized)
            {
                TempData["UnathorizedTicketAccess"] = $"You are not authorized to Edit Ticket {id}";
                return RedirectToAction("Dashboard", "Home");
            }

            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "Email", ticket.DeveloperId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,SubmitterId,DeveloperId,Title,Description,Created,IsArchived")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {

                //AsNoTracking() get a Memento(old version) Ticket object   
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);


                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                //Use the History Helper to intelligently create the appropriate records
                historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);

                //Use the Notificaion Helper to intelligently create the appropriate records
                notificationHelper.ManageNotifications(oldTicket, newTicket);
                return RedirectToAction("Index", "Tickets");
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            return View(ticket);
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

            historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);
            notificationHelper.ManageNotifications(oldTicket, newTicket);

            return RedirectToAction("Index");
        }


        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
