using Newtonsoft.Json;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Setting.Model
{
    public class PointInit
    {
        public PointInit(Color PointColor)
        {
            HasColor = !(PointColor.ToString() == ColorConst.BackGroupColor);
        }
        public int X { get; set; }
        public int Y { get; set; }
        public bool HasColor { get; set; }

      
    }
}
