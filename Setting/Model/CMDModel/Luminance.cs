using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
    public class LuminanceSend : SendBase<string>
    {
        public LuminanceSend(string arge)
        {
            cmd = "luminance";
            data = arge;
        }
        
    }
    public class LuminanceRetrun : ReturnBase<string>
    {
        public LuminanceRetrun()
        {
            origincmd = "luminance";
        }
    }
}
