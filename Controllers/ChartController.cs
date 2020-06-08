using BugTracker.Models;
using BugTracker.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class ChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Chart
        public JsonResult GetTicketPriorityPieChatData()
        {
            var pieChartViewModel = new PieChartViewModel();
            var priorities = db.TicketPriorities.ToList();
            var ticket = db.Tickets.ToList();

            foreach (var priority in priorities)
            {
                pieChartViewModel.Lables.Add(priority.Name);
                pieChartViewModel.BackgroundColor.Add(priority.Color);
                var ticketPriorityCount = ticket.Where(tp => tp.TicketPriorityId == priority.Id).ToList().Count;
                pieChartViewModel.Data.Add(ticketPriorityCount);
            }
            return Json(pieChartViewModel);

        }

        

        public JsonResult GetTicketPriorityBarChatData()
        {
            var barChartViewModel = new BarChartResponsiveViewModel();
            var priorities = db.TicketPriorities.ToList();
            var ticket = db.Tickets.ToList();
            var i = 10;
            var x = 10;
            var y = 10;
           

           barChartViewModel.BackgroundColor1 = "Gold";
            barChartViewModel.BackgroundColor2 = "Red";
            barChartViewModel.BackgroundColor3 = "Green";

            barChartViewModel.LabelType1 = "Test Label 1"; 
            barChartViewModel.LabelType2 = "Test Label 2"; 
            barChartViewModel.LabelType3 = "Test Label 3";

            foreach (var priority in priorities)
            {
                barChartViewModel.Labels.Add(priority.Name);
               
                var ticketPriorityCount = ticket.Where(tp => tp.TicketPriorityId == priority.Id).ToList().Count;
                ticketPriorityCount += i;
                barChartViewModel.DataSet1.Add(ticketPriorityCount);
                i += 5;
            }
            foreach (var priority in priorities)
            {
                var ticketPriorityCount = ticket.Where(tp => tp.TicketPriorityId == priority.Id).ToList().Count;
                ticketPriorityCount += i;
                barChartViewModel.DataSet2.Add(ticketPriorityCount);
                x += 7;
            }
            foreach (var priority in priorities)
            {
                var ticketPriorityCount = ticket.Where(tp => tp.TicketPriorityId == priority.Id).ToList().Count;
                ticketPriorityCount += i;
                barChartViewModel.DataSet3.Add(ticketPriorityCount);
                y += 6;
            }


            return Json(barChartViewModel);

        }

    }
}