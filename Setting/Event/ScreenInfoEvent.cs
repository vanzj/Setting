using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Event
{
    public class ScreenInfoEvent
    {
        public ScreenInfoEvent(bool isOpen, int lum, bool isRotate, string devNo)
        {
            IsOpen = isOpen;
            this.lum = lum;
            IsRotate = isRotate;
            DevNo = devNo;
        }

        public bool IsOpen { get; set; }
        public int lum { get; set; }
        public bool IsRotate { get; set; }

        public string DevNo { get; set; }
    }
}
