using BugTracker.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BugTracker.Helpers
{
    public class TicketIndexViewModel
    {
        public Ticket Ticket { get; set; }

        public IEnumerable<SelectListItem> TicketStatus { get; set; }

    }
}