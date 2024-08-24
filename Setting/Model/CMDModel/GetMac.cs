using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class getMacSend : SendBase<string>
    {
        public getMacSend()
        {
            cmd = "getMac";
            data = "";
        }
    }
    public class getMacRetrun : ReturnBase<string>
    {
        public getMacRetrun()
        {
            origincmd = "getMac";
        }
    }
}
