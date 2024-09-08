using GIfTool;
using Newtonsoft.Json;
using Setting.Model;
using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Setting.Helper
{
   public class ABCHelper
    {
        public static List<PointItem> GetPonitItems(int start,int left,int right,ABCEnum aBC,Color  color)
        {
            var result = new List<PointItem>();
            if (aBC == ABCEnum.one)
            {
                result= GetOnePonitItems(color);
            }
            if (aBC == ABCEnum.two)
            {
                result = GetTwoPonitItems(color);
            }
            if (aBC == ABCEnum.three)
            {
                result = GetThreePonitItems(color);
            }
            if (aBC == ABCEnum.four)
            {
                result = GetFourPonitItems(color);
            }
            if (aBC == ABCEnum.five)
            {
                result = GetFivePonitItems(color);
            }
            if (aBC == ABCEnum.six)
            {
                result = GetSixPonitItems(color);
            }
            if (aBC == ABCEnum.seven)
            {
                result = GetSevenPonitItems(color);
            }
            if (aBC == ABCEnum.eight)
            {
                result = GetEightPonitItems(color);
            }
            if (aBC == ABCEnum.nine)
            {
                result = GetNinePonitItems(color);
            }
            if (aBC == ABCEnum.zero)
            {
                result = GetZorePonitItems(color);
            }
            if (aBC == ABCEnum.percent)
            {
                result = GetPercentPonitItems(color);
            }
            if (aBC == ABCEnum.celsius)
            {
                result = GetCelsiusPonitItems(color);
            }
            if (aBC == ABCEnum.lianjiexian)
            {
                result = GetLianjiexianPonitItems(color);
            }

            return result;
        }

         private static List<PointItem> GetOnePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":false},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":true},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":true},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }

        private static List<PointItem> GetTwoPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetThreePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetFourPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":false},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":true}]");


                return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetFivePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetSixPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetSevenPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetEightPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetNinePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetZorePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetPercentPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":3,\"Y\":0,\"HasColor\":false},{\"X\":4,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":true},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":3,\"Y\":1,\"HasColor\":true},{\"X\":4,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":3,\"Y\":2,\"HasColor\":false},{\"X\":4,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":true},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":3,\"Y\":3,\"HasColor\":true},{\"X\":4,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":false},{\"X\":3,\"Y\":4,\"HasColor\":true},{\"X\":4,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetCelsiusPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":false},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":3,\"Y\":0,\"HasColor\":true},{\"X\":4,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":3,\"Y\":1,\"HasColor\":false},{\"X\":4,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":3,\"Y\":2,\"HasColor\":false},{\"X\":4,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":3,\"Y\":3,\"HasColor\":false},{\"X\":4,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":true},{\"X\":3,\"Y\":4,\"HasColor\":true},{\"X\":4,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
        private static List<PointItem> GetLianjiexianPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":false},{\"X\":1,\"Y\":0,\"HasColor\":false},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":3,\"Y\":0,\"HasColor\":false},{\"X\":4,\"Y\":0,\"HasColor\":false},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":3,\"Y\":1,\"HasColor\":false},{\"X\":4,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":3,\"Y\":2,\"HasColor\":false},{\"X\":4,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":3,\"Y\":3,\"HasColor\":false},{\"X\":4,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":false},{\"X\":3,\"Y\":4,\"HasColor\":false},{\"X\":4,\"Y\":4,\"HasColor\":false}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : ColorConst.BackGroupColor)).ToList(); ;
        }
    }

    public enum ABCEnum
    {
        zero=0,
        one=1,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine,
       
        /// <summary>
        /// %
        /// </summary>
        percent,
        
        /// <summary>
        /// ℃
        /// </summary>
        celsius,

        /// <summary>
        /// 连接线
        /// </summary>
        lianjiexian
        
    }
}
