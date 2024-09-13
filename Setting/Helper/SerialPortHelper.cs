using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Model.CMDModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;


namespace Setting.Helper
{
    public class SerialPortHelper
    {
        private ManagementEventWatcher _insertWatcher;
        private ManagementEventWatcher _removeWatcher;
        private SerialPort srialPort { get; set; }
        private string CurrentComName { get; set; }
        private string currentCmd { get; set; }
        private int index { get; set; }
        private List<string> msgList { get; set; }
        private DispatcherTimer timer;
        private string[] ComList { get; set; }
        private int COMindex { get; set; }
        //加锁因子
        private readonly static object obj3 = new object();


        private SerialPortHelper()
        {
            // 设置WMI查询来监听串口设备的添加和移除事件
            _insertWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2"));
            _removeWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3"));
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick; ; // 订阅Tick事件
            timer.Interval = TimeSpan.FromMilliseconds(3000); // 设置计时器的时间间隔为1秒

            _insertWatcher.EventArrived += _insertWatcher_EventArrived; ;
            _removeWatcher.EventArrived += _removeWatcher_EventArrived;
            // 开始监听事件
            _insertWatcher.Start();
            _removeWatcher.Start();
        }

        private void _removeWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
           
            if (string.IsNullOrEmpty(CurrentComName))
            {
                //没有连接上不处理
                return;
            }
            var comlist = SerialPort.GetPortNames();
            string[] ports = SerialPort.GetPortNames();
            if (comlist.All(c=>c!= CurrentComName))
            {//连接的串口被拔出
                Messenger.Default.Send(new DebugInfoEvent($"串口被拔出==> {CurrentComName}被拔出了。"));
                CurrentComName = "";
            }
        }

        private void _insertWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (string.IsNullOrEmpty(CurrentComName))
            {
                timer.Stop();
                AutoConnect();
            }
        }

    
        private static SerialPortHelper instance = null;
        public static SerialPortHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj3)
                    {
                        if (instance == null)
                        {

                            instance = new SerialPortHelper();
                        }
                    }
                }
                return instance;
            }
        }

        private  bool InitCOM(string PortName)
        {
            try
            {

                if (srialPort != null)
                {
                    srialPort.DataReceived -= SrialPort_DataReceived;
                    srialPort.Close();
                }
                srialPort = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.One);
                srialPort.ReceivedBytesThreshold = 1;
                srialPort.RtsEnable = false;
                srialPort.DtrEnable = true;
                srialPort.DataReceived += SrialPort_DataReceived;
                srialPort.Open();
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{PortName}成功"));
                return true;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{PortName}失败，失败原因：{ex.Message}"));
                return false;
            }
        }

        private  void SrialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
            try
            {

                SerialPort port = (SerialPort)sender;



              var  msgreturn =   port.ReadLine();

                 Messenger.Default.Send(new DebugInfoEvent("接收消息<==  " + port.PortName + msgreturn));
                var msgobject = JsonConvert.DeserializeObject<ReturnBase<string>>(msgreturn);
                switch (msgobject.cmd)
                {
                    case "GetMac":
                    case "getMac":
                        if (msgobject.cmd == "getMac" || msgobject.cmd == "GetMac")
                        {
                         
                                var getMacmsg = JsonConvert.DeserializeObject<getMacRetrun>(msgreturn);
                                if (!string.IsNullOrEmpty(getMacmsg.data))
                                {
                                    timer.Stop();
                                CurrentComName = port.PortName;
                                    Messenger.Default.Send(new DebugInfoEvent("串口扫描=>  " + port.PortName + "连接成功"));
                                }
                                else
                                {
                                    Messenger.Default.Send(new DebugInfoEvent("串口扫描+==  " + port.PortName + "未返回mac地址未能自动连接"));
                                }
                            

                        }

                        break;
                    case "open":
                    case "close":
                    case "luminance":
                        ;
                        break;
                    case "theme":
                        if (msgobject.cmd == currentCmd)
                        {
                            index++;
                            if (index < msgList.Count)
                            {
                                currentCmd = "themeSendStart";

                                Write(msgList[index]);
                            }

                        }
                        break;
                    case "themeSendStart":
                        if (msgobject.cmd == currentCmd)
                        {
                            var themeSendStartreturnmsg = JsonConvert.DeserializeObject<ThemeSendStartRetrun>(msgreturn);
                            if (!themeSendStartreturnmsg.HasDetail())
                            {
                                index++;
                                if (index < msgList.Count)
                                {
                                    currentCmd = "themeSegment";

                                    Write(msgList[index]);
                                }
                            }

                        }
                        break;
                    case "themeSegment":
                        if (msgobject.cmd == currentCmd)
                        {
                            index++;
                            if (index < msgList.Count)
                            {

                                Write(msgList[index]);
                            }
                        }
                        break;
                    default:
                        Messenger.Default.Send(new DebugInfoEvent("串口未连接成功"));
                        break;
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private  void Timer_Tick(object sender, EventArgs e)
        {
          
            COMindex++;
            if (COMindex >= ComList.Length)
            {
                timer.Stop();
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描结束"));
                return;
            }
            if (ComList.Length == 0)
            {
                return;
            }
            InitCOM(ComList[COMindex]);
         //   SendgetMacSendMessage();
        }

      

        public void End()

        {
            timer.Tick -= Timer_Tick;
            timer?.Stop();
        }
        public  void AutoConnect()
        {
          

            timer.Start(); // 启动计时器

            ComList = SerialPort.GetPortNames();
            Messenger.Default.Send(new DebugInfoEvent($"串口扫描==>  发现{ComList.Length}个串口，串口列表：{String.Join(",", ComList)}"));
            COMindex = -1;
        }


        public  bool ClosePort()
        {
            try
            {

                if (srialPort != null)
                {
                    srialPort.DataReceived -= SrialPort_DataReceived; 

                    srialPort.Close();
                }
                srialPort = new SerialPort();
                srialPort.ReceivedBytesThreshold = 1;
                srialPort.RtsEnable = false;
                srialPort.DtrEnable = false;


            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                srialPort.Dispose();
            }
            return true;
        }


        public  void SendgetMacSendMessage()
        {

            var cmd = new getMacSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            if (srialPort.IsOpen)
            {
                Write(msg );
            }
        }

        private  void Write(string Sendmsg, bool isHeart=false)
        {
            if (srialPort.IsOpen)
            {
                srialPort.Write(Sendmsg + "**");
               
                
                    Messenger.Default.Send(new DebugInfoEvent("发送消息==>  " + srialPort.PortName + Sendmsg + "**"));
                
            }
            else
            {
                Messenger.Default.Send(new DebugInfoEvent("发送消息==>  失败串口关闭"+  srialPort.PortName + Sendmsg + "**"));
            }

        
        }
        public  void SendLuminanceSendMessage(int arge)
        {
            var cmd = new LuminanceSend(arge.ToString());
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public  void SendOpenSendMessage()
        {
            var cmd = new OpenSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public  void SendCloseSendMessage()
        {
            var cmd = new CloseSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public  void SendThemeCirculateSendMessage(List<string> cmdlist)
        {
            var cmd = new ThemeSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
        public  void SendThemeDynamicSendMessage()
        {
            var cmd = new ThemeSend();
            currentCmd = cmd.cmd;
            cmd.data = new ThemeData()
            {
                model = "dynamic",
                name = ""
            };
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public  void SendThemeSegmentSendMessage(List<string> cmdlist)
        {
        
            var cmd = new ThemeSegmentSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
    }
}
