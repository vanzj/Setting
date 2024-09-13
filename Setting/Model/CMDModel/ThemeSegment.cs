using GIfTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{

    public class ThemeSegmentSend : SendBase<List<ThemeSegmentData>>
    {
        public ThemeSegmentSend()
        {
            cmd = "themeSegment";
        }
    }


    public class ThemeSegmentRetrun : ReturnBase<string>
    {
        public ThemeSegmentRetrun()
        {
            origincmd = "themeSegment";
        }
    }

}
