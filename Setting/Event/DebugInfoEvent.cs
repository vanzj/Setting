using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
    public class DebugInfoEvent
    {
        public string Msg { get; set; }

        public DebugInfoEvent(string msg)
        {
            Msg = msg + "\r\n";
        }
    }
}
