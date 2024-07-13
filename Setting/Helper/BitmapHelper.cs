using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Setting.Helper
{
    public class BitmapHelper
    {
        public static Color GetPixelColor(BitmapSource bitmapSource, int xindex, int yindex, double OneStep)
        {
            int pixelX = (int)((OneStep / 2) + (OneStep * xindex));
            int pixelY = (int)((OneStep / 2) + (OneStep * yindex));
            return GetPixelColorAt(bitmapSource, pixelX, pixelY);
        }

        public static Color GetPixelColorAt(BitmapSource bitmapSource, int pixelX, int pixelY)
        {
            // 检查像素位置有效性
            if (pixelX < 0 || pixelX >= bitmapSource.PixelWidth ||
                pixelY < 0 || pixelY >= bitmapSource.PixelHeight)
            {
                throw new ArgumentOutOfRangeException("Pixel position is out of bounds.");
            }

            // 获取像素格式信息
            PixelFormat pixelFormat = bitmapSource.Format;

            // 计算行跨度
            int stride = bitmapSource.PixelWidth * ((pixelFormat.BitsPerPixel + 7) / 8);

            // 创建缓冲区以容纳一行像素数据
            byte[] pixelRow = new byte[stride];

            // 根据像素格式的不同，可能需要不同的处理方式
            Color color;
            if (pixelFormat == PixelFormats.Indexed8)
            {
                // 获取调色板
                var palette = bitmapSource.Palette;
                if (palette == null)
                {
                    throw new InvalidOperationException("Indexed8 format requires a valid palette.");
                }

                // 获取像素索引
                bitmapSource.CopyPixels(new Int32Rect(pixelX, pixelY, 1, 1), pixelRow, stride, 0);
                byte pixelIndex = pixelRow[0];
                color = palette.Colors[pixelIndex];
                return color;
            }


            //if (pixelFormat == PixelFormats.Bgra32 || pixelFormat == PixelFormats.Pbgra32)
            //{


            //    // 对于32位格式，直接获取像素并转换为Color
            //    int[] pixelData = new int[1];
            //    bitmapSource.CopyPixels(new Int32Rect(pixelX, pixelY, 1, 1), pixelData, 4, 0);
            //    color = Color.FromRgb(pixelData[0] & 0x00FF, (pixelData[0] >> 8) & 0x00FF, (pixelData[0] >> 16) & 0x00FF);
            //    return color;
            //}



            throw new NotSupportedException($"The pixel format '{pixelFormat}' is not supported.");
        }



    }
}

