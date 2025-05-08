using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Setting.Helper
{
    public class LostCurrentScreenEvent
    {
       public string DevNo { get; set; }
    }
    public class ConnectCurrentScreenEvent
    {
        public string DevNo { get; set; }
        public ConnectCurrentScreenEvent(string devNo) {
        
            DevNo = devNo;
        }
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
