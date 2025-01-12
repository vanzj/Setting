using Setting.Media;
using System.Windows.Media;

namespace Setting.Media
{
    public class RgbaColor
    {
        int r = 0, g = 0, b = 0, a = 0;
        /// <summary>
        /// 0 - 255
        /// </summary>
        public int R { get { return r; } set { r = value < 0 ? 0 : value > 255 ? 255 : value; } }
        /// <summary>
        /// 0 - 255
        /// </summary>
        public int G { get { return g; } set { g = value < 0 ? 0 : value > 255 ? 255 : value; } }
        /// <summary>
        /// 0 - 255
        /// </summary>
        public int B { get { return b; } set { b = value < 0 ? 0 : value > 255 ? 255 : value; } }
        /// <summary>
        /// 0 - 255
        /// </summary>
        public int A { get { return 255; } }
        /// <summary>
        /// 亮度 0 - 100
        /// </summary>
        public int Y { get { return Utility.GetBrightness(R, G, B); } }

        public RgbaColor() { R = 255; G = 255; B = 255;  }
        public RgbaColor(int r, int g, int b, int a = 255) { R = r; G = g; B = b; }
        public RgbaColor(Brush brush)
        {
            if (brush != null)
            {
                R = ((SolidColorBrush)brush).Color.R;
                G = ((SolidColorBrush)brush).Color.G;
                B = ((SolidColorBrush)brush).Color.B;
            }
            else
            {
                R = G = B  = 255;
            }
        }
        public RgbaColor(double h, double s, double b, double a = 1)
        {
            RgbaColor rgba = Utility.HsbaToRgba(new HsbaColor(h, s, b, a));
            R = rgba.R;
            G = rgba.G;
            B = rgba.B;
           

        }
        public RgbaColor(string hexColor)
        {
            try
            {
                Color color;
                if (hexColor.Substring(0, 1) == "#") color = (Color)ColorConverter.ConvertFromString(hexColor);
                else color = (Color)ColorConverter.ConvertFromString("#" + hexColor);
                R = color.R;
                G = color.G;
                B = color.B;
         
            }
            catch
            {

            }
        }

        public Color Color { get { return Color.FromArgb((byte)A, (byte)R, (byte)G, (byte)B); } }
        public Color OpaqueColor { get { return Color.FromArgb((byte)255.0, (byte)R, (byte)G, (byte)B); } }
        public SolidColorBrush SolidColorBrush { get { return new SolidColorBrush(Color); } }
        public SolidColorBrush OpaqueSolidColorBrush { get { return new SolidColorBrush(OpaqueColor); } }

        public string HexString { get { return Color.ToString().Replace("#FF",""); } }
        public string RgbaString { get { return R + "," + G + "," + B + "," + A; } }

        public HsbaColor HsbaColor { get { return Utility.RgbaToHsba(this); } }
    }
}