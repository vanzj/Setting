using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Event
{
    public class MsgEvent
    {

        public string Msg { get; set; }

        public MsgEvent(string msg)
        {
            Msg = msg;
        }    
    }
}
