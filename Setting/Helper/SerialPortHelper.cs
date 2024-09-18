using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Model.CMDModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Timer = System.Timers.Timer;

namespace Setting.Helper
{
    public class SerialPortHelper
    {

        private SerialDevice serialDevice { get; set; }



        public List<string> ScanDeviceComportsAsync()
        {
            string aqs = SerialDevice.GetDeviceSelector();
            var deviceCollection = DeviceInformation.FindAllAsync(aqs).GetAwaiter().GetResult();
            List<string> portNamesList = new List<string>();
            foreach (var item in deviceCollection)
            {
                var serialDevice = SerialDevice.FromIdAsync(item.Id).GetAwaiter().GetResult();
                if (serialDevice!=null)
                {
                    var portName = serialDevice.PortName;
                    portNamesList.Add(portName);
                    serialDevice.Dispose();
                }
             
            }
            return portNamesList;
        }


        private string CurrentComName { get; set; }
        private string currentCmd { get; set; }
        private int index { get; set; }
        private List<string> msgList { get; set; }
        private Timer timer;

      static readonly object _object = new object();

        private List<string> ComList { get; set; }
        private int COMindex { get; set; }
        Thread t;


        private SerialPortHelper()
        {
            // 设置WMI查询来监听串口设备的添加和移除事件

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Tick; ; // 订阅Tick事件
            timer.Interval = 3000; // 设置计时器的时间间隔为1秒
            timer.AutoReset = true;
            timer.Enabled = true;
            t = new Thread(TimerRead_Tick);
            t.Start();

        }
        private string ReadMsg { get; set; }
        private void TimerRead_Tick()
        {
            do
            {
                try
                {

                    if (serialDevice != null)
                    {
                        Messenger.Default.Send(new DebugInfoEvent($"线程————————————————"));
                        var count = serialDevice.BytesReceived;
                        var rBuffer = serialDevice.InputStream.ReadAsync(new Windows.Storage.Streams.Buffer(50), 50, InputStreamOptions.None).GetAwaiter().GetResult(); ;
                        string hexString = CryptographicBuffer.EncodeToHexString(rBuffer);

                        byte[] bytes = HexStringToByteArray(hexString);
                        string normalString = Encoding.UTF8.GetString(bytes);

                        ReadMsg += normalString;

                        ReadMsg = ReadMsg.Replace("\r", "");;

                        if (ReadMsg.Contains("\n"))
                        {
                           
                            var templist = ReadMsg.Split('\n');
                            for (int i = 0; i < templist.Length-1; i++)
                            {
                                SrialPort_DataReceived(templist[i]);
                            }
                            ReadMsg = templist[templist.Length - 1];
                        }
                    }

                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    

                }

            } while (true);
           
        }


        static byte[] HexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
                throw new ArgumentException(hexString);

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                bytes[i / 2] = System.Convert.ToByte(hexString.Substring(i, 2), 16);
            return bytes;
        }
    

    private static SerialPortHelper instance = null;
        public static SerialPortHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
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

        private bool InitCOM(string PortName)
        {
            try
            {
                if (serialDevice !=null)
                {
                    serialDevice.Dispose();
                }
                DeviceInformationCollection serialDeviceInfos =
                DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector(PortName)).GetAwaiter().GetResult(); ;

                try
                {
                    serialDevice = SerialDevice.FromIdAsync(serialDeviceInfos[0].Id).GetAwaiter().GetResult(); ;

                    if (serialDevice != null)
                    {
                        serialDevice.BaudRate = 115200;
                        serialDevice.Handshake = SerialHandshake.None;
                        serialDevice.Parity = SerialParity.None;
                        serialDevice.DataBits = 8;
                        serialDevice.StopBits = SerialStopBitCount.One;
                        serialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                        serialDevice.WriteTimeout = TimeSpan.FromSeconds(600);
                        serialDevice.IsRequestToSendEnabled = false;
                        serialDevice.IsDataTerminalReadyEnabled = true;
                       
                    }
                    SendgetMacSendMessage();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{PortName}成功"));
                return true;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{PortName}失败，失败原因：{ex.Message}"));
                return false;
            }
        }

        private void SrialPort_DataReceived(string msgreturn)
        {

            try
            {

            



                Messenger.Default.Send(new DebugInfoEvent("接收消息<==  " + serialDevice.PortName +"  "+ msgreturn));
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
                                CurrentComName = serialDevice.PortName;
                                Messenger.Default.Send(new DebugInfoEvent("串口扫描=>  " + serialDevice.PortName + "连接成功"));
                            }
                            else
                            {
                                Messenger.Default.Send(new DebugInfoEvent("串口扫描+==  " + serialDevice.PortName + "未返回mac地址未能自动连接"));
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
            catch (Exception )
            {
               
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            COMindex++;
            if (COMindex >= ComList.Count)
            {
                timer.Stop();
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描结束"));
                return;
            }
            if (ComList.Count == 0)
            {
                return;
            }
            InitCOM(ComList[COMindex]);
            //   SendgetMacSendMessage();
        }



        public void End()

        {
            timer.Elapsed -= Timer_Tick;
            timer?.Stop();
        }
        public void AutoConnect()
        {


            timer.Start(); // 启动计时器

            ComList = ScanDeviceComportsAsync();
            Messenger.Default.Send(new DebugInfoEvent($"串口扫描==>  发现{ComList.Count}个串口，串口列表：{String.Join(",", ComList)}"));
            COMindex = -1;
        }


        public bool ClosePort()
        {
            try
            {

                if (serialDevice != null)
                {
                    serialDevice.Dispose();
                }

            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }


        public void SendgetMacSendMessage()
        {

            var cmd = new getMacSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            if (serialDevice != null)
            {
                Write(msg);
            }
        }

        private void Write(string Sendmsg)
        {
            try
            {
                if (serialDevice != null)
                {

                    string str = Sendmsg + "**";



                    // send request cmd 
                    var sendDatas = Encoding.UTF8.GetBytes(str);
                    Messenger.Default.Send(new DebugInfoEvent($"发送消息==>  长度{sendDatas.Length}"));
                    var wBuffer = CryptographicBuffer.CreateFromByteArray(sendDatas);
                    var sw = serialDevice.OutputStream.WriteAsync(wBuffer).GetAwaiter().GetResult();


                    Messenger.Default.Send(new DebugInfoEvent("发送消息==>  " + serialDevice.PortName + Sendmsg + "**"));


                }
                else
                {
                    Messenger.Default.Send(new DebugInfoEvent("发送消息==>  失败串口关闭" + serialDevice.PortName + Sendmsg + "**"));
                }
            }
            catch (Exception ex)
            {

                Messenger.Default.Send(new DebugInfoEvent($"发送消息失败 ：{ex.ToString()}"));
            }
              
        
           


        }
        public void SendLuminanceSendMessage(int arge)
        {
            var cmd = new LuminanceSend(arge.ToString());
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public void SendOpenSendMessage()
        {
            var cmd = new OpenSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public void SendCloseSendMessage()
        {
            var cmd = new CloseSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public void SendThemeCirculateSendMessage(List<string> cmdlist)
        {
            var cmd = new ThemeSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
        public void SendThemeDynamicSendMessage()
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
        public void SendThemeSegmentSendMessage(List<string> cmdlist)
        {

            var cmd = new ThemeSegmentSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
    }
}
