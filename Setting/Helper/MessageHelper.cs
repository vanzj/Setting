using Newtonsoft.Json;
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

        public static List<string> Build(Dictionary<int, List<PointItem>> AllPonitList,int xindex,int yIndex)

        {
            var templist = new List<string>();
            //循环模式
            MessageItem msg = new MessageItem()
            {
                cmd = MessageConst.Model,
                date =MessageConst.Cirulate,
            };
            templist.Add(JsonConvert.SerializeObject(msg));
            //开始
            StratInfo stratInfo = new StratInfo()
            {
                FrameCount = AllPonitList.Count,
                FrameRate = 30,
            };

            MessageItem msg1 = new MessageItem()
            {
                cmd = MessageConst.Start,
                date = JsonConvert.SerializeObject(stratInfo)
            };
            //帧
            templist.Add(JsonConvert.SerializeObject(msg1));
            for (int i = 0; i < AllPonitList.Count; i++)
            {

                var OnFrameAllPonitList = AllPonitList[i];
                OnFrameAllPonitList.Sort();
                
               var show = OnFrameAllPonitList .Where(c => c.X >= 0 && c.X < xindex && c.Y >= 0 && c.Y < yIndex);

                MessageItemFrame msg2 = new MessageItemFrame()
                {
                    cmd = MessageConst.OneFrame,
                    date = show.Select(c => c.Fill.Color.ToString().Replace("#FF", "")).ToList()
                };
                templist.Add(JsonConvert.SerializeObject(msg2));
            }
            MessageItem msg3 = new MessageItem()
            {
                cmd = MessageConst.End,
                date = ""
            };
            return templist;
        }

        public List<string> BuildOpen()

        {
            var templist = new List<string>();

            MessageItem msg = new MessageItem()
            {
                cmd = MessageConst.Open,
                date = ""
            };
            templist.Add(JsonConvert.SerializeObject(msg));
            return templist;

        }
        public List<string> BuildClose()

        {
            var templist = new List<string>();

            MessageItem msg = new MessageItem()
            {
                cmd = MessageConst.Close,
                date = ""
            };
            templist.Add(JsonConvert.SerializeObject(msg));

            return templist;

        }
        public List<string> BuildLuminance(int lumminance)

        {
            var templist = new List<string>();

            MessageItem msg = new MessageItem()
            {
                cmd = MessageConst.Luminance,
                date = lumminance.ToString()
            };
            templist.Add(JsonConvert.SerializeObject(msg));

            return templist;

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
        public static string Luminance { get; set; } = "Luminance";
        public static string Model { get; set; } = "Model";
        public static string Dynamic { get; set; } = "Dynamic";
        public static string Cirulate { get; set; } = "Cirulate";

        public static string Start { get; set; } = "Start";
        public static string OneFrame { get; set; } = "OneFrame";
        public static string End { get; set; } = "End";
    }
}
