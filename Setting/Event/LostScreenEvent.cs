using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Setting.Helper
{
    public class LostCurrentScreenEvent
    {
        public bool isLocal { get; set; }
        public List<DeviceInfo> DeviceInfos { get; set; }
    }
    public class ConnectCurrentScreenEvent
    {
        public bool isLocal { get; set; }
        public List<DeviceInfo> DeviceInfos { get; set; }
    }


    public class LostScreenEvent
    {
        public bool isLocal { get; set; }
        public List<DeviceInfo> DeviceInfos { get; set; }
    }
    public class ConnectScreenEvent
    {
        public bool isLocal { get; set; }
        public List<DeviceInfo> DeviceInfos { get; set; }
    }
}
