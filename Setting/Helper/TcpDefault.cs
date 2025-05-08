
using GalaSoft.MvvmLight.Messaging;
using GIfTool;
using Setting.Event;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Timer = System.Timers.Timer;


namespace Setting.Helper
{
    public class TcpDefault
    {
        string ip = "119.3.5.23";
        int port = 1010;
        public int brightness = 255;
        public string DevNo = "";
        public string pcmac = "";
        private string ReadMsg { get; set; }
        static Timer timer = null;
        Socket client;


        Thread t;

        public TcpDefault(string mac)
        {
            pcmac = "PC" + mac.Substring(2);
            DevNo = mac;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;
            IPAddress ipAdress = IPAddress.Parse(ip);
            //网络端点：为待请求连接的IP地址和端口号
            IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, port);
            //connect()向服务端发出连接请求。客户端不需要bind()绑定ip和端口号，
            //因为系统会自动生成一个随机的地址（具体应该为本机IP+随机端口号）
         

            //每2s发送一次心跳或尝试一次重连  
            timer = new Timer(1000*30);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
            t = new Thread(TimerRead);
            t.Start();
            try
            {
                if (!string.IsNullOrEmpty(pcmac))
                {
                    client.Connect(ipEndpoint);
                    var heartMsg = CommandBuilder.BuildHeartCmd(pcmac);
                    client.Send(heartMsg);
                }
            }
            catch (Exception ex)
            {
                ;
          
            }

        }

        public void DisConnect()
        {
            client?.Close();
            client?.Dispose();
        }
        private void TimerRead()
        {
            do
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024];
                    //接收服务端消息
                    int num = client.Receive(buffer);
                    string normalString = Encoding.UTF8.GetString(buffer, 0, num);
                    ReadMsg += normalString;

                    ReadMsg = ReadMsg.Replace("\r", "");
                    ReadMsg = ReadMsg.Replace("\n", "");
                    if (ReadMsg.Contains("**"))
                    {
                        string pattern = @"\*\*"; // 正则表达式中的两个星号需要用反斜杠转义
                        string[] result = Regex.Split(ReadMsg, pattern);
                        foreach (var item in result)
                        {
                            Console.WriteLine("'{0}'", item);
                        }
                        for (int i = 0; i < result.Length - 1; i++)
                        {
                            DataReceived(result[i]);
                        }
                        ReadMsg = result[result.Length - 1];
                    }
                }
                catch (Exception ex)
                {
                    ;
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            } while (true);
           
               

         
        }
        public  string HexToString(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = System.Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }
        private void DataReceived(string v)
        {
            
            if (v.StartsWith("?WTRANSPARENTDATA"))
            {
                try
                {
                    string pattern = @"#"; // 正则表达式中的两个星号需要用反斜杠转义
                    string[] result = Regex.Split(v, pattern);
                    if (result.Length == 3)
                    {
                        var hexString = result[2].Replace("**", "");
                        hexString = hexString.Replace("09010000", "");

                        string gifUrl = HexToString(hexString);
                        if (gifUrl.EndsWith(".gif"))
                        {

                            InputGIF inputGIF = new InputGIF();
                            int defaultFramerate = 6;
                            var ImgInfo = inputGIF.OPENGIFURL(gifUrl, 85, 5, "gifword", brightness, defaultFramerate);
                            Messenger.Default.Send(new SendStartEvent(this.DevNo));
                            var data = MessageHelper.BuildOnePackageGIFURL(ImgInfo, 85, 5, "gifword", defaultFramerate);
                            SerialPortSendMsgHelper.Instance.SendThemeCirculateSendMessage(data,DevNo);
                            var msg = CommandBuilder.BuildGIFSuccessCmd(pcmac);
                            client.Send(msg);
                            Messenger.Default.Send(new DebugInfoEvent($"TCP：==> gif下发成功:{v}"));
                        }
                    }
                }
                catch (Exception)
                {
                    Messenger.Default.Send(new DebugInfoEvent($"TCP：==> gif下发失败:{v}"));
                    var msg = CommandBuilder.BuildGIFFailCmd(pcmac);
                    client.Send(msg);
                }
               
            }
            Messenger.Default.Send(new DebugInfoEvent($"TCP：==> 心跳或其他:{v}"));
            return;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Heart();
            }
            catch (Exception ex)
            {

             
            } 
           
        }

        public void Heart()
        {
            //心跳包
            if (client!=null && client.Connected)
            {
                if (!string.IsNullOrEmpty(pcmac))
                {
                    var heartMsg = CommandBuilder.BuildHeartCmd(pcmac);
                   
                    client.Send(heartMsg);
                }

            }
            //断线重连
            else
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAdress = IPAddress.Parse(ip);
                //网络端点：为待请求连接的IP地址和端口号
                IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, port);
                //connect()向服务端发出连接请求。客户端不需要bind()绑定ip和端口号，
                //因为系统会自动生成一个随机的地址（具体应该为本机IP+随机端口号）
           
                client.Connect(ipEndpoint);
                if (!string.IsNullOrEmpty(pcmac))
                {
                    var heartMsg = CommandBuilder.BuildHeartCmd(pcmac);
                    client.Send(heartMsg);
                }

            }
        }
    

}

   

}
