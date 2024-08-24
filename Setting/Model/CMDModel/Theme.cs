using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class ThemeSend : SendBase<ThemeData>
    {
        public ThemeSend()
        {
            cmd = "theme";
        }
    }
    public class ThemeData
    {
        public string name { get; set; }
        public string model { get; set; }
    }
    public class ThemeRetrun : ReturnBase<string>
    {
        public ThemeRetrun()
        {
            origincmd = "theme";
        }

        public bool HasDetail()
        {
            return code == "0";
        }
    }
}
