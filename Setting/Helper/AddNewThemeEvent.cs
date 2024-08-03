using Setting.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
   public class AddNewThemeEvent
    {
       public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class CopyThemeEvent
    {
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class RemoveThemeEvent
    {
        public JsonFileInfo JsonFileInfo { get; set; }
    }
    public class ChangeThemeNameEvent
    {
        public JsonFileInfo JsonFileInfo { get; set; }
    }
}
