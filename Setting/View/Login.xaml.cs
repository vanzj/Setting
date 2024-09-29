using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Event;
using Setting.Helper;
using Setting.Model.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Webapi;

namespace Setting.View
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : UserControl
    {
        private string MsgImgUUId;
        public Login()
        {
            InitializeComponent();

            Messenger.Default.Register<InitEndEvent>(this, HandleInitEndEvent);
            this.PWDGrid.Visibility = Visibility.Visible;
            this.MsgCodeGrid.Visibility = Visibility.Collapsed;
            this.Loading.Visibility = Visibility.Collapsed;
            this.RegGird.Visibility = Visibility.Collapsed;
        }

        private void HandleInitEndEvent(InitEndEvent obj)
        {
            if (obj.endName== "init")
            {
                this.init = false;
            }
            else if (obj.endName == "findScreen")
            {
                this.findScreen = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            RegisterReq req = new RegisterReq()
            {
                Mobile = this.mobile.Text,
                MsgCode = this.msgCode.Text,
                Password = this.password.Text,
                RePassword = this.Repassword.Text,

            };
            var msg = client.RegisterUsingPOSTAsync(req).GetAwaiter().GetResult();
        }

        private void msgCodeimg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.msgCodeimg.Source = GetMsgCodeImg2();
            this.msgCodemsgCodeimg .Source = GetMsgCodeImg2();
        }


        private BitmapImage GetMsgCodeImg2()
        {
            MsgImgUUId = Guid.NewGuid().ToString();
            BitmapImage bitImage = new BitmapImage();
           bitImage.BeginInit();
           bitImage.UriSource = new Uri($"https://testsmart.9jodia.net/smart/api/dotpc/captcha?uuid={MsgImgUUId}", UriKind.Absolute);
           bitImage.EndInit();
            return bitImage;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.msgCodeimg.Source = GetMsgCodeImg2();
            this.msgCodemsgCodeimg.Source = GetMsgCodeImg2();
            RegGird.Visibility = Visibility.Collapsed;
            MsgCodeGrid.Visibility = Visibility.Collapsed;
            PWDGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());

            SendMsgReq sendMsgReq = new SendMsgReq()
            {
                Code = this.code.Text,
                Mobile = this.mobile.Text,
                Uuid = MsgImgUUId,
            };
            var  res=   client.SendMsgUsingPOSTAsync(sendMsgReq).GetAwaiter().GetResult();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            LoginReq req = new LoginReq()
            {
                Mobile= msgCodemobile.Text,
                MsgCode = msgCodemsgCode.Text,
            };
             var res    = client.LoginUsingPOSTAsync(req).GetAwaiter().GetResult();
            var temp = res.Data.ToString();
            var tokenJson = JsonConvert.DeserializeObject<Tokenjson>(temp);
            HttpClientHelper.Instance.setToken(tokenJson.Token);
            Init();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());

            SendMsgReq sendMsgReq = new SendMsgReq()
            {
                Code = this.msgCodeCode.Text,
                Mobile = this.msgCodemobile.Text,
                Uuid = MsgImgUUId,
            };
            var res = client.SendMsgUsingPOSTAsync(sendMsgReq).GetAwaiter().GetResult();

        }

        private void Reg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegGird.Visibility = Visibility.Visible;
            MsgCodeGrid.Visibility = Visibility.Collapsed;
            PWDGrid.Visibility = Visibility.Collapsed;
        }

        private void Login_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegGird.Visibility = Visibility.Collapsed;
            MsgCodeGrid.Visibility = Visibility.Collapsed;
            PWDGrid.Visibility = Visibility.Visible;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            LoginReq req = new LoginReq()
            {
                Mobile = pwsmobile.Text,
                Password = pwspassword.Text,
            };
            var res = client.LoginUsingPOSTAsync(req).GetAwaiter().GetResult();
            var temp = res.Data.ToString();
            var tokenJson = JsonConvert.DeserializeObject<Tokenjson>(temp);
            HttpClientHelper.Instance.setToken(tokenJson.Token);
            Init();
        }

        private void msgCodeModel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegGird.Visibility = Visibility.Collapsed;
            MsgCodeGrid.Visibility = Visibility.Visible;
            PWDGrid.Visibility = Visibility.Collapsed;
        }

        private void pwsMOdel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegGird.Visibility = Visibility.Collapsed;
            MsgCodeGrid.Visibility = Visibility.Collapsed;
            PWDGrid.Visibility = Visibility.Visible;
        }
        private bool init = false;
        private bool findScreen = false;
        private void Init()
        {
            this.PWDGrid.Visibility = Visibility.Collapsed;
            this.MsgCodeGrid.Visibility = Visibility.Collapsed;
            this.Loading.Visibility = Visibility.Visible;
            this.RegGird.Visibility = Visibility.Collapsed;
            Task.Run(() =>
            {
                init = true;
                //获取设备列表，
                JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                var devList = client.MacListUsingGETAsync().GetAwaiter().GetResult();
                if (devList.Code ==0)
                {
                    Messenger.Default.Send(new FindScreenEvent { DeviceInfos = devList.Data.ToList() });
                }
            
                // 获取本地屏幕
                findScreen = true;
                SerialPortHelper.Instance.AutoConnect();
                while (init||findScreen)
                {
                    Thread.Sleep(100);
                }

                this.Dispatcher.Invoke(() =>
                {
                    this.Visibility = Visibility.Collapsed;
                });
              
            });

         

        }
    }
}
