using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Event
{
    public class SendStartEvent
    {
        public SendStartEvent(string devNo)
        {
            DevNo = devNo;
        }

        public string DevNo { get; set; }   
        
    }

    public class SendEndEvent
    {
        public string DevNo { get; set; }

        public SendEndEvent(string devNo)
        {
            DevNo = devNo;
        }
    }
    public class SendStartStoryEvent
    {

    }

    public class SendEndStoryEvent
    {

    }

}
