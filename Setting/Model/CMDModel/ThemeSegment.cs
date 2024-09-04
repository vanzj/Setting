using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Model.CMDModel
{

    public class ThemeSegmentSend : SendBase<List<ThemeSegmentData>>
    {
        public ThemeSegmentSend()
        {
            cmd = "themeSegment";
        }
    }

    public class ThemeSegmentData
    {
        /// <summary>
        /// 主题 的guid
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 亮度0-255
        /// </summary>
        public int brightness { get; set; }
       /// <summary>
       /// 帧率
       /// </summary>
        public int frameRate { get; set; }
        /// <summary>
        /// 帧数 
        /// </summary>
        public string frameCount { get; set; }
        /// <summary>
        /// 分片数
        /// </summary>
        public string count { get; set; }
        /// <summary>
        /// 分片计数 1 开始
        /// </summary>
        public string index { get; set; }

        public List<ThemeSegmentDataPoint> pointList { get; set; }
    }

    public class ThemeSegmentDataPoint
    {
        public string frameIndex { get; set; }
        public List<string> frameRGB { get; set; }
    }
    public class ThemeSegmentRetrun : ReturnBase<string>
    {
        public ThemeSegmentRetrun()
        {
            origincmd = "themeSegment";
        }
    }

}
