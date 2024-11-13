using GalaSoft.MvvmLight.Messaging;
using Setting.Event;
using Setting.Helper;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Setting
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Cursor = CursorHelper.MOVE();
            Messenger.Default.Register<CursorModelChangeEvent>(this, HandleCursorModelChangeEvent);
            Messenger.Default.Register<SendStartEvent>(this, HandleSendStartEvent);
            Messenger.Default.Register<SendEndEvent>(this, HandleSendEndEvent);

       
            this.Login.Visibility = Visibility.Visible;

        }

        private void HandleSendEndEvent(SendEndEvent obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                Storyboard sb = (Storyboard)this.FindResource("SendStory");
                sb.Stop();
            });
         
        }

        private void HandleSendStartEvent(SendStartEvent obj)
        {
            Storyboard sb = (Storyboard)this.FindResource("SendStory");
            sb.Begin();
        }

        private void HandleCursorModelChangeEvent(CursorModelChangeEvent obj)
        {
            switch (obj.model)
            {
                case Enum.CursorEnum.MOVE:
                    this.Cursor = CursorHelper.MOVE();
                    break;
                case Enum.CursorEnum.ERASE:
                    this.Cursor = CursorHelper.ERASE();
                    break;
                case Enum.CursorEnum.Magic:
                    this.Cursor = CursorHelper.MAGIC();
                    break;
                default:
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
         SerialPortScanHelper.Instance.ClosePort();
            SerialPortHelper.Instance.ClosePort();
        }



        private void Theme_MouseLeave(object sender, MouseEventArgs e)
        {
            Messenger.Default.Send(new ThemeLotFocusEvent { });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //【最小化当前窗口】
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {//【还原 或者 最大化当前窗口】
            if (Application.Current.MainWindow.WindowState == WindowState.Normal)
            {
                //Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;//用这个，如果没有隐藏任务栏，就显示不全
                Application.Current.MainWindow.MaxHeight = SystemParameters.WorkArea.Height;
                Application.Current.MainWindow.WindowStyle = WindowStyle.None;
                Application.Current.MainWindow.ResizeMode = ResizeMode.NoResize;
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                return;
            }

            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.ResizeMode = ResizeMode.CanResizeWithGrip;
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                return;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("您确定要退出吗？", "退出提示", MessageBoxButton.OKCancel, MessageBoxImage.None, MessageBoxResult.Cancel);
            if (result == MessageBoxResult.OK)
            {
                //【关闭当前窗口】
                System.Environment.Exit(0);
            }
        }
 
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void ScreenList_MouseLeave(object sender, MouseEventArgs e)
        {
            Messenger.Default.Send(new ScreenLotFocusEvent { });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (this.degbug.Visibility == Visibility.Visible)
            {
                this.degbug.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.degbug.Visibility = Visibility.Visible;
            }
        }
        private void Button_Click_send(object sender, RoutedEventArgs e)
        {
           
            JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());

            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Messenger.Default.Send(new LoadedEvent { });

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Messenger.Default.Send(new KeyDownEvent { Key = e.Key});

            }
            else if (e.Key == Key.Enter)
            {
                Messenger.Default.Send(new KeyDownEvent { Key = e.Key });
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Console.WriteLine(e.NewValue);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Task.Run(() =>
            {
                JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());

                var result = jdClient.LogoutUsingPOSTAsync();
                // Restart current process Method 1

                this.Dispatcher.Invoke(() =>
                {
                    System.Windows.Forms.Application.Restart();
                    Application.Current.Shutdown();

                });



            });

        }



        private void Logoutgrip_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Logoutgrip.Visibility = Visibility.Collapsed;
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.Logoutgrip.Visibility = Visibility.Visible;
        }
    }

}
