using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Event
{
   public class ScreenReConnactEvent
    {
        public string ComId { get; set; }
        public ScreenDeviceInfoViewModel CurrentDevInfo { get; set; }
    }
}
