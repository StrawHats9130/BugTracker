using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.ViewModel
{
    public class BarChartResponsiveViewModel
    {
        public List<string> Labels { get; set; }
        public string LabelType1 { get; set; }
        public string LabelType2 { get; set; }
        public string LabelType3 { get; set; }
        

        public string BackgroundColor1 { get; set; }
        public string BackgroundColor2 { get; set; }
        public string BackgroundColor3 { get; set; }
       

        public  List<int> DataSet1 { get; set; }
        public List<int> DataSet2 { get; set; }
        public List<int> DataSet3 { get; set; }

        public BarChartResponsiveViewModel()
        {
            Labels = new List<string>();
            DataSet1 = new List<int>(); 
            DataSet2 = new List<int>();
            DataSet3 = new List<int>();
        }


    }

   

}