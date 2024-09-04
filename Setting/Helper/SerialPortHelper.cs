using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Model.CMDModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Setting.Helper
{
    public class SerialPortHelper
    {
        private static SerialPort srialPort { get; set; }

        private static string currentCmd { get; set; }
        private static int index { get; set; }
        private static List<string> msgList { get; set; }
        private static DispatcherTimer timer;
       //private static DispatcherTimer Hearttimer;
        private static string[] ComList { get; set; }
        private static int COMindex { get; set; }
        private static bool InitCOM(string PortName)
        {
            try
            {

                if (srialPort != null)
                {
                    srialPort.ErrorReceived -= SrialPort_ErrorReceived;
                    srialPort.PinChanged -= SrialPort_PinChanged;
        
                    srialPort.Close();

                }
                srialPort = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.One);
                srialPort.ReceivedBytesThreshold = 1;
                srialPort.RtsEnable = false;
                srialPort.DtrEnable = true;
                srialPort.ErrorReceived += SrialPort_ErrorReceived;
                srialPort.PinChanged += SrialPort_PinChanged;
                srialPort.ReadTimeout = 1000;
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

        

        private static void SrialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            Messenger.Default.Send(new DebugInfoEvent($"串口发生变化==> ：{e.EventType}"));
        }

        private static void SrialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Messenger.Default.Send(new DebugInfoEvent($"串口异常==> {e.EventType}"));
            if (!timer.IsEnabled)
            {
                timer.Start();
            }
        }

        


        private static void Timer_Tick(object sender, EventArgs e)
        {
          
            COMindex++;
            if (COMindex >= ComList.Length)
            {
                ComList = SerialPort.GetPortNames();

                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==>  发现{ComList.Length}个串口，串口列表：{String.Join(",", ComList)}"));
                COMindex = 0;

            }
            if (ComList.Length == 0)
            {
                return;
            }
            InitCOM(ComList[COMindex]);
            SendgetMacSendMessage();
        }

      public static void HeartStart()
        {
            //Hearttimer = new DispatcherTimer();
            //Hearttimer.Tick += Hearttimer_Tick;  // 订阅Tick事件
            //Hearttimer.Interval = TimeSpan.FromMilliseconds(3000); // 设置计时器的时间间隔为1秒
            //Hearttimer.Start(); // 启动计时器
            Messenger.Default.Send(new DebugInfoEvent($"心跳检测==>  开始"));
        }

        private static void Hearttimer_Tick(object sender, EventArgs e)
        {
            //if (currentCmd == "Heart")
            //{
            //    var cmd = new getMacSend();
            //    msgList = new List<string>();
            //    string msg = JsonConvert.SerializeObject(cmd);
            //    if (srialPort.IsOpen)
            //    {
            //        Write(msg, true);
            //    }
            //    else
            //    {
            //        HeartEnd();
            //        AutoConnect();
            //    }
            //}
            
        }

        public static void HeartEnd()
        {
            //Hearttimer.Start(); // 启动计时器
           // Messenger.Default.Send(new DebugInfoEvent($"心跳检测==>  结束"));
           // Hearttimer.Tick -= Hearttimer_Tick;
            //Hearttimer?.Stop();
        }

        public void End()

        {
            timer.Tick -= Timer_Tick;
            timer?.Stop();
        }
        public static void AutoConnect()
        {
          
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick; ; // 订阅Tick事件
            timer.Interval = TimeSpan.FromMilliseconds(3000); // 设置计时器的时间间隔为1秒
        
            timer.Start(); // 启动计时器

            ComList = SerialPort.GetPortNames();
            Messenger.Default.Send(new DebugInfoEvent($"串口扫描==>  发现{ComList.Length}个串口，串口列表：{String.Join(",", ComList)}"));
            COMindex = -1;
        }


        public static bool ClosePort()
        {
            try
            {
             
                srialPort?.Close();
                srialPort.Dispose();
                
            
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }


        public static void SendgetMacSendMessage()
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

        private static void Write(string Sendmsg, bool isHeart=false)
        {
            var msgreturn = "";
            try
            {


                if (isHeart)
                {
                    Messenger.Default.Send(new DebugInfoEvent("心跳检测==>  " + srialPort.PortName + Sendmsg + "**"));

                }
                else
                {
                    Messenger.Default.Send(new DebugInfoEvent("发送消息==>  " + srialPort.PortName + Sendmsg + "**"));

                }



                srialPort.Write(Sendmsg + "**");

                 msgreturn = srialPort.ReadLine();
                if (isHeart)
                {
                    Messenger.Default.Send(new DebugInfoEvent("心跳检测<==  " + srialPort.PortName + msgreturn));

                }
                else
                {
                    Messenger.Default.Send(new DebugInfoEvent("接收消息<==  " + srialPort.PortName + msgreturn));

                }

                var msgobject = JsonConvert.DeserializeObject<ReturnBase<string>>(msgreturn);
                switch (msgobject.cmd)
                {
                    case "GetMac":
                    case "getMac":  
                        if (msgobject.cmd == "getMac" || msgobject.cmd== "GetMac")
                        {
                            if (!isHeart)
                            {
                                var getMacmsg = JsonConvert.DeserializeObject<getMacRetrun>(msgreturn);
                                if (!string.IsNullOrEmpty(getMacmsg.data))
                                {
                                      timer.Stop();
                                        Messenger.Default.Send(new DebugInfoEvent("串口扫描+==  " + srialPort.PortName + "连接成功"));
                                    //    HeartStart();
                                    //currentCmd = "Heart";
                                }
                                else
                                {
                                    Messenger.Default.Send(new DebugInfoEvent("串口扫描+==  " + srialPort.PortName + "未返回mac地址未能自动连接"));
                                }
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
                            else
                            {
                                //currentCmd = "Heart";
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
                //currentCmd = "Heart";
                Messenger.Default.Send(new DebugInfoEvent($"发送消息==>  失败 发送消息{Sendmsg},接受消息{msgreturn}" + ex.ToString()));

            }
        }
        public static void SendLuminanceSendMessage(int arge)
        {
            var cmd = new LuminanceSend(arge.ToString());
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public static void SendOpenSendMessage()
        {
            var cmd = new OpenSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public static void SendCloseSendMessage()
        {
            var cmd = new CloseSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public static void SendThemeCirculateSendMessage(List<string> cmdlist)
        {
            var cmd = new ThemeSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
        public static void SendThemeDynamicSendMessage()
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
        public static void SendThemeSegmentSendMessage(List<string> cmdlist)
        {
        
            var cmd = new ThemeSegmentSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
    }
}
