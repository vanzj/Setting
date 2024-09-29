using Setting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi;

namespace Setting.Helper
{
   public class ThemeItemClickedEvent
    {
        public DeviceInfo CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
}
