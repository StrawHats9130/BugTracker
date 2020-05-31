using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class DisplayHelper
    {
        public static string ChangeUserIdToFullName(string userId)
        {
            var userFullName = "";

            if (userId != null )
            {
            var db = new ApplicationDbContext();

             userFullName = db.Users.FirstOrDefault(u => u.Id == userId).FullName;
            }
            else
            {
                userFullName = "Unassigned";
            }
            return userFullName;
        }

        public static string ChangeTicketStatusIdToName(int statusId)
        {
            var db = new ApplicationDbContext();


            return db.TicketStatus.FirstOrDefault(ts=>ts.Id == statusId).Name;
        }

        public static string ChangeTicketPriorityIdToName(int priorityId)
        {
            var db = new ApplicationDbContext();
            return db.TicketPriorities.FirstOrDefault(tp => tp.Id == priorityId).Name;
        }

        public static string ChangeTicketTypeIdToName(int typeId)
        {
            var db = new ApplicationDbContext();
            return db.TicketTypes.FirstOrDefault(tt => tt.Id == typeId).Name;
        }

    }
}