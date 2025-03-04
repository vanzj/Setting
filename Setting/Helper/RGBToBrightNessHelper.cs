using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
    public class RGBToBrightNessHelper
    {
        public double Ar = 0.34;
        public double Ag = 0.33;
        public double Ab = 0.33;
        public double maxA = 1;
        public double minA = 0.3;


        public Dictionary<string,double> keyValuePairs = new Dictionary<string,double>();

        static readonly object _object = new object();
        private static RGBToBrightNessHelper instance = null;
        public static RGBToBrightNessHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new RGBToBrightNessHelper();
                        }
                    }
                }
                return instance;
            }
        }

        private RGBToBrightNessHelper() {

            for (double hex = 0; hex < 256; hex++) {
                var R =hex/256.0;
               var rHex =   ((int)hex).ToString("X2");
                keyValuePairs.Add(rHex , R );

            }

        }

        public void Init()
        {

        }
        public double Get(string hexrgb)
        {
            var r = hexrgb.Substring(0, 2);
            var g = hexrgb.Substring(2, 2);
            var b = hexrgb.Substring(4, 2);
            var ar = Ar - ( keyValuePairs[r] * (maxA - minA) * Ar);
             var ab = Ab - ( keyValuePairs[g] * (maxA - minA) * Ag);
            var ag= Ag - ( keyValuePairs[b] * (maxA - minA) * Ab);
            return Math.Round(ar + ab + ag, 2); 
        }

    }
}
