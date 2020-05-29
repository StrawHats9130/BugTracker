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
                var ticketPriorityCount = ticket.Where(tp => tp.Id == priority.Id).ToList();
                pieChartViewModel.Data.Add(ticketPriorityCount.Count);
            }
            return Json(pieChartViewModel);

        }
        //var pieChartCanvas = $('#pieChart1').get(0).getContext('2d')
        //var pieData = { labels: [ 'Immediate',  'High', 'Medium', 'Low', 'None',  ],

        //    datasets: [
        //        {
        //            data: [700, 500, 400, 600, 300, 100],
        //            backgroundColor: ['#f56954', '#00a65a', '#f39c12', '#00c0ef', '#3c8dbc', '#d2d6de'],
        //        }
        //    ]
    }
}