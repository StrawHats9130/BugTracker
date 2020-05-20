using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void ManageHistoryRecordCreation(Ticket oldTicket, Ticket newTicket)
        {
            //now I can compair old ticket to new ticket
            if (oldTicket.Title != newTicket.Title)
            {
                //var newHistoryRecord = new TicketHistory();

                //newHistoryRecord.ChangedOn = (DateTime)newTicket.Updated;
                //newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                //newHistoryRecord.Property = "Title";
                //newHistoryRecord.OldValue = oldTicket.Title;
                //newHistoryRecord.NewValue = newTicket.Title;
                //newHistoryRecord.TicketId = newTicket.Id;

                var newHistoryRecord = new TicketHistory
                {
                    ChangedOn = (DateTime)newTicket.Updated,
                    UserId = HttpContext.Current.User.Identity.GetUserId(),
                    Property = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    TicketId = newTicket.Id
                };
                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)newTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "DeveloperId";
                newHistoryRecord.OldValue = oldTicket.DeveloperId;
                newHistoryRecord.NewValue = newTicket.DeveloperId;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }
            db.SaveChanges();

        }
    }
}