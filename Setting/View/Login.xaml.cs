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
using System.Text.RegularExpressions;
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
using System.Windows.Threading;


namespace Setting.View
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : UserControl
    {
        private string MsgImgUUId;
        private DispatcherTimer RegTimer;
        private int RegTimerIndex;
        private DispatcherTimer MsgCodeRegTimer;
        private int MsgCodeRegTimerIndex;
        public Login()
        {
            InitializeComponent();

            Messenger.Default.Register<InitEndEvent>(this, HandleInitEndEvent);
            Messenger.Default.Register<EndChangeScreenEvent>(this, HandleEndChangeScreenEvent);
            this.PWDGrid.Visibility = Visibility.Visible;
            this.MsgCodeGrid.Visibility = Visibility.Collapsed;
            this.Loading.Visibility = Visibility.Collapsed;
            this.RegGird.Visibility = Visibility.Collapsed;

            RegTimer = new DispatcherTimer();
            RegTimer.Tick += RegTimer_Tick;
            RegTimer.Interval = new TimeSpan(0, 0, 1);
            MsgCodeRegTimer = new DispatcherTimer();
            MsgCodeRegTimer.Tick += MsgCodeRegTimer_Tick; ;
            MsgCodeRegTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void HandleEndChangeScreenEvent(EndChangeScreenEvent obj)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.LoadScreenTheme = false;
            });
        }

        private void MsgCodeRegTimer_Tick(object sender, EventArgs e)
        {
            MsgCodeRegTimerIndex--;
            if (MsgCodeRegTimerIndex < 0)
            {
                this.regTimeOutmsgCodGrip.Visibility = Visibility.Collapsed;
                MsgCodeRegTimer.Stop();
                return;
            }

            this.msgCodregTimeout.Content = MsgCodeRegTimerIndex;
        }

        private void HandleInitEndEvent(InitEndEvent obj)
        {
            if (obj.endName == "init")
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!mobileCheck(false))
            {
                return;
            }
            if (!passwordCheck(false))
            {
                return;
            }
            if (!codeCheck(false))
            {
                return;
            }
            if (!msgCodeCheck(false))
            {
                return;
            }

            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            RegisterReq req = new RegisterReq()
            {
                Mobile = this.mobile.Text,
                MsgCode = this.msgCode.Text,
                Password = this.password.Password,
                RePassword = this.Repassword.Password

            };
            var msg = client.RegisterUsingPOSTAsync(req).GetAwaiter().GetResult();
            if (msg.Code != 0)
            {
                this.msgCodeerror.Content = msg.Msg;
            }
            else
            {
                this.msgCodeerror.Content = "注册成功";
                RegGird.Visibility = Visibility.Collapsed;
                MsgCodeGrid.Visibility = Visibility.Collapsed;
                PWDGrid.Visibility = Visibility.Visible;

                this.pwsmobile.Text = this.mobile.Text;
                this.pwspassword.Password = this.password.Password;

                LoginReq LoginReq = new LoginReq()
                {
                    Mobile = pwsmobile.Text,
                    Password = pwspassword.Password,
                };
                var res = client.LoginUsingPOSTAsync(LoginReq).GetAwaiter().GetResult();
                if (res.Code != 0)
                {
                    this.pwspassworderror.Content = res.Msg;
                    return;
                }
                else
                {
                    this.pwspassworderror.Content = "";
                }
                var temp = res.Data.ToString();
                var tokenJson = JsonConvert.DeserializeObject<Tokenjson>(temp);
                HttpClientHelper.Instance.setToken(tokenJson.Token);
                Messenger.Default.Send(new AccountEvent() { Name = pwsmobile.Text });
                Init();
            }
        }

        private void msgCodeimg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.msgCodeimg.Source = GetMsgCodeImg2();
        }
        private void msgCodemsgCodeimg_MouseDown(object sender, MouseButtonEventArgs e)
        {

            this.msgCodemsgCodeimg.Source = GetMsgCodeImg2();
        }

        
        private BitmapImage GetMsgCodeImg2()
        {
            MsgImgUUId = Guid.NewGuid().ToString();
            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.UriSource = new Uri($"https://smart.9jodia.net/smart/api/dotpc/captcha?uuid={MsgImgUUId}", UriKind.Absolute);
            bitImage.EndInit();
            return bitImage;

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.msgCodeimg.Source == null)
            {
                RegGird.Visibility = Visibility.Collapsed;
                MsgCodeGrid.Visibility = Visibility.Collapsed;
                PWDGrid.Visibility = Visibility.Visible;
                var login = LoginInfoHelper.Open();
                this.pwsmobile.Text = login.UserName;
                this.pwspassword.Password = login.Password;
                if (login.isAutoLogin)
                {
                    JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                    LoginReq req = new LoginReq()
                    {
                        Mobile = pwsmobile.Text,
                        Password = pwspassword.Password,
                    };
                    var res = client.LoginUsingPOSTAsync(req).GetAwaiter().GetResult();
                    if (res.Code != 0)
                    {
                        this.pwspassworderror.Content = res.Msg;
                        return;
                    }
                    else
                    {
                        this.pwspassworderror.Content = "";
                    }
                    var temp = res.Data.ToString();
                    var tokenJson = JsonConvert.DeserializeObject<Tokenjson>(temp);
                    HttpClientHelper.Instance.setToken(tokenJson.Token);
                    Messenger.Default.Send(new AccountEvent() { Name = pwsmobile.Text });
                    Init();
                }



            }

        }

        private void RegsendMsg(object sender, RoutedEventArgs e)
        {
            if (!codeCheck(false))
            {
                return;
            }
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());

            SendMsgReq sendMsgReq = new SendMsgReq()
            {
                Code = this.code.Text,
                Mobile = this.mobile.Text,
                Uuid = MsgImgUUId,
            };
            var res = client.SendMsgUsingPOSTAsync(sendMsgReq).GetAwaiter().GetResult();
            if (res.Code != 0)
            {
                this.codeerror.Content = res.Msg;
                return;
            }
            else
            {
                this.codeerror.Content = "";
            }
            this.regTimeOutGrip.Visibility = Visibility.Visible;
            RegTimerIndex = 60;
            this.regTimeout.Content = RegTimerIndex;
            this.RegTimer.Start();
        }
        private void RegTimer_Tick(object sender, EventArgs e)
        {
            RegTimerIndex--;
            if (RegTimerIndex < 0)
            {
                this.regTimeOutGrip.Visibility = Visibility.Collapsed;
                RegTimer.Stop();
                return;
            }

            this.regTimeout.Content = RegTimerIndex;
        }
        private void msgCodeLogin(object sender, RoutedEventArgs e)
        {
            if (!msgCodemobileCheck(false))
            {
                return;

            }


            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            LoginReq req = new LoginReq()
            {
                Mobile = msgCodemobile.Text,
                MsgCode = msgCodeCode.Text,
            };
            var res = client.LoginUsingPOSTAsync(req).GetAwaiter().GetResult();
            if (res.Code != 0)
            {
                this.msgCodeCodeerror.Content = res.Msg;
                return;
            }
            else
            {
                this.msgCodeCodeerror.Content = "";
            }
            var temp = res.Data.ToString();
            var tokenJson = JsonConvert.DeserializeObject<Tokenjson>(temp);
            HttpClientHelper.Instance.setToken(tokenJson.Token);
            Messenger.Default.Send(new AccountEvent() { Name = msgCodemobile.Text });
            Init();
        }

        private void msgCodRegsendMsg(object sender, RoutedEventArgs e)
        {
            if (!msgCodemsgCodeCheck(false))
            {
                return;
            }

            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());

            SendMsgReq sendMsgReq = new SendMsgReq()
            {
                Code = this.msgCodemsgCode.Text,
                Mobile = this.msgCodemobile.Text,
                Uuid = MsgImgUUId,
            };
            var res = client.SendMsgUsingPOSTAsync(sendMsgReq).GetAwaiter().GetResult();
            if (res.Code != 0)
            {
                this.msgCodemsgCodeerror.Content = res.Msg;
                return;
            }
            else
            {
                this.msgCodemsgCodeerror.Content = "";
            }
            this.regTimeOutmsgCodGrip.Visibility = Visibility.Visible;
            MsgCodeRegTimerIndex = 60;
            this.msgCodregTimeout.Content = MsgCodeRegTimerIndex;
            this.MsgCodeRegTimer.Start();

        }

        private void Reg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.msgCodeimg.Source = GetMsgCodeImg2();
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
            if (!pwspasswordCheck(false))
            {
                return;
            }
            if (!pwsmobileCheck(false))
            {
                return;
            }

            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            LoginReq req = new LoginReq()
            {
                Mobile = pwsmobile.Text,
                Password = pwspassword.Password,
            };
            var res = client.LoginUsingPOSTAsync(req).GetAwaiter().GetResult();
            if (res.Code != 0)
            {
                this.pwspassworderror.Content = res.Msg;
                return;
            }
            else
            {
                this.pwspassworderror.Content = "";
            }
            var temp = res.Data.ToString();
            var tokenJson = JsonConvert.DeserializeObject<Tokenjson>(temp);
            HttpClientHelper.Instance.setToken(tokenJson.Token);
            Messenger.Default.Send(new AccountEvent() { Name = pwsmobile.Text });
            Init();
        }

        private void msgCodeModel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.msgCodemsgCodeimg.Source = GetMsgCodeImg2();
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

        private bool LoadScreenTheme = false;
        private void Init()
        {
            LoginInfoHelper.Save(new LoginModel() { Password = this.pwspassword.Password, UserName = this.pwsmobile.Text, isAutoLogin = true });
            this.Visibility = Visibility.Hidden;
            this.PWDGrid.Visibility = Visibility.Collapsed;
            this.MsgCodeGrid.Visibility = Visibility.Collapsed;
            this.Loading.Visibility = Visibility.Visible;
            this.RegGird.Visibility = Visibility.Collapsed;
            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.LoadingInfo.Children.Add(new LoadingInfoItem("1、屏幕信息获取"));

                });


                //获取设备列表，
                JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                var devList = client.MacListUsingGETAsync().GetAwaiter().GetResult();
                if (devList.Code == 0)
                {
                    Messenger.Default.Send(new FindScreenEvent { DeviceInfos = devList.Data.ToList() });
                }

                var temp =  SerialPortScanHelper.Instance;
           
                this.Dispatcher.Invoke(() =>
                {
                    this.LoadingInfo.Children.Add(new LoadingInfoItem("2、屏幕资源获取"));

                });

                LoadScreenTheme = true;
                Messenger.Default.Send(new CanChangeScreenEvent { });

                while (LoadScreenTheme)
                {
                    Thread.Sleep(100);
                }

                this.Dispatcher.Invoke(() =>
                {
                    this.LoadingInfo.Children.Add(new LoadingInfoItem("3、电脑硬件信息读取"));

                });


                this.Dispatcher.Invoke(() =>
                {
                    this.Visibility = Visibility.Collapsed;
                });
            });
        }

        private void mobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            mobileCheck();
        }

        private bool mobileCheck(bool canNull = true)
        {

            string phoneNumber = mobile.Text;

            if (phoneNumber.Length == 0 && !canNull)
            {
                this.mobileerror.Content = "手机号不能为空";
                return false;
            }
            if (phoneNumber.Length == 0 && canNull)
            {
                return true;
            }
            string pattern = @"^1[3-9]\d{9}$";
            Regex regex = new Regex(pattern);
            bool isMatch = regex.IsMatch(phoneNumber);

            if (isMatch)
            {
                if (this.mobileerror != null)
                {
                    this.mobileerror.Content = "";
                }

                return true;
            }
            else
            {
                this.mobileerror.Content = "手机号码格式不正确";
                return false;
            }
        }

        private bool passwordCheck(bool canNull = true)
        {
            string password = this.password.Password;
            string repassword = this.Repassword.Password;
            if (password.Length == 0 && !canNull)
            {
                this.passworderror.Content = "密码不能为空";
                return false;
            }
            if (repassword.Length == 0 && !canNull)
            {
                this.Repassworderror.Content = "密码不能为空";
                return false;
            }
            if (password.Length < 8 && password.Length > 0)
            {
                this.passworderror.Content = "密码长度小于8位";
                return false;
            }
            if (repassword.Length < 8 && repassword.Length > 0)
            {
                this.Repassworderror.Content = "密码长度小于8位";
                return false;
            }
            if (password.Length >= 8 && repassword.Length >= 8 && password != repassword)
            {
                this.passworderror.Content = "两次密码不一致";
                this.Repassworderror.Content = "两次密码不一致";
                return false;
            }
            this.passworderror.Content = "";
            this.Repassworderror.Content = "";
            return true;
        }

        private bool codeCheck(bool canNull = true)
        {
            string code = this.code.Text;

            if (code.Length == 0 && !canNull)
            {
                this.codeerror.Content = "验证码不能为空";
                return false;
            }
            this.codeerror.Content = "";
            return true;
        }
        private bool msgCodeCheck(bool canNull = true)
        {
            string msgCode = this.msgCode.Text;

            if (msgCode.Length == 0 && !canNull)
            {
                this.msgCodeerror.Content = "验证码不能为空";
                return false;
            }
            this.msgCodeerror.Content = "";
            return true;
        }
        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.passwordTextBox.Text != this.password.Password)
            {
                this.passwordTextBox.Text = this.password.Password;
            }

            passwordCheck();
        }
        private void passwordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.passwordTextBox.Text != this.password.Password)
            {
                this.password.Password = this.passwordTextBox.Text;
            }
            passwordCheck();
        }

        private void Repassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.RepasswordTextBox.Text != this.Repassword.Password)
            {
                this.RepasswordTextBox.Text = this.Repassword.Password;
            }

            passwordCheck();
        }
        private void RepasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.RepasswordTextBox.Text != this.Repassword.Password)
            {
                this.Repassword.Password = this.RepasswordTextBox.Text;
            }
            passwordCheck();
        }

        private void passwordChart_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush brush = new ImageBrush();

            if (this.passwordTextBox.Visibility == Visibility.Visible)
            {
                this.passwordTextBox.Visibility = Visibility.Collapsed;
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ZZJDMD;component/img/预览-打开_preview-open.png"));
            }
            else
            {
                this.passwordTextBox.Visibility = Visibility.Visible;
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ZZJDMD;component/img/预览-关闭_preview-close.png"));
            }
            this.passwordChart.Background = brush;
        }

        private void RepasswordChart_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush brush = new ImageBrush();

            if (this.RepasswordTextBox.Visibility == Visibility.Visible)
            {
                this.RepasswordTextBox.Visibility = Visibility.Collapsed;
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ZZJDMD;component/img/预览-打开_preview-open.png"));
            }
            else
            {
                this.RepasswordTextBox.Visibility = Visibility.Visible;
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ZZJDMD;component/img/预览-关闭_preview-close.png"));
            }
            this.RepasswordChart.Background = brush;
        }



        private void msgCodemobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            msgCodemobileCheck();
        }
        private bool msgCodemobileCheck(bool canNull = true)
        {
            string phoneNumber = msgCodemobile.Text;

            if (phoneNumber.Length == 0 && !canNull)
            {
                this.msgCodemobileerror.Content = "手机号不能为空";
                return false;
            }
            if (phoneNumber.Length == 0 && canNull)
            {
                return true;
            }
            string pattern = @"^1[3-9]\d{9}$";
            Regex regex = new Regex(pattern);
            bool isMatch = regex.IsMatch(phoneNumber);

            if (isMatch)
            {
                if (this.msgCodemobileerror != null)
                {
                    this.msgCodemobileerror.Content = "";
                }

                return true;
            }
            else
            {
                this.msgCodemobileerror.Content = "手机号码格式不正确";
                return false;
            }
        }

        private void code_TextChanged(object sender, TextChangedEventArgs e)
        {
            codeCheck();
        }

        private void msgCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            msgCodeCheck();
        }

        private void msgCodemsgCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            msgCodemsgCodeCheck();
        }

        private void msgCodeCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            msgCodeCodeCheck();
        }
        private bool msgCodemsgCodeCheck(bool canNull = true)
        {
            string code = this.msgCodemsgCode.Text;

            if (code.Length == 0 && !canNull)
            {
                this.msgCodemsgCodeerror.Content = "验证码不能为空";
                return false;
            }
            this.msgCodemsgCodeerror.Content = "";
            return true;
        }
        private bool msgCodeCodeCheck(bool canNull = true)
        {
            string msgCode = this.msgCodeCode.Text;

            if (msgCode.Length == 0 && !canNull)
            {
                this.msgCodeCodeerror.Content = "验证码不能为空";
                return false;
            }
            this.msgCodeCodeerror.Content = "";
            return true;
        }

        private void pwspasswordChart_Click(object sender, RoutedEventArgs e)
        {
            ImageBrush brush = new ImageBrush();

            if (this.pwspasswordTextBox.Visibility == Visibility.Visible)
            {
                this.pwspasswordTextBox.Visibility = Visibility.Collapsed;
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ZZJDMD;component/img/预览-打开_preview-open.png"));
            }
            else
            {
                this.pwspasswordTextBox.Visibility = Visibility.Visible;
                brush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/ZZJDMD;component/img/预览-关闭_preview-close.png"));
            }
            this.pwspasswordChart.Background = brush;
        }

        private void pwsmobile_TextChanged(object sender, TextChangedEventArgs e)
        {
            pwsmobileCheck();
        }

        private bool pwsmobileCheck(bool canNull = true)
        {
            string phoneNumber = pwsmobile.Text;

            if (phoneNumber.Length == 0 && !canNull)
            {
                this.pwsmobileerror.Content = "手机号不能为空";
                return false;
            }
            if (phoneNumber.Length == 0 && canNull)
            {
                return true;
            }
            string pattern = @"^1[3-9]\d{9}$";
            Regex regex = new Regex(pattern);
            bool isMatch = regex.IsMatch(phoneNumber);

            if (isMatch)
            {
                if (this.pwsmobileerror != null)
                {
                    this.pwsmobileerror.Content = "";
                }

                return true;
            }
            else
            {
                this.pwsmobileerror.Content = "手机号码格式不正确";
                return false;
            }
        }

        private void pwspasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.pwspasswordTextBox.Text != this.pwspassword.Password)
            {
                this.pwspassword.Password = this.pwspasswordTextBox.Text;
            }

            passwordCheck();
        }

        private void pwspassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pwspasswordTextBox == null)
            {
                return;
            }

            if (this.pwspasswordTextBox.Text != this.pwspassword.Password)
            {
                this.pwspasswordTextBox.Text = this.pwspassword.Password;
            }

            pwspasswordCheck();
        }

        private bool pwspasswordCheck(bool canNull = true)
        {
            string password = this.pwspassword.Password;

            if (password.Length == 0 && !canNull)
            {
                this.pwspassworderror.Content = "密码不能为空";
                return false;
            }

            if (password.Length < 8 && password.Length > 0)
            {
                this.pwspassworderror.Content = "密码长度小于8位";
                return false;
            }

            this.pwspassworderror.Content = "";

            return true;
        }

        private void Guest_MouseDown(object sender, MouseButtonEventArgs e)
        {

                LoginInfoHelper.Save(new LoginModel() { Password = "", UserName = "", isAutoLogin = false });
                this.Visibility = Visibility.Hidden;
                this.PWDGrid.Visibility = Visibility.Collapsed;
                this.MsgCodeGrid.Visibility = Visibility.Collapsed;
                this.Loading.Visibility = Visibility.Visible;
                this.RegGird.Visibility = Visibility.Collapsed;
                Task.Run(() =>
                {
                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    this.LoadingInfo.Children.Add(new LoadingInfoItem("1、屏幕信息获取"));

                    //});


                    ////获取设备列表，
                    //JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                    //var devList = client.MacListUsingGETAsync().GetAwaiter().GetResult();
                    //if (devList.Code == 0)
                    //{
                    //    Messenger.Default.Send(new FindScreenEvent { DeviceInfos = devList.Data.ToList() });
                    //}

                    Messenger.Default.Send(new FindScreenEvent { DeviceInfos = new List<DeviceInfo>() });

                    var temp = SerialPortScanHelper.Instance;

                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    this.LoadingInfo.Children.Add(new LoadingInfoItem("2、屏幕资源获取"));

                    //});

                    LoadScreenTheme = true;
                    Messenger.Default.Send(new CanChangeScreenEvent { });

                    while (LoadScreenTheme)
                    {
                        Thread.Sleep(100);
                    }

                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    this.LoadingInfo.Children.Add(new LoadingInfoItem("3、电脑硬件信息读取"));

                    //});


                    this.Dispatcher.Invoke(() =>
                    {
                        this.Visibility = Visibility.Collapsed;
                    });
                });
            
        }
    }
}
