using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Windows.Media;

namespace GIfTool
{
    public class InputGIF
    {

        public List<ThemeSegmentData> OPENGIF(string filePath,int xIndex,int yIndex,string filename)
        {
            var result = new List<ThemeSegmentData>();
            System.Drawing.Image imgGif = System.Drawing.Image.FromFile(filePath); // 获取图片对象

    
            if (ImageAnimator.CanAnimate(imgGif))
            {
                FrameDimension imgFrmDim = new FrameDimension(imgGif.FrameDimensionsList[0]);
              var  FramesCount = imgGif.GetFrameCount(imgFrmDim); // 获取帧数

               
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
                    var firstFrameAllPonitList = new List<ThemeSegmentDataPoint>();
                    var oneseg = new ThemeSegmentData()
                    {
                        name = filename,
                        count = FramesCount.ToString(),
                        frameCount = FramesCount.ToString(),
                        frameRate = 30,
                        brightness = 255,
                        index = (i + 1).ToString(),

                    };

                    oneseg.pointList = firstFrameAllPonitList;

                    var OneFrame = new ThemeSegmentDataPoint();

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
                                OneFrame.frameRGB.Add(ColorCon .Replace("#FF", ""));
                            }


                        }
                    }
             
                    result.Add(oneseg);
                }

            }

        }


        
            }

        //public void cloneGIF()
        //{
        //    int width = 128;
        //    int height = width;
        //    int stride = width / 8;
        //    byte[] pixels = new byte[height * stride];

        //    Define the image palette
        //    BitmapPalette myPalette = BitmapPalettes.WebPalette;

        //    Creates a new empty image with the pre-defined palette

        //   BitmapSource image = BitmapSource.Create(
        //       width,
        //       height,
        //       96,
        //       96,
        //       PixelFormats.Indexed1,
        //       myPalette,
        //       pixels,
        //       stride);

        //    FileStream stream = new FileStream("new.gif", FileMode.Create);
        //    GifBitmapEncoder encoder = new GifBitmapEncoder();

        //    encoder.Frames.Add(BitmapFrame.Create(image));
        //    encoder.Save(stream);
        //}
    }

public class ColorConst
{
    public static string BackGroupColor => "#FFFFFFFF";
    public static string AbcColor => "#FF234EDE";

}
}
