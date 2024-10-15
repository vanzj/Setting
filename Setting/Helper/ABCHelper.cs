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
        public static List<PointItem> GetPonitItems(int start, int left, int right, ABCEnum aBC, Color color)
        {
            var result = new List<PointItem>();
            if (aBC == ABCEnum.one)
            {
                result = GetOnePonitItems(color);
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
            if (aBC == ABCEnum.GB)
            {
                result = GetGBPonitItems(color);
            }
            if (aBC == ABCEnum.MB)
            {
                result = GetMBPonitItems(color);
            }
            if (aBC == ABCEnum.KB)
            {
                result = GetKBPonitItems(color);
            }
            if (aBC == ABCEnum.Up)
            {
                result = GetUPPonitItems(color);
            }
            if (aBC == ABCEnum.Down)
            {
                result = GetDownPonitItems(color);
            }
            if (aBC == ABCEnum.fengexian)
            {
                result = GetfengexianPonitItems(color);
            }
            return result;
        }

        private static List<PointItem> GetOnePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":false},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":true},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":true},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }

        private static List<PointItem> GetTwoPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetThreePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetFourPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":false},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":true}]");


            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetFivePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetSixPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetSevenPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetEightPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetNinePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetZorePonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetPercentPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":3,\"Y\":0,\"HasColor\":false},{\"X\":4,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":true},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":3,\"Y\":1,\"HasColor\":true},{\"X\":4,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":3,\"Y\":2,\"HasColor\":false},{\"X\":4,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":true},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":3,\"Y\":3,\"HasColor\":true},{\"X\":4,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":true},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":false},{\"X\":3,\"Y\":4,\"HasColor\":true},{\"X\":4,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetCelsiusPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":1,\"Y\":0,\"HasColor\":false},{\"X\":2,\"Y\":0,\"HasColor\":true},{\"X\":3,\"Y\":0,\"HasColor\":true},{\"X\":4,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":3,\"Y\":1,\"HasColor\":false},{\"X\":4,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":3,\"Y\":2,\"HasColor\":false},{\"X\":4,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":3,\"Y\":3,\"HasColor\":false},{\"X\":4,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":true},{\"X\":3,\"Y\":4,\"HasColor\":true},{\"X\":4,\"Y\":4,\"HasColor\":true}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetLianjiexianPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":false},{\"X\":1,\"Y\":0,\"HasColor\":false},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":3,\"Y\":0,\"HasColor\":false},{\"X\":4,\"Y\":0,\"HasColor\":false},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":false},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":3,\"Y\":1,\"HasColor\":false},{\"X\":4,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":1,\"Y\":2,\"HasColor\":false},{\"X\":2,\"Y\":2,\"HasColor\":true},{\"X\":3,\"Y\":2,\"HasColor\":false},{\"X\":4,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":false},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":3,\"Y\":3,\"HasColor\":false},{\"X\":4,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":false},{\"X\":2,\"Y\":4,\"HasColor\":false},{\"X\":3,\"Y\":4,\"HasColor\":false},{\"X\":4,\"Y\":4,\"HasColor\":false}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }

        private static List<PointItem> GetGBPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{ \"HasColor\": false, \"X\": 0, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 1, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 2, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 3, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 4, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 5, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 6, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 7, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 8, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 9, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 10, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 11, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 12, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 13, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 14, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 15, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 16, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 17, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 18, \"Y\": 0, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 0, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 1, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 2, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 3, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 4, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 5, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 6, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 7, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 8, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 9, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 10, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 11, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 12, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 13, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 14, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 15, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 16, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 17, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 18, \"Y\": 1, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 0, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 1, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 2, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 3, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 4, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 5, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 6, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 7, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 8, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 9, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 10, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 11, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 12, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 13, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 14, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 15, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 16, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 17, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 18, \"Y\": 2, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 0, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 1, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 2, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 3, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 4, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 5, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 6, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 7, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 8, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 9, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 10, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 11, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 12, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 13, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 14, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 15, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 16, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 17, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 18, \"Y\": 3, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 0, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 1, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true,\"X\": 2, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 3, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 4, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 5, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 6, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 7, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 8, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 9, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 10, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 11, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 12, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 13, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 14, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 15, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 16, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": true, \"X\": 17, \"Y\": 4, \"IsInDesignMode\": false },{ \"HasColor\": false, \"X\": 18, \"Y\": 4, \"IsInDesignMode\": false }]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetMBPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{ \"HasColor\": true, \"X\": 0, \"Y\": 0 },{ \"HasColor\": false, \"X\": 1, \"Y\": 0 },{ \"HasColor\": false, \"X\": 2, \"Y\": 0 },{ \"HasColor\": true, \"X\": 3, \"Y\": 0 },{ \"HasColor\": false, \"X\": 4, \"Y\": 0 },{ \"HasColor\": true, \"X\": 5, \"Y\": 0 },{ \"HasColor\": true, \"X\": 6, \"Y\": 0 },{ \"HasColor\": true, \"X\": 7, \"Y\": 0 },{ \"HasColor\": false, \"X\": 8, \"Y\": 0 },{ \"HasColor\": false, \"X\": 9, \"Y\": 0 },{ \"HasColor\": false, \"X\": 10, \"Y\": 0 },{ \"HasColor\": false, \"X\": 11, \"Y\": 0 },{ \"HasColor\": false, \"X\": 12, \"Y\": 0 },{ \"HasColor\": true, \"X\": 13, \"Y\": 0 },{ \"HasColor\": false, \"X\": 14, \"Y\": 0 },{ \"HasColor\": false, \"X\": 15, \"Y\": 0 },{ \"HasColor\": true, \"X\": 16, \"Y\": 0 },{ \"HasColor\": true, \"X\": 17, \"Y\": 0 },{ \"HasColor\": true, \"X\": 18, \"Y\": 0 },{ \"HasColor\": true, \"X\": 0, \"Y\": 1 },{ \"HasColor\": true, \"X\": 1, \"Y\": 1 },{ \"HasColor\": true, \"X\": 2, \"Y\": 1 },{ \"HasColor\": true,\"X\": 3, \"Y\": 1 },{ \"HasColor\": false, \"X\": 4, \"Y\": 1 },{ \"HasColor\": true, \"X\": 5, \"Y\": 1 },{ \"HasColor\": false, \"X\": 6, \"Y\": 1 },{ \"HasColor\": false, \"X\": 7, \"Y\": 1 },{ \"HasColor\": true, \"X\": 8, \"Y\": 1 },{ \"HasColor\": false, \"X\": 9, \"Y\": 1 },{ \"HasColor\": false, \"X\": 10, \"Y\": 1 },{ \"HasColor\": false, \"X\": 11, \"Y\": 1 },{ \"HasColor\": true, \"X\": 12, \"Y\": 1 },{ \"HasColor\": false, \"X\": 13, \"Y\": 1 },{ \"HasColor\": false, \"X\": 14, \"Y\": 1 },{ \"HasColor\": true, \"X\": 15, \"Y\": 1 },{ \"HasColor\": false, \"X\": 16, \"Y\": 1 },{ \"HasColor\": false, \"X\": 17, \"Y\": 1 },{ \"HasColor\": false, \"X\": 18, \"Y\": 1 },{ \"HasColor\": true, \"X\": 0, \"Y\": 2 },{ \"HasColor\": false, \"X\": 1, \"Y\": 2 },{ \"HasColor\": false, \"X\": 2, \"Y\": 2 },{ \"HasColor\": true,\"X\": 3, \"Y\": 2 },{ \"HasColor\": false, \"X\": 4, \"Y\": 2 },{ \"HasColor\": true, \"X\": 5, \"Y\": 2 },{ \"HasColor\": true,\"X\": 6, \"Y\": 2 },{ \"HasColor\": true, \"X\": 7, \"Y\": 2 },{ \"HasColor\": false, \"X\": 8, \"Y\": 2 },{ \"HasColor\": false, \"X\": 9, \"Y\": 2 },{ \"HasColor\": false, \"X\": 10, \"Y\": 2 },{ \"HasColor\": true, \"X\": 11, \"Y\": 2 },{ \"HasColor\": false, \"X\": 12, \"Y\": 2 },{ \"HasColor\": false, \"X\": 13, \"Y\": 2 },{ \"HasColor\": false, \"X\": 14, \"Y\": 2 },{ \"HasColor\": false, \"X\": 15, \"Y\": 2 },{ \"HasColor\": true, \"X\": 16, \"Y\": 2 },{ \"HasColor\": true,\"X\": 17, \"Y\": 2 },{ \"HasColor\": false, \"X\": 18, \"Y\": 2 },{ \"HasColor\": true, \"X\": 0, \"Y\": 3 },{ \"HasColor\": false, \"X\": 1, \"Y\": 3 },{ \"HasColor\": false, \"X\": 2, \"Y\": 3 },{ \"HasColor\": true,\"X\": 3, \"Y\": 3 },{ \"HasColor\": false, \"X\": 4, \"Y\": 3 },{ \"HasColor\": true, \"X\": 5, \"Y\": 3 },{ \"HasColor\": false, \"X\": 6, \"Y\": 3 },{ \"HasColor\": false, \"X\": 7, \"Y\": 3 },{ \"HasColor\": true, \"X\": 8, \"Y\": 3 },{ \"HasColor\": false, \"X\": 9, \"Y\": 3 },{ \"HasColor\": true, \"X\": 10, \"Y\": 3 },{ \"HasColor\": false, \"X\": 11, \"Y\": 3 },{ \"HasColor\": false, \"X\": 12, \"Y\": 3 },{ \"HasColor\": false, \"X\": 13, \"Y\": 3 },{ \"HasColor\": false, \"X\": 14, \"Y\": 3 },{ \"HasColor\": false, \"X\": 15, \"Y\": 3 },{ \"HasColor\": false, \"X\": 16, \"Y\": 3 },{ \"HasColor\": false, \"X\": 17, \"Y\": 3 },{ \"HasColor\": true, \"X\": 18, \"Y\": 3 },{ \"HasColor\": true, \"X\": 0, \"Y\": 4 },{ \"HasColor\": false, \"X\": 1, \"Y\": 4 },{ \"HasColor\": false, \"X\": 2, \"Y\": 4 },{ \"HasColor\": true, \"X\": 3, \"Y\": 4 },{ \"HasColor\": false, \"X\": 4, \"Y\": 4 },{ \"HasColor\": true, \"X\": 5, \"Y\": 4 },{ \"HasColor\": true, \"X\": 6, \"Y\": 4 },{ \"HasColor\": true, \"X\": 7, \"Y\": 4 },{ \"HasColor\": false, \"X\": 8, \"Y\": 4 },{ \"HasColor\": true, \"X\": 9, \"Y\": 4 },{ \"HasColor\": false, \"X\": 10, \"Y\": 4 },{ \"HasColor\": false, \"X\": 11, \"Y\": 4 },{ \"HasColor\": false, \"X\": 12, \"Y\": 4 },{ \"HasColor\": false, \"X\": 13, \"Y\": 4 },{ \"HasColor\": false, \"X\": 14, \"Y\": 4 },{ \"HasColor\": true, \"X\": 15, \"Y\": 4 },{ \"HasColor\": true, \"X\": 16, \"Y\": 4 },{ \"HasColor\": true, \"X\": 17, \"Y\": 4 },{ \"HasColor\": false, \"X\": 18, \"Y\": 4 } ]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetKBPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"HasColor\": true,\"X\": 0,\"Y\": 0},{\"HasColor\": false,\"X\": 1,\"Y\": 0},{\"HasColor\": false,\"X\": 2,\"Y\": 0},{\"HasColor\": true,\"X\": 3,\"Y\": 0},{\"HasColor\": false,\"X\": 4,\"Y\": 0},{\"HasColor\": true,\"X\": 5,\"Y\": 0},{\"HasColor\": true,\"X\": 6,\"Y\": 0},{\"HasColor\": true,\"X\": 7,\"Y\": 0},{\"HasColor\": false,\"X\": 8,\"Y\": 0},{\"HasColor\": false,\"X\": 9,\"Y\": 0},{\"HasColor\": false,\"X\": 10,\"Y\": 0},{\"HasColor\": false,\"X\": 11,\"Y\": 0},{\"HasColor\": false,\"X\": 12,\"Y\": 0},{\"HasColor\": true,\"X\": 13,\"Y\": 0},{\"HasColor\": false,\"X\": 14,\"Y\": 0},{\"HasColor\": false,\"X\": 15,\"Y\": 0},{\"HasColor\": true,\"X\": 16,\"Y\": 0},{\"HasColor\": true,\"X\": 17,\"Y\": 0},{\"HasColor\": true,\"X\": 18,\"Y\": 0},{\"HasColor\": true,\"X\": 0,\"Y\": 1},{\"HasColor\": false,\"X\": 1,\"Y\": 1},{\"HasColor\": true,\"X\": 2,\"Y\": 1},{\"HasColor\": false,\"X\": 3,\"Y\": 1},{\"HasColor\": false,\"X\": 4,\"Y\": 1},{\"HasColor\": true,\"X\": 5,\"Y\": 1},{\"HasColor\": false,\"X\": 6,\"Y\": 1},{\"HasColor\": false,\"X\": 7,\"Y\": 1},{\"HasColor\": true,\"X\": 8,\"Y\": 1},{\"HasColor\": false,\"X\": 9,\"Y\": 1},{\"HasColor\": false,\"X\": 10,\"Y\": 1},{\"HasColor\": false,\"X\": 11,\"Y\": 1},{\"HasColor\": true,\"X\": 12,\"Y\": 1},{\"HasColor\": false,\"X\": 13,\"Y\": 1},{\"HasColor\": false,\"X\": 14,\"Y\": 1},{\"HasColor\": true,\"X\": 15,\"Y\": 1},{\"HasColor\": false,\"X\": 16,\"Y\": 1},{\"HasColor\": false,\"X\": 17,\"Y\": 1},{\"HasColor\": false,\"X\": 18,\"Y\": 1},{\"HasColor\": true,\"X\": 0,\"Y\": 2},{\"HasColor\": true,\"X\": 1,\"Y\": 2},{\"HasColor\": false,\"X\": 2,\"Y\": 2},{\"HasColor\": false,\"X\": 3,\"Y\": 2},{\"HasColor\": false,\"X\": 4,\"Y\": 2},{\"HasColor\": true,\"X\": 5,\"Y\": 2},{\"HasColor\": true,\"X\": 6,\"Y\": 2},{\"HasColor\": true,\"X\": 7,\"Y\": 2},{\"HasColor\": false,\"X\": 8,\"Y\": 2},{\"HasColor\": false,\"X\": 9,\"Y\": 2},{\"HasColor\": false,\"X\": 10,\"Y\": 2},{\"HasColor\": true,\"X\": 11,\"Y\": 2},{\"HasColor\": false,\"X\": 12,\"Y\": 2},{\"HasColor\": false,\"X\": 13,\"Y\": 2},{\"HasColor\": false,\"X\": 14,\"Y\": 2},{\"HasColor\": false,\"X\": 15,\"Y\": 2},{\"HasColor\": true,\"X\": 16,\"Y\": 2},{\"HasColor\": true,\"X\": 17,\"Y\": 2},{\"HasColor\": false,\"X\": 18,\"Y\": 2},{\"HasColor\": true,\"X\": 0,\"Y\": 3},{\"HasColor\": false,\"X\": 1,\"Y\": 3},{\"HasColor\": true,\"X\": 2,\"Y\": 3},{\"HasColor\": false,\"X\": 3,\"Y\": 3},{\"HasColor\": false,\"X\": 4,\"Y\": 3},{\"HasColor\": true,\"X\": 5,\"Y\": 3},{\"HasColor\": false,\"X\": 6,\"Y\": 3},{\"HasColor\": false,\"X\": 7,\"Y\": 3},{\"HasColor\": true,\"X\": 8,\"Y\": 3},{\"HasColor\": false,\"X\": 9,\"Y\": 3},{\"HasColor\": true,\"X\": 10,\"Y\": 3},{\"HasColor\": false,\"X\": 11,\"Y\": 3},{\"HasColor\": false,\"X\": 12,\"Y\": 3},{\"HasColor\": false,\"X\": 13,\"Y\": 3},{\"HasColor\": false,\"X\": 14,\"Y\": 3},{\"HasColor\": false,\"X\": 15,\"Y\": 3},{\"HasColor\": false,\"X\": 16,\"Y\": 3},{\"HasColor\": false,\"X\": 17,\"Y\": 3},{\"HasColor\": true,\"X\": 18,\"Y\": 3},{\"HasColor\": true,\"X\": 0,\"Y\": 4},{\"HasColor\": false,\"X\": 1,\"Y\": 4},{\"HasColor\": false,\"X\": 2,\"Y\": 4},{\"HasColor\": true,\"X\": 3,\"Y\": 4},{\"HasColor\": false,\"X\": 4,\"Y\": 4},{\"HasColor\": true,\"X\": 5,\"Y\": 4},{\"HasColor\": true,\"X\": 6,\"Y\": 4},{\"HasColor\": true,\"X\": 7,\"Y\": 4},{\"HasColor\": false,\"X\": 8,\"Y\": 4},{\"HasColor\": true,\"X\": 9,\"Y\": 4},{\"HasColor\": false,\"X\": 10,\"Y\": 4},{\"HasColor\": false,\"X\": 11,\"Y\": 4},{\"HasColor\": false,\"X\": 12,\"Y\": 4},{\"HasColor\": false,\"X\": 13,\"Y\": 4},{\"HasColor\": false,\"X\": 14,\"Y\": 4},{\"HasColor\": true,\"X\": 15,\"Y\": 4},{\"HasColor\": true,\"X\": 16,\"Y\": 4},{\"HasColor\": true,\"X\": 17,\"Y\": 4},{\"HasColor\": false,\"X\": 18,\"Y\": 4}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetUPPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":false},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":0,\"Y\":1,\"HasColor\":true},{\"X\":1,\"Y\":1,\"HasColor\":true},{\"X\":2,\"Y\":1,\"HasColor\":true},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":1,\"Y\":3,\"HasColor\":true},{\"X\":2,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":false}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetDownPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":false},{\"X\":1,\"Y\":0,\"HasColor\":true},{\"X\":2,\"Y\":0,\"HasColor\":false},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":1,\"Y\":1,\"HasColor\":true},{\"X\":2,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":false},{\"X\":1,\"Y\":2,\"HasColor\":true},{\"X\":2,\"Y\":2,\"HasColor\":false},{\"X\":0,\"Y\":3,\"HasColor\":true},{\"X\":1,\"Y\":3,\"HasColor\":true},{\"X\":2,\"Y\":3,\"HasColor\":true},{\"X\":0,\"Y\":4,\"HasColor\":false},{\"X\":1,\"Y\":4,\"HasColor\":true},{\"X\":2,\"Y\":4,\"HasColor\":false}]");

            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
        private static List<PointItem> GetfengexianPonitItems(Color color)
        {
            var pointinitlist = JsonConvert.DeserializeObject<List<PointInit>>("[{\"X\":0,\"Y\":0,\"HasColor\":true},{\"X\":0,\"Y\":1,\"HasColor\":false},{\"X\":0,\"Y\":2,\"HasColor\":true},{\"X\":0,\"Y\":3,\"HasColor\":false},{\"X\":0,\"Y\":4,\"HasColor\":true}]");
            return pointinitlist.Select(c => new PointItem(c.X, c.Y, c.HasColor ? color : (Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor))).ToList(); ;
        }
    }

    public enum ABCEnum
    {
        zero = 0,
        one = 1,
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
        lianjiexian,
        fengexian,
        GB,
        MB,
        KB,
         Up,
         Down



    }
}