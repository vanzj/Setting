using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Setting.Event
{
  public  class KeyDownEvent
    {
        public Key Key { get; set; }
    }

    public class KeyDownEventTheme
    {

        public string FileName { get; set; }
        public Key Key { get; set; }
    }
    public class KeyDownEventScreen
    {

        public string DevNo { get; set; }
        public Key Key { get; set; }
    }
}
