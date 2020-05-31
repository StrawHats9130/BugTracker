using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModel
{
    public class TicketDashboardViewModel 
    {
        public Ticket Ticket { get; set; }
        public ICollection<TicketComment> UsersTicketComments { get; set; }
        public ICollection<TicketAttachment> UsersTicketAttachments { get; set; }

        public ICollection<TicketNotification> UserTicketNotifications { get; set; }


    }
}