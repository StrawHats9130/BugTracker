using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class HistoriesDisplayHelper
    {
        public static string DisplayData(TicketHistory ticket)
        {
            var db = new ApplicationDbContext();

            var data = "";
            switch (ticket.Property)
            {
                //Create a case for when you need to display something other then a guid or Id
                case "DeveloperId":
                    data = db.Users.FirstOrDefault(u => u.Id == ticket.NewValue).FullName;
                    break;
                default:
                    data = ticket.NewValue;
                    break;
            }


            return data;

        }

    }
}