using Setting.Model;
using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
    public class HistroyInitEvent
    {
        public HistoryItem HistoryItem { get; set; }
    }

    public class HistroyAddEvent
    {
        public HistoryItem HistoryItem { get; set; }
    }
    public class InitFromHistroyEvent
    {
        public HistoryEnum HistoryEnum { get; set; }
        public HistoryItem HistoryItem { get; set; }
    }

    public enum HistoryEnum
    {
        ReBack,
        Cancel,
        Redo,
    }

}
