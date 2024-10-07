using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi;

namespace Setting.Event
{
  public  class LostScreenEvent
    {
        public bool isLocal { get; set; }
        public List<DeviceInfo> DeviceInfos { get; set; }
    }
}
