using AnimatedGif;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GIfTool
{
    public class OutPutGIF
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="themeJsonInfo"></param>
        /// <param name="xindex"></param>
        /// <param name="yindex"></param>
        /// <param name="scale"></param>
        /// <param name="FilePath">完整路径待gif</param>
        public void Output(List<ThemeSegmentData> themeJsonInfo, int xindex, int yindex, int scale,string FilePath)
        {
            GifBitmapEncoder gEnc = new GifBitmapEncoder();


            List<BitmapFrame> bitmaps = new List<BitmapFrame>();
            foreach (var ThemeSegmentData in themeJsonInfo)
            {
                int index = 0;
                Bitmap bmpImage = new System.Drawing.Bitmap(xindex * scale, yindex * scale);
              
                foreach (var rgb in ThemeSegmentData.pointList[0].frameRGB)
                {

                    setPeixs(bmpImage, index, xindex, yindex, scale, rgb);
                    index++;
                }
                var bmp = bmpImage.GetHbitmap();
                var src = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp,IntPtr.Zero,Int32Rect.Empty,BitmapSizeOptions.FromEmptyOptions());
                var temp = BitmapFrame.Create(src);
                bitmaps.Add(temp);
            }

            foreach (var bitmap in bitmaps)
            {
                gEnc.Frames.Add(bitmap);
            }
            using (FileStream fs = new FileStream(FilePath, FileMode.Create))
            {
                gEnc.Save(fs);
            }
        }


        private void setPeixs(Bitmap bitmap, int index, int xindex, int yindex, int scale, string rgb)
        {
            var px = index % xindex;
            var py = index / xindex;
            int r = Convert.ToInt32(rgb.Substring(0, 2), 16);
            int g = Convert.ToInt32(rgb.Substring(2, 2), 16);
            int b = Convert.ToInt32(rgb.Substring(4, 2), 16);
            for (int i = 0; i < scale; i++)
            {
                for (int j = 0; j < scale; j++)
                {
                    if (i ==0 || j==0||j==scale-1||i==scale-1)
                    {
                        bitmap.SetPixel(px * scale + i, py * scale + j, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        bitmap.SetPixel(px * scale + i, py * scale + j, Color.FromArgb(r, g, b));
                    }
     
                }
            }

        }
    }
}
