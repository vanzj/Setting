using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class OpenSend : SendBase<string>
    {
        public OpenSend()
        {
            cmd = "open";
            data = "";
        }
    }
    public class OpenRetrun : ReturnBase<string>
    {
        public OpenRetrun()
        {
            origincmd = "open";
        }
    }
}

