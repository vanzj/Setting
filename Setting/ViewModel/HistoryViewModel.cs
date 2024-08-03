using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.ViewModel
{
   public class HistoryViewModel
    {
        public  List<HistoryItem> HistoryItems { get; set; }

        public  int Index { get; set; }

       

    }
    public class HistoryItem
    {
        public string Title { get; set; }

        public List<PointItem> PointItems { get; set; }
    }
   
}
