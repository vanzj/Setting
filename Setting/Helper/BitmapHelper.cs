//using System;
//using System.Drawing;
//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;

//namespace Setting.Helper
//{
//    public class BitmapHelper
//    {
//        public static System.Windows.Media.Color? GetPixelColor(Bitmap bitmapSource, int xindex, int yindex, double OneStep)
//        {
//            int pixelX = (int)((OneStep / 2) + (OneStep * xindex));
//            int pixelY = (int)((OneStep / 2) + (OneStep * yindex));
//            var temp =   GetPixelColorAt(bitmapSource, pixelX, pixelY);
//            if (temp ==null)
//            {
//                return null;
//            }
//            return System.Windows.Media.Color.FromArgb(temp.Value.A, temp.Value.R, temp.Value.G, temp.Value.B);
//        }

//        public static System.Drawing.Color? GetPixelColorAt(Bitmap bitmapSource, int pixelX, int pixelY)
//        {
//            // 检查像素位置有效性
//            if (pixelX < 0 || pixelX >= bitmapSource.Width ||
//                pixelY < 0 || pixelY >= bitmapSource.Height)
//            {
//                return null;
//            }
//        return     bitmapSource.GetPixel(pixelX, pixelY);
           
//        }



//    }
//}

