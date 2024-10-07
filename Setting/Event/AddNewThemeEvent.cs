using Setting.Model;
using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webapi;

namespace Setting.Helper
{
   public class InputNewThemeEvent
    {
        public ScreenDeviceInfoViewModel CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class CopyThemeEvent
    {
        public ScreenDeviceInfoViewModel CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class NewThemeEvent
    {
        public ScreenDeviceInfoViewModel CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class RemoveThemeEvent
    {
        public ScreenDeviceInfoViewModel CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class ChangeThemeNameEvent
    {
        public ScreenDeviceInfoViewModel CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
}
