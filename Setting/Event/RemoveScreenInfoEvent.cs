using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Event
{
  public  class RemoveScreenInfoEvent
    {
        public ScreenDeviceInfoViewModel CurrentDevInfo { get; set; }
    }
}
