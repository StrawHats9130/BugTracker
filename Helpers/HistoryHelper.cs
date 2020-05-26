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

            if (oldTicket.Description != newTicket.Description)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)newTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "Description";
                newHistoryRecord.OldValue = oldTicket.Description;
                newHistoryRecord.NewValue = newTicket.Description;
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)newTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketPriorityId";
                newHistoryRecord.OldValue = oldTicket.TicketPriorityId.ToString();
                newHistoryRecord.NewValue = newTicket.TicketPriorityId.ToString();
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)newTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketStatusId";
                newHistoryRecord.OldValue = oldTicket.TicketStatusId.ToString();
                newHistoryRecord.NewValue = newTicket.TicketStatusId.ToString();
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)newTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "TicketTypeId";
                newHistoryRecord.OldValue = oldTicket.TicketTypeId.ToString();
                newHistoryRecord.NewValue = newTicket.TicketTypeId.ToString();
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.IsArchived != newTicket.IsArchived)
            {
                var newHistoryRecord = new TicketHistory();

                newHistoryRecord.ChangedOn = (DateTime)newTicket.Updated;
                newHistoryRecord.UserId = HttpContext.Current.User.Identity.GetUserId();
                newHistoryRecord.Property = "IsArchived";
                newHistoryRecord.OldValue = oldTicket.IsArchived.ToString();
                newHistoryRecord.NewValue = newTicket.IsArchived.ToString();
                newHistoryRecord.TicketId = newTicket.Id;

                db.TicketHistories.Add(newHistoryRecord);
            }


            db.SaveChanges();
            //Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,SubmitterId,DeveloperId,Title,Description,Created,IsArchived"
            //TicketTypeId,IsArchived"
        }
    }
}