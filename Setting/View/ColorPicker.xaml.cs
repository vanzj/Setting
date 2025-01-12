
using CommonServiceLocator;
using Setting.Media;
using Setting.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Setting.View
{

    /// <summary>
    /// ColorPicker.xaml 的交互逻辑
    /// </summary>
    public partial class ColorPicker : UserControl, INotifyPropertyChanged
    {

        public PointListViewModel PointList
        {
            get
            {
                var a = ServiceLocator.Current.GetInstance<PointListViewModel>();
                return a;
            }
        }

        public ColorPicker()
        {
            InitializeComponent();
        }

        double H = 0;
        double S = 1;
        double B = 1;



        private void thumbHChanged(double xpercent, double ypercent)
        {
            H = 360 * ypercent;
            HsbaColor Hcolor = new HsbaColor(H, 1, 1, 1);
            viewSelectColor.Fill = Hcolor.SolidColorBrush;

            Hcolor = new HsbaColor(H, S, B, 1);
            PointList.ChangeColor = Hcolor.SolidColorBrush;

            ColorChange(Hcolor.RgbaColor);
        }

        private void  thumbSBChanged(double xpercent, double ypercent)
        {
            S = xpercent;
            B = 1 - ypercent;
            HsbaColor Hcolor = new HsbaColor(H, S, B, 1);

            PointList.ChangeColor = Hcolor.SolidColorBrush;

            ColorChange(Hcolor.RgbaColor);
        }

        // 依赖属性字段
        public static readonly DependencyProperty SelectColorProperty =
           DependencyProperty.Register(
               "SelectColor",
               typeof(SolidColorBrush),
               typeof(ColorPicker),
               new PropertyMetadata(new SolidColorBrush(Colors.Black)));





        int R = 255;
        int G = 255;
        int _B = 255;
        int A = 255;

        public event PropertyChangedEventHandler PropertyChanged;



     

        private bool isColorChange = false;
        private void ColorChange(RgbaColor Hcolor)
        {
            isColorChange = true;
      

            TextHex.Text = Hcolor.HexString;
            isColorChange = false;
        }



        private void TextHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isColorChange)
            {
                return;
            }
            string pattern = @"^[0-9A-Fa-f]{6}$";
            string input = TextHex.Text;
            bool isMatch = Regex.IsMatch(input, pattern);
            if (isMatch)
            {

                RgbaColor Hcolor = new RgbaColor("#FF" + TextHex.Text);

                HsbaColor hsbaColor = new HsbaColor(Hcolor.R, Hcolor.G, Hcolor.B);
                H = hsbaColor.H;
                S = hsbaColor.S;
                B = hsbaColor.B;

                this.thumbH.Top = thumbH.ActualHeight * (H / 360) - thumbH.Yoffset;
                this.thumbSB.Top = thumbSB.ActualHeight * (1-B) - thumbSB.Yoffset;
                this.thumbSB.Left = thumbSB.ActualWidth * (S) - thumbSB.Xoffset;
           


                    var newH = 360 * this.thumbH.Ypercent;
                    HsbaColor Hcolornew = new HsbaColor(newH, 1, 1, 1);
                    viewSelectColor.Fill = Hcolornew.SolidColorBrush;
                
                PointList.ChangeColor = Hcolor.SolidColorBrush;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TextHex.Text = "000000";
            TextHex.Text = "FFFFFF";
        }
    }

    /// <summary>
    /// 封装Canvas 到Thumb来简化 Thumb的使用，关注熟悉X,Y 表示 thumb在坐标中距离左，上的距离
    /// 默认canvas 里用一个小圆点来表示当前位置
    /// </summary>
    public class ThumbPro : Thumb
    {
        //距离Canvas的Top,模板中需要Canvas.Top 绑定此Top
        public double Top
        {
            get { return (double)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Top.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TopProperty =
            DependencyProperty.Register("Top", typeof(double), typeof(ThumbPro), new PropertyMetadata(0.0));


        //距离Canvas的Top,模板中需要Canvas.Left 绑定此Left
        public double Left
        {
            get { return (double)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LeftProperty =
            DependencyProperty.Register("Left", typeof(double), typeof(ThumbPro), new PropertyMetadata(0.0));

        double FirstTop;
        double FirstLeft;

        //小圆点的半径
        public double Xoffset { get; set; }
        public double Yoffset { get; set; }

        public bool VerticalOnly { get; set; } = false;

        public double Xpercent { get { return (Left + Xoffset) / ActualWidth; } }
        public double Ypercent { get { return (Top + Yoffset) / ActualHeight; } }

        public void SetTopLeftByPercent(double xpercent, double ypercent)
        {
            Top = ypercent * ActualHeight - Yoffset;
            if (!VerticalOnly)
                Left = xpercent * ActualWidth - Xoffset;
        }

        public event Action<double, double> ValueChanged;

        public ThumbPro()
        {
            Loaded += (object sender, RoutedEventArgs e) => {
                if (!VerticalOnly)
                    Left = -Xoffset;
                Top = -Yoffset;


            };
            DragStarted += (object sender, DragStartedEventArgs e) =>
            {
                //当随便点击某点，把小远点移到当前位置，注意是小远点的中心位置移到当前位置
                if (!VerticalOnly)
                {
                    Left = e.HorizontalOffset - Xoffset;
                    FirstLeft = Left;
                }
                Top = e.VerticalOffset - Yoffset;
                FirstTop = Top;

                ValueChanged?.Invoke(Xpercent, Ypercent);
            };

            DragDelta += (object sender, DragDeltaEventArgs e) =>
            {
                //按住拖拽时，小远点随着鼠标移动
                if (!VerticalOnly)
                {
                    double x = FirstLeft + e.HorizontalChange;

                    if (x < -Xoffset) Left = -Xoffset;
                    else if (x > ActualWidth - Xoffset) Left = ActualWidth - Xoffset;
                    else Left = x;
                }




                double y = FirstTop + e.VerticalChange;

                if (y < -Yoffset) Top = -Yoffset;
                else if (y > ActualHeight - Yoffset) Top = ActualHeight - Yoffset;
                else Top = y;
                ValueChanged?.Invoke(Xpercent, Ypercent);
            };
        }
    }
}
