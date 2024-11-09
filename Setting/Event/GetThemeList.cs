using Setting.Model;
using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Setting.Event
{
  public  class GetThemeListEvent
    {
        public ScreenDeviceInfoViewModel device { get; set; }
        public List<JsonFileInfo> jsonFileInfos { get; set; }
    }
}
