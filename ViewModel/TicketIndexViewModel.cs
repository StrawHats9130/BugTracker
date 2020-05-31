using BugTracker.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BugTracker.Helpers
{
    public class TicketIndexViewModel
    {
        public Ticket Ticket { get; set; }

        public IEnumerable<SelectListItem> TicketStatus { get; set; }
        public IEnumerable<SelectListItem> Developer { get; set; }

        public IEnumerable<SelectListItem> TicketType { get; set; }

        public IEnumerable<SelectListItem> TicketPriority { get; set; }
        public ICollection<Ticket> TicketsIn { get; set; }

        public ICollection<TicketComment> MyTicketComments { get; set; }

    }
}