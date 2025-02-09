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
using System.Windows.Media.Imaging;
using Setting.Helper;
using System.Linq;

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
            var frameRate = GetFrameRate(filePath);
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
                        frameRate = frameRate,
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
            else
            {
                Stream imageStreamSource = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                GifBitmapDecoder decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                var FramesCount = decoder.Frames.Count; // 获取帧数



                for (int i = 0; i < FramesCount; i++)
                {

                    Bitmap t = BitmapHelper.BitmapFromSource(decoder.Frames[i]);
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
                        frameRate = frameRate,
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
                imageStreamSource.Close();
                return result;
            }
            return result;
        }



        public  int GetFrameRate(string gifFilePath)
        {
            using (Image gif = Image.FromFile(gifFilePath))
            {
                // 获取GIF的第一帧
                var frameDimensions = new FrameDimension(gif.FrameDimensionsList[0]);
                gif.SelectActiveFrame(frameDimensions, 0);

                // 获取帧的延迟时间（单位为0.1秒）
                var delayTime = gif.GetPropertyItem(0x5100).Value[0];
                var FrameRate = delayTime == 0 ? 0 : (int)(100 / delayTime);
                if (FrameRate>30)
                {
                    FrameRate = 30;
                }
                if (FrameRate == 0)
                {
                    FrameRate = 20;
                }
                // GIF的帧率是100 / 延迟时间，但如果延迟时间为0则不是有效的GIF
                return FrameRate;
            }
        }
    


        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public List<ThemeSegmentData> OPENGIFURL(string imageUrl, int xIndex, int yIndex, string filename, int brightness,int frameRate)
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
                        var MaxbrightnessAverage = RGBToBrightNessHelper.Instance.maxA;
                        var oneseg = new ThemeSegmentData()
                        {
                            name = filename,
                            count = "1",
                            frameCount = FramesCount.ToString(),
                            frameRate = frameRate,
                            brightness = brightness,
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
                            var brightnessAverage = OneFrame.frameRGB.Select(c => RGBToBrightNessHelper.Instance.Get(c)).Average();
                            if (MaxbrightnessAverage < brightnessAverage)
                            {
                                MaxbrightnessAverage = brightnessAverage;
                            }
                            oneseg.pointList.Add(OneFrame);

                        }
                        oneseg.brightness = (int)(brightness * MaxbrightnessAverage);
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

