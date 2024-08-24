using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class ThemeSendStartSend : SendBase<ThemeSendStartData>
    {
        public ThemeSendStartSend()
        {
            cmd = "themeSendStart";
        }
    }
    public class ThemeSendStartData
    {
        public string name { get; set; }
        public int brightness { get; set; }
        public string frameCount { get; set; }
        public string frameRate { get; set; }
        public string count { get; set; }
    }
    public class ThemeSendStartRetrun : ReturnBase<string>
    {
        public ThemeSendStartRetrun()
        {
            origincmd = "themeSendStart";
        }

        public bool HasDetail()
        {
            return code == "1";
        }
    }
}
