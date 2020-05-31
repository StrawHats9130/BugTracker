using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void ManageNotifications(Ticket oldTicket,Ticket newTicket)
        {
            GenerateTicketAssignmentNotifications(oldTicket, newTicket);

            GenerateTicketChangeNotification(oldTicket, newTicket);



        }

        private void GenerateTicketAssignmentNotifications(Ticket oldTicket, Ticket newTicket)
        {
            bool assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            bool unassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            bool reassigned = !assigned && !unassigned && oldTicket.DeveloperId != newTicket.DeveloperId;

            if (assigned)
            {
                var created = DateTime.Now;
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = newTicket.DeveloperId,
                    Subject = "You have been assigned to a Ticket",
                    NotificationBody = $"You have been assigned to Ticket Id:{newTicket.Id} on {created.ToString("MMM, dd yyyy")}. This Ticket is on the {newTicket.Project.Name} project. "
                });
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = oldTicket.Project.ProjectManagerId,
                    Subject = "Ticket Developer Change",
                    NotificationBody = $"{DisplayHelper.ChangeUserIdToFullName(newTicket.DeveloperId)} has been assigned to Ticket Id:{newTicket.Id} on {created.ToString("MMM, dd yyyy")}. This Ticket is on the {newTicket.Project.Name} project. "

                });
            }
            if (unassigned)
            {
                var created = DateTime.Now;
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = oldTicket.DeveloperId,
                    Subject = "You have been unassigned from a Ticket",
                    NotificationBody = $"You have been unassigned from Ticket Id:{newTicket.Id} on {created.ToString("MMM, dd yyyy")}. This Ticket is on the {newTicket.Project.Name} project. "
                   
                });
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = oldTicket.Project.ProjectManagerId,
                    Subject = "Ticket Developer Change",
                    NotificationBody = $"{DisplayHelper.ChangeUserIdToFullName(oldTicket.DeveloperId)} has been unassigned from Ticket Id:{newTicket.Id} on {created.ToString("MMM, dd yyyy")}. This Ticket is on the {newTicket.Project.Name} project. "

                });
            }
            if (reassigned)
            {
                var created = DateTime.Now;
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = newTicket.DeveloperId,
                    Subject = "You have been assigned to a Ticket",
                    NotificationBody = $"You have been assigned to Ticket Id:{newTicket.Id} on {created.ToString("MMM, dd yyyy")}. This Ticket is on the {newTicket.Project.Name} project. "
                });
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = oldTicket.DeveloperId,
                    Subject = "You have been unassigned from a Ticket",
                    NotificationBody = $"You have been unassigned from Ticket Id:{newTicket.Id} on {created.ToString("MMM, dd yyyy")}. This Ticket is on the {newTicket.Project.Name} project. "

                });
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = oldTicket.Project.ProjectManagerId,
                    Subject = "Ticket Developer Change",
                    NotificationBody = $"Ticket Id:{newTicket.Id} Developer has been changed from {DisplayHelper.ChangeUserIdToFullName(oldTicket.DeveloperId)} to {DisplayHelper.ChangeUserIdToFullName(newTicket.DeveloperId)} on {created.ToString("MMM, dd yyyy")}. This Ticket is on the {newTicket.Project.Name} project. "

                });
            }
            db.SaveChanges();
        }

        private void GenerateTicketChangeNotification(Ticket oldTicket, Ticket newTicket)
        {

        }

    }
}