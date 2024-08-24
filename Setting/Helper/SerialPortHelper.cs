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
        private static bool Connected { get; set; }
        
        private static   string[] ComList { get; set; }
        private static int COMindex { get; set; }
        private static bool InitCOM(string PortName)
        {

            if (srialPort!=null)
            {
                srialPort.Close();
                srialPort.DataReceived-= new SerialDataReceivedEventHandler(serialPort_InitDataReceived);
            }
                srialPort = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.One);
                srialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_InitDataReceived);
                srialPort.ReceivedBytesThreshold = 1;
                srialPort.RtsEnable = false;
                srialPort.DtrEnable = true;
            
        
            return OpenPort();
        }

        private static DispatcherTimer timer;
       

        private static void Timer_Tick(object sender, EventArgs e)
        {
            COMindex++;
            if (COMindex>=ComList.Length)
            {
                ComList = SerialPort.GetPortNames();
                COMindex = 0;
            }
            InitCOM(ComList[COMindex]);
            SendgetMacSendMessage();
        }

        public void End()
        {
            timer?.Stop();
        }
        public static void AutoConnect()
        {
            ComList = SerialPort.GetPortNames();
            COMindex = -1;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000); // 设置计时器的时间间隔为1秒
            timer.Tick += Timer_Tick; ; // 订阅Tick事件
            timer.Start(); // 启动计时器


        }
        public static bool OpenPort()
        {
            try
            {
                if (!srialPort.IsOpen)
                {
                    srialPort.Open();
                }

            }
            catch (Exception ex)
            {
         
                return false;
            }

            return srialPort.IsOpen;
        }
        public static bool ClosePort()
        {
            try
            {
                srialPort.Close();
            }
            catch (Exception ex)
            {

                return false;
            }

            return !srialPort.IsOpen;
        }
        private static void serialPort_InitDataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                if (srialPort.IsOpen)
                {
                    var msg = srialPort.ReadLine();
                    var returnmsg = JsonConvert.DeserializeObject<ReturnBase<string>>(msg);
                    switch (returnmsg.cmd)
                    {
                        case "GetMac":
                        case "getMac":
                            
                                Connected = true;
                                timer.Stop();
                               

                                srialPort.DataReceived -= new SerialDataReceivedEventHandler(serialPort_InitDataReceived);
                              
                                srialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                               
                            
                            break;
                       
                        default:
                            break;
                    }

                }
            }
            catch (Exception)
            {
                srialPort.Close();

            }
            

        }

        private static void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (srialPort.IsOpen)
            {
                try
                {
                    var msg = srialPort.ReadLine();
                Messenger.Default.Send(new DebugInfoEvent("接收消息<==  " + msg));
             
                    var returnmsg = JsonConvert.DeserializeObject<ReturnBase<string>>(msg);
                    switch (returnmsg.cmd)
                    {
                        case "getMac":
                            if (returnmsg.cmd == currentCmd)
                            {
                                Connected = true;
                                timer.Stop();
                            }
                            break;
                        case "open":
                        case "close":
                        case "luminance":
                            ;
                            break;
                        case "theme":
                            if (returnmsg.cmd == currentCmd)
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
                            if (returnmsg.cmd == currentCmd)
                            {
                                var themeSendStartreturnmsg = JsonConvert.DeserializeObject<ThemeSendStartRetrun>(msg);
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
                            if (returnmsg.cmd == currentCmd)
                            {
                                index++;
                                if (index < msgList.Count)
                                {

                                    Write(msgList[index]);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                }
                catch (Exception)
                {


                }
            }
   
        }

        public static void SendgetMacSendMessage()
        {
           
            var cmd  = new getMacSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            if (srialPort.IsOpen)
            {
                srialPort.Write(msg + "**");
            }
        }

        private static void Write(string msg)
        {
            try
            {
                if (Connected)
            {

                Messenger.Default.Send(new DebugInfoEvent("发送消息==>  " + msg + "**"));
              

          
                srialPort.Write(msg + "**");
            }
            else
            {
                Messenger.Default.Send(new DebugInfoEvent("串口未连接成功"));

                }
            }
            catch (Exception ex)
            {

                Messenger.Default.Send(new DebugInfoEvent("发送消息==>  失败" + ex.ToString()));

            }
        }
        public static void SendLuminanceSendMessage(int arge)
        {
            var cmd  = new LuminanceSend(arge.ToString());
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public static void SendOpenSendMessage()
        {
            var cmd  = new OpenSend();
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
            var cmd  = new ThemeSend();
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
