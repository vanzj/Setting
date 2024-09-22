using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SerialCommunication;

namespace Setting.Helper
{
   public class MYSerialDevice
    {
        public string COMID { get; set; }
        public bool IsConnect { get; set; }
        public SerialDevice SerialDevice { get; set; }
    }
}
