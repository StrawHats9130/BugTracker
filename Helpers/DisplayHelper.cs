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

            if (userId != "" )
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



    }
}