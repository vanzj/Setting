using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
    public class HardInfoEvent
    {
       public int GPUUse { get; set; }
       public int GPUTemp { get; set; }
        public int CPUUse { get; set; }
        public int CPUTemp { get; set; }
        public int UpLoad { get; set; }
        public int DownLoad { get; set; }
        public ABCEnum UpLoadflag { get; set; }
        public ABCEnum DownLoadflag { get; set; }
    }
}
