using Setting.Model;
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
        public  DeviceInfo CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class CopyThemeEvent
    {
        public DeviceInfo CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class NewThemeEvent
    {
        public DeviceInfo CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class RemoveThemeEvent
    {
        public DeviceInfo CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class ChangeThemeNameEvent
    {
        public DeviceInfo CurrentDevInfo { get; set; }
        public JsonFileInfo JsonFileInfo { get; set; }
    }
}
