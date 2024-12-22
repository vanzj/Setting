
using System;
using System.Net;
using System.Text;
using System.Threading;
using Setting.Model.TCP;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using Timer = System.Timers.Timer;


namespace Setting.Helper
{
    public class TcpHelper
    {
        static EasyClient<StringPackageInfo> client = null;
        static Timer timer = null;
        private int port = 1010;

        static readonly object _object = new object();


        private TcpHelper()
        {
            byte[] beginMark = Encoding.Default.GetBytes("!");
            byte[] endMark = Encoding.Default.GetBytes("**");
            client = new EasyClient<StringPackageInfo>();
            client.Initialize(new ReceiveFilter(beginMark, endMark));
            client.Connected += OnClientConnected;
            client.NewPackageReceived += Client_NewPackageReceived;
            client.Error += OnClientError;
            client.Closed += OnClientClosed;
            client.ConnectAsync(new IPEndPoint(IPAddress.Parse("119.3.5.23"), port)).GetAwaiter().GetResult();
            //每2s发送一次心跳或尝试一次重连  
            timer = new Timer(2000);
            timer.Elapsed += Timer_Elapsed;
       
            timer.Enabled = true;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //心跳包
            if (client.IsConnected)
            {
                var heartMsg = CommandBuilder.BuildHeartCmd();
                client.Send(heartMsg);
            }
            //断线重连
            else if (!client.IsConnected)
            {
                client.ConnectAsync(new IPEndPoint(IPAddress.Parse("119.3.5.23"), port)).GetAwaiter().GetResult() ;

            }
        }

        private void OnClientError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {

         var temp =   e.Exception.Message;
            ;
        }

        private void Client_NewPackageReceived(object sender, PackageEventArgs<StringPackageInfo> e)
        {
            string msg = e.Package.Body;
            Console.WriteLine(msg.Trim('\0'));
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            ;
        }
        private void OnClientClosed(object sender, EventArgs e)
        {
           
            ;
        }
   
    
        private static TcpHelper instance = null;
        public static TcpHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new TcpHelper();
                        }
                    }
                }
                return instance;
            }
        }

    }

   

}
