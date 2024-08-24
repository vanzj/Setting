using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{
   public class SendBase<T>
    {
        public string cmd { get; set; }
       public T data { get; set; }
    }


}
