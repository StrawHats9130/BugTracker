

using BugTracker.Helpers;
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
        // GET: Tickets


        public ActionResult Dashboard(int id)
        {


            return View(db.Tickets.Find(id));
        }




        public ActionResult Index()
        {
            //var tickets = db.Tickets.Include(t => t.Developer).Include(t => t.Project).Include(t => t.Submitter);
            //return View(tickets.ToList());
            var ticketIndexVMs = new List<TicketIndexViewModel>();
            var allTickets = db.Tickets.ToList();
            foreach (var ticket in allTickets)
            {
                ticketIndexVMs.Add(new TicketIndexViewModel
                {
                    Ticket = ticket,
                    TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId)
                });
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
        [Authorize(Roles ="Admin, Dev, Sub, PM")]
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
                notificationHelper.ManageNotifications(oldTicket,newTicket);
                return RedirectToAction("Index", "Tickets");
            }
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            return View(ticket);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SpecialEdit(int ticketId, int ticketStatusId)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticketId);


            var ticket = db.Tickets.Find(ticketId);
            ticket.TicketStatusId = ticketStatusId;
            ticket.Updated = DateTime.Now;
            db.SaveChanges();

            historyHelper.ManageHistoryRecordCreation(oldTicket, ticket);

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
