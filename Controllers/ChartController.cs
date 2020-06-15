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


            barChartViewModel.BackgroundColor1 = "Blue";
            barChartViewModel.BackgroundColor2 = "Green";
            

            barChartViewModel.LabelType1 = "Demo Cat 1";
            barChartViewModel.LabelType2 = "Demo Cat 2";
           

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
           


            return Json(barChartViewModel);

        }

        //public JsonResult GetTicketPriorityBarChatData()
        //{
        //    var barChartViewModel = new BarChartResponsiveViewModel();
        //    var barChartTest = new List<BarChartResponsiveViewModel>();
        //    var priorities = db.TicketPriorities.ToList();
        //    var ticket = db.Tickets.ToList();
        //    var i = 10;
        //    var x = 3;
        //    var y = 10;


        //    //barChartViewModel.BackgroundColor1 = "Gold";
        //    //barChartViewModel.BackgroundColor2 = "Red";
        //    //barChartViewModel.BackgroundColor3 = "Green";

        //    //barChartViewModel.LabelType1 = "Test Label 1";
        //    //barChartViewModel.LabelType2 = "Test Label 2";
        //    //barChartViewModel.LabelType3 = "Test Label 3";



        //    foreach (var priority in priorities)
        //    {
        //        var datasetInfo = new List<int>();
        //        var listOfLabels = new List<string>();


        //        foreach (var priorityFill in priorities)
        //        {
        //            listOfLabels.Add(priorityFill.Name);  
        //            var ticketPriorityCount = ticket.Where(tp => tp.TicketPriorityId == priorityFill.Id).ToList().Count;
        //            ticketPriorityCount += i;
        //           datasetInfo.Add(ticketPriorityCount);
        //            i += x;
        //            x += 3;

        //        }


        //        // BarChartTest.BarChartResponsiveViewModels..Add(priority.Name);
        //        barChartTest.Add(new BarChartResponsiveViewModel 
        //        { 
        //        BackgroundColor1 = priority.Color,
        //        LabelType1 = priority.Name,
        //        Labels = listOfLabels,
        //        DataSet1 = datasetInfo
        //        });

        //    }





        //    return Json(barChartTest);

        //}



    }
}