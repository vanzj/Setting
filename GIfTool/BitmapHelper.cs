using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GIfTool
{
    public class BitmapHelper
    {
        public static System.Windows.Media.Color? GetPixelColor(Bitmap bitmapSource, int xindex, int yindex, double OneStep)
        {


            List<int> RList = new List<int>();
            List<int> GList = new List<int>();
            List<int> BList = new List<int>();
            for (int i = 5; i < (OneStep / 2); i++)
            {
                for (int j = 5; i < (OneStep / 2); i++)
                {
                    int pixelX = (int)(i + (OneStep * xindex));
                    int pixelY = (int)(j + (OneStep * yindex));
                    var OneStep1_4 = 5;
                    var temp1 = GetPixelColorAt(bitmapSource, pixelX, pixelY);

                    RList.Add(temp1.Value.R);
                    GList.Add(temp1.Value.G);
                    BList.Add(temp1.Value.B);
                }

            }
            var temp = System.Windows.Media.Color.FromArgb(Convert.ToByte(255),
       Convert.ToByte(RList.Average()),
       Convert.ToByte(GList.Average()),
       Convert.ToByte(BList.Average()));
            return temp;
        }
     
        public static System.Drawing.Color? GetPixelColorAt(Bitmap bitmapSource, int pixelX, int pixelY)
        {
            // 检查像素位置有效性
            if (pixelX < 0 || pixelX >= bitmapSource.Width ||
                pixelY < 0 || pixelY >= bitmapSource.Height)
            {
                return null;
            }
            return bitmapSource.GetPixel(pixelX, pixelY);

        }



    }
}

