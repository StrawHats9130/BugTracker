using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class MessageHelper
    {

        public static List<TicketNotification> GetUnreadNotifications()
        {
            var unreadTicketNotifications = new List<TicketNotification>();
            var userId = HttpContext.Current.User.Identity.GetUserId();

            if (userId == null)
            {
                return unreadTicketNotifications;
            }

            ApplicationDbContext db = new ApplicationDbContext();
            unreadTicketNotifications = db.TicketNotifications.Where(t => t.RecipientId == userId && !t.IsRead).ToList();

            return unreadTicketNotifications;

        }

    }
}