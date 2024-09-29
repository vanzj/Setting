using Setting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi;

namespace Setting.Event
{
  public  class GetThemeListEvent
    {
        public DeviceInfo device { get; set; }
        public List<JsonFileInfo> jsonFileInfos { get; set; }
    }
}
