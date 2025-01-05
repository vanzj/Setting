using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Configuration;
using System.Net.Http;

namespace GIfTool
{
    public class InputGIF
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<ThemeSegmentData> OPENGIF(string filePath, int xIndex, int yIndex, string filename)
        {
            var result = new List<ThemeSegmentData>();
            System.Drawing.Image imgGif = System.Drawing.Image.FromFile(filePath); // 获取图片对象
            if (ImageAnimator.CanAnimate(imgGif))
            {
                FrameDimension imgFrmDim = new FrameDimension(imgGif.FrameDimensionsList[0]);
                var FramesCount = imgGif.GetFrameCount(imgFrmDim); // 获取帧数


                for (int i = 0; i < FramesCount; i++)
                {
                    // 把每一帧保存为jpg图片
                    imgGif.SelectActiveFrame(imgFrmDim, i);
                    Bitmap t = new Bitmap(imgGif);
                    var tmp = t.GetPixel(1, 1);
                    var pw = t.Width;
                    var ph = t.Height;

                    var phdouble = (double)ph;
                    var xindexdouble = (double)yIndex;
                    var CurrentImgxIndex = (int)(pw / (ph / xindexdouble));
                    var OneStep = phdouble / yIndex;
                
                    var oneseg = new ThemeSegmentData()
                    {
                        name = filename,
                        count = FramesCount.ToString(),
                        frameCount = FramesCount.ToString(),
                        frameRate = 20,
                        brightness = 255,
                        index = (i + 1).ToString(),

                    };

                   

                    var OneFrame = new ThemeSegmentDataPoint();
                    OneFrame.frameRGB = new List<string>();
                    OneFrame.frameIndex = i.ToString();
                    for (int y = 0; y < yIndex; y++)
                    {
                        for (int x = 0; x < xIndex; x++)
                        {
                            if (x < CurrentImgxIndex)
                            {
                                var tempcolor = BitmapHelper.GetPixelColor(t, x, y, OneStep);

                                OneFrame.frameRGB.Add(tempcolor.Value.ToString().Replace("#FF", ""));
                            }
                            else
                            {
           
                                if (true)
                                {
                                    OneFrame.frameRGB.Add(ColorConst.BackGroupColor.Replace("#FF", ""));
                                }
                                else
                                {
                                    var tempcolor = BitmapHelper.GetPixelColor(t, x - CurrentImgxIndex, y, OneStep);
                                    OneFrame.frameRGB.Add(tempcolor.Value.ToString().Replace("#FF", ""));
                                }
                            }
                        }
                    }
                    oneseg.pointList = new List<ThemeSegmentDataPoint>() { OneFrame };
                    result.Add(oneseg);
                }

            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<ThemeSegmentData> OPENGIFURL(string imageUrl, int xIndex, int yIndex, string filename)
        {
         
            var result = new List<ThemeSegmentData>();
            using (HttpClient client = new HttpClient())
            using (var response =  client.GetAsync(imageUrl).GetAwaiter().GetResult())
            using (var stream =  response.Content.ReadAsStreamAsync().GetAwaiter().GetResult())
            {
                if (response.IsSuccessStatusCode)
                {

                    System.Drawing.Image imgGif = Image.FromStream(stream); // 获取图片对象
                    if (ImageAnimator.CanAnimate(imgGif))
                    {
                        FrameDimension imgFrmDim = new FrameDimension(imgGif.FrameDimensionsList[0]);
                        var FramesCount = imgGif.GetFrameCount(imgFrmDim); // 获取帧数
                        if (FramesCount>110)
                        {
                            throw new Exception("内容太长了");
                        }
                        var oneseg = new ThemeSegmentData()
                        {
                            name = filename,
                            count = "1",
                            frameCount = FramesCount.ToString(),
                            frameRate = 6,
                            brightness = 255,
                            index = "1",
                        };
                        oneseg.pointList = new List<ThemeSegmentDataPoint>() {  };
                        for (int i = 0; i < FramesCount; i++)
                        {
                            // 把每一帧保存为jpg图片
                            imgGif.SelectActiveFrame(imgFrmDim,i==0?0:i);
                            Bitmap t = new Bitmap(imgGif);
                            var pw = t.Width;
                            var ph = t.Height;
                            
                            var OneFrame = new ThemeSegmentDataPoint();
                            OneFrame.frameRGB = new List<string>();
                            OneFrame.frameIndex = i.ToString();
                            for (int y = 0; y < yIndex; y++)
                            {
                                for (int x = 0; x < xIndex; x++)
                                {
                                    if (x < pw)
                                    {
                                        var tempcolor = BitmapHelper.GetPixelMediaColorAt(t, x, y);
                                        OneFrame.frameRGB.Add(tempcolor.Value.ToString().Replace("#FF", ""));
                                    }
                                    else
                                    {
                                            OneFrame.frameRGB.Add(ColorConst.BackGroupColor.Replace("#FF", ""));
                                    }
                                }
                            }
                            oneseg.pointList.Add(OneFrame);

                        }
                        result.Add(oneseg);
                    }
                }
            }
            return result;
        }



    }


}

public class ColorConst
{
    public static string BackGroupColor => ConfigurationManager.AppSettings["Background"] ?? "#FF999999";
    public static string AbcColor => ConfigurationManager.AppSettings["Foreground"] ?? "#FF234EDE";

}

