using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class RotateSend : SendBase<string>
    {
        public RotateSend(string arge)
        {
            cmd = "rotate";
            data = arge;
        }

    }
    public class RotateRetrun : ReturnBase<string>
    {
        public RotateRetrun()
        {
            origincmd = "rotate";
        }
    }
}
