using GIfTool;
using Newtonsoft.Json;
using Setting.Model.CMDModel;
using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
    public class MessageHelper
    {
      


        public static List<string> BuildOnePackage(Dictionary<int, List<PointItem>> AllPonitList, int xindex, int yIndex, string filename,int brightness,int frameRate)

        {
            var templist = new List<string>();
            //循环模式
            ThemeSend ThemeSend = new ThemeSend();
            ThemeSend.data = new ThemeData() { model = Const.CMDModelCirulate, name = filename };

            templist.Add(JsonConvert.SerializeObject(ThemeSend));
            //开始
            ThemeSendStartSend themeSendStartSend = new ThemeSendStartSend();
            themeSendStartSend.data = new ThemeSendStartData()
            {
                name = filename,
                count = "1",
                frameCount = AllPonitList.Count.ToString(),
                frameRate = frameRate.ToString(),
            };
          
            //帧
            ThemeSegmentSend themeSegmentSend = new ThemeSegmentSend();

            themeSegmentSend.data = new List<ThemeSegmentData>();

            var MaxbrightnessAverage = RGBToBrightNessHelper.Instance.maxA;


            var oneseg = new ThemeSegmentData()
            {
                name = filename,
                count = "1",
                frameCount = AllPonitList.Count.ToString(),
                frameRate = frameRate,
                index = "1",

            };
            oneseg.pointList = new List<ThemeSegmentDataPoint>() {  };

            for (int i = 0; i < AllPonitList.Count; i++)
            {

                var OnFrameAllPonitList = AllPonitList[i];
                OnFrameAllPonitList.Sort();

                var show = OnFrameAllPonitList.Where(c => c.X >= 0 && c.X < xindex && c.Y >= 0 && c.Y < yIndex);


                ThemeSegmentDataPoint oneframe = new ThemeSegmentDataPoint()
                {
                    frameRGB = show.Select(c => c.Fill.Color.ToString().Replace("#FF", "")).ToList(),
                    frameIndex = i.ToString()
                };

                var brightnessAverage = oneframe.frameRGB.Select(c => RGBToBrightNessHelper.Instance.Get(c)).Average();
                if (MaxbrightnessAverage > brightnessAverage)
                {
                    MaxbrightnessAverage = brightnessAverage;
                }
                oneseg.pointList.Add(oneframe);
           
            }
            oneseg.brightness = (int)(brightness * MaxbrightnessAverage);
            themeSendStartSend.data .brightness = (int)(brightness * MaxbrightnessAverage);
            themeSegmentSend.data.Add(oneseg);
            templist.Add(JsonConvert.SerializeObject(themeSendStartSend));
            templist.Add(JsonConvert.SerializeObject(themeSegmentSend));
            return templist;
        }
        public static List<string> BuildOnePackageGIFURL(List<ThemeSegmentData> data, int xindex, int yIndex, string filename,int frameRate)

        {
            var templist = new List<string>();
            //循环模式
            ThemeSend ThemeSend = new ThemeSend();
            ThemeSend.data = new ThemeData() { model = Const.CMDModelCirulate, name = filename };

            templist.Add(JsonConvert.SerializeObject(ThemeSend));
            //开始
            ThemeSendStartSend themeSendStartSend = new ThemeSendStartSend();
            themeSendStartSend.data = new ThemeSendStartData()
            {
                name = filename,
                count = "1",
                frameCount = data.Count.ToString(),
                frameRate = frameRate.ToString(),
                brightness = data.First().brightness,
            };
            templist.Add(JsonConvert.SerializeObject(themeSendStartSend));
            //帧
            ThemeSegmentSend themeSegmentSend = new ThemeSegmentSend();
            themeSegmentSend.data = data;
            templist.Add(JsonConvert.SerializeObject(themeSegmentSend));
            return templist;
        }

        public static List<string> BuildDynamic(Dictionary<int, List<PointItem>> AllPonitList, int xindex, int yIndex, string filename,int brightness, int frameRate=10)

        {

            var templist = new List<string>();

            for (int i = 0; i < AllPonitList.Count; i++)
            {

                var OnFrameAllPonitList = AllPonitList[i];
                OnFrameAllPonitList.Sort();

                var show = OnFrameAllPonitList.Where(c => c.X >= 0 && c.X < xindex && c.Y >= 0 && c.Y < yIndex);

                ThemeSegmentSend themeSegmentSend = new ThemeSegmentSend();
             
                   var oneseg= 
                    new ThemeSegmentData()
                {
                    name = filename,
                    count = AllPonitList.Count.ToString(),
                    frameCount = AllPonitList.Count.ToString(),
                    frameRate = frameRate,
                    brightness = brightness,
                    index = (i + 1).ToString(),

                };
                ThemeSegmentDataPoint oneframe = new ThemeSegmentDataPoint()
                {
                    frameRGB = show.Select(c => c.Fill.Color.ToString().Replace("#FF", "")).ToList(),
                    frameIndex = i.ToString()
                };
                oneseg.pointList = new List<ThemeSegmentDataPoint>() { oneframe };

                themeSegmentSend.data = new List<ThemeSegmentData>() { oneseg };
             templist.Add(JsonConvert.SerializeObject(themeSegmentSend));
            }
            return templist;
        }
        public static List<ThemeSegmentData> Buildgif(Dictionary<int, List<PointItem>> AllPonitList, int xindex, int yIndex, string filename, int frameRate)

        {
            var result = new List<ThemeSegmentData>();


            for (int i = 0; i < AllPonitList.Count; i++)
            {

                var OnFrameAllPonitList = AllPonitList[i];
                OnFrameAllPonitList.Sort();

                var show = OnFrameAllPonitList.Where(c => c.X >= 0 && c.X < xindex && c.Y >= 0 && c.Y < yIndex);

                ThemeSegmentSend themeSegmentSend = new ThemeSegmentSend();

                var oneseg = new ThemeSegmentData()
                {
                    name = filename,
                    count = AllPonitList.Count.ToString(),
                    frameCount = AllPonitList.Count.ToString(),
                    frameRate = frameRate,
                    index = (i + 1).ToString(),

                };
                ThemeSegmentDataPoint oneframe = new ThemeSegmentDataPoint()
                {
                    frameRGB = show.Select(c => c.Fill.Color.ToString().Substring(3,6)).ToList(),
                    frameIndex = i.ToString()
                };
                oneseg.pointList = new List<ThemeSegmentDataPoint>() { oneframe };

                result.Add(oneseg);


            }
            return result;
        }


    }

    public   class MessageItem
    {
        public string cmd { get; set; }
        public string date { get; set; }
    }

    public class MessageItemFrame
    {
        public string cmd { get; set; }
        public List<string> date { get; set; }
    }
    public class StratInfo
    {
        public int FrameCount { get; set; }
        public int FrameRate { get; set; }
    }

    public class MessageConst
    {
        public static  string Open { get; set; } = "Open";
        public static string Close { get; set; } = "Close";
        public static string Luminance { get; set; } = "luminance";
        public static string Model { get; set; } = "Model";


        public static string Start { get; set; } = "Start";
        public static string OneFrame { get; set; } = "OneFrame";
        public static string End { get; set; } = "End";
    }
}
