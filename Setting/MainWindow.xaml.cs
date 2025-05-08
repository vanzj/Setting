using AutoUpdaterDotNET;
using GalaSoft.MvvmLight.Messaging;
using HidSharp;
using Setting.Event;
using Setting.Event.MsgSendEvent;
using Setting.Helper;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
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
using Windows.Media.Protection.PlayReady;
using static DotNetty.Common.ThreadLocalPool;


namespace Setting
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isDebug = false;
        public MainWindow()
        {
            InitializeComponent();
            this.Cursor = CursorHelper.MOVE();
            Messenger.Default.Register<CursorModelChangeEvent>(this, HandleCursorModelChangeEvent);
            Messenger.Default.Register<SendStartStoryEvent>(this, HandleSendStartStoryEvent);
            Messenger.Default.Register<SendEndStoryEvent>(this, HandleSendEndStoryEvent);
            Messenger.Default.Register<SendNetWorkEvent>(this, HandleSendNetWorkEvent);
            RGBToBrightNessHelper.Instance.Init();
       
            this.Login.Visibility = Visibility.Visible;
          var isdebugConfig =   ConfigurationManager.AppSettings["IsDebug"] ?? "0";
            isDebug   = !(isdebugConfig == "0");
        }

        private void HandleSendNetWorkEvent(SendNetWorkEvent @event)
        {
            this.Dispatcher.Invoke(() =>
            {
                Storyboard sb = (Storyboard)this.FindResource("SendNetworkStory");
                sb.Begin();

            });
        }

        private void HandleSendEndStoryEvent(SendEndStoryEvent obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                Storyboard sb = (Storyboard)this.FindResource("SendStory");
                sb.Stop();
            });
         
        }

        private void HandleSendStartStoryEvent(SendStartStoryEvent obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                Storyboard sb = (Storyboard)this.FindResource("SendStory");
                sb.Begin();

            });
        
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
            SerialPortSendMsgHelper.Instance.CloseAllPort();
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
            if (isDebug)
            {
                this.debugClear.Visibility = Visibility.Visible;
                this.debugClick.Visibility = Visibility.Visible;
            }
            AutoUpdater.ExecutablePath = "Setup.msi";
            AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
            var tcp =   TcpDefaultHelper.Instance;
            Messenger.Default.Send(new LoadedEvent { });

            this.KeyDown += new KeyEventHandler(MainWindow_KeyDown);

            JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            try
            {
                AutoUpdater.Start("https://smart.9jodia.net/holocubic/dot/1/updaterStart.xml");

            }
            catch (Exception)
            {

                
            }
            
        }

        private void AutoUpdater_ApplicationExitEvent()
        {
            Thread.Sleep(1000);
            System.Environment.Exit(0);
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

                LoginInfoHelper.DisableAutoLogin();

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

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

        }

        private void VersionCheck_Click(object sender, RoutedEventArgs e)
        {
            JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            try
            {
                AutoUpdater.Start("https://smart.9jodia.net/holocubic/dot/1/updaterStart.xml");
                if (isfirstCheckUpater )
                    {
                        isfirstCheckUpater = false;//防止重复绑定事件。
                        AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
                    }
            }
            catch (Exception)
            {


            }
        }
        bool isfirstCheckUpater = true;

    
private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {
                    MessageBoxResult result;
                     result = MessageBox.Show("有新的版本需要更新？", "更新提示", MessageBoxButton.OKCancel, MessageBoxImage.None, MessageBoxResult.Cancel);

                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            if (AutoUpdater.DownloadUpdate(args))
                            {
                                System.Environment.Exit(0);
                            }
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButton.OKCancel, MessageBoxImage.None, MessageBoxResult.Cancel);
                        }
                    }
                    
                }
                else
                {
                    MessageBox.Show("无版本更新，请稍后重试", "无版本更新", MessageBoxButton.OKCancel, MessageBoxImage.None, MessageBoxResult.Cancel);
                }
            }
            else
            {
                MessageBox.Show("更新异常，请稍后重试", "更新异常", MessageBoxButton.OKCancel, MessageBoxImage.None, MessageBoxResult.Cancel);

            }
        }

        private void Slider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.OldValue != e.NewValue)
            {
                Messenger.Default.Send(new FrameChangeEvent { CurrentFrame = (int)e.NewValue });
            }
        }

        private void Grid_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Messenger.Default.Send(new MsgSendCloseEvent ());
            
        }

      
    }

}
