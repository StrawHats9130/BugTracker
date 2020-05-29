using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModel
{
    public class PieChartViewModel
    {

       
        public List <string> Lables { get; set; }
        public List<string> BackgroundColor { get; set; }
        public List<int> Data { get; set; } 

        public PieChartViewModel()
        {
            Lables = new List<string>();
            BackgroundColor = new List<string>();
            Data = new List<int>();
        }


    }

    public class PieChartData
    {
        public string Immediate { get; set; }
        public string High { get; set; }
        public string Medium { get; set; }
        public string Low { get; set; }
        public string None { get; set; }
    }
}