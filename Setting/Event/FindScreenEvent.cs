using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Setting.Event
{
  public  class FindScreenEvent
    {
        public bool isLocal { get; set; }
        public List<DeviceInfo> DeviceInfos { get; set; }
    }
}
