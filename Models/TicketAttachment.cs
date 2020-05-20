using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        public int ID { get; set; }

        public string UserID { get; set; }
        public int TicketID { get; set; }

        public string FilePath { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public string FileUrl { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual ApplicationUser USer { get; set; }

    }
}