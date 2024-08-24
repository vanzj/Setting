using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class CloseSend : SendBase<string>
    {
        public CloseSend()
        {
            cmd = "close";
            data = "";
        }
    }
    public class CloseRetrun : ReturnBase<string>
    {
        public CloseRetrun()
        {
            origincmd = "close";
        }
    }
}

