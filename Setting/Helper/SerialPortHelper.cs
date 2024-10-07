using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Event;
using Setting.Model;
using Setting.Model.CMDModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Timer = System.Timers.Timer;

namespace Setting.Helper
{
    public class SerialPortHelper
    {
        private Dictionary<string, string> devNoComNo = new Dictionary<string, string>();
        private MYSerialDevice MYSerialDevice { get; set; }
        private Dictionary<DeviceWatcher, String> mapDeviceWatchersToDeviceSelector;

        public List<string> ScanDeviceComportsAsync()
        {
            string aqs = SerialDevice.GetDeviceSelector();
            var deviceCollection = DeviceInformation.FindAllAsync(aqs).GetAwaiter().GetResult();
            List<string> portNamesList = new List<string>();
            foreach (var item in deviceCollection)
            {
                portNamesList.Add(item.Id);
              
             
            }
            return portNamesList;
        }


        private string CurrentCOMID { get; set; }
        private string currentCmd { get; set; }
        private int index { get; set; }
        private List<string> msgList { get; set; }
        private Timer timer;

      static readonly object _object = new object();

        private List<string> DeviceIdList { get; set; }
        private int COMindex { get; set; }
        Thread t;


        private SerialPortHelper()
        {
           
            // 设置WMI查询来监听串口设备的添加和移除事件
            mapDeviceWatchersToDeviceSelector = new Dictionary<DeviceWatcher, String>();
            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Tick; ; // 订阅Tick事件
            timer.Interval = 1000; // 设置计时器的时间间隔为1秒
            t = new Thread(TimerRead_Tick);
            t.Start();
            InitializeDeviceWatchers();
            StartDeviceWatchers();
            Messenger.Default.Register<ScreenClickedEvent>(this, HandleScreenClickedEvent);
            Messenger.Default.Register<ScreenReConnactEvent>(this, HandleScreenReConnactEvent);
            
        }

        private void HandleScreenReConnactEvent(ScreenReConnactEvent obj)
        {
            CurrentCOMID = "";
            if (MYSerialDevice != null)
            {
                MYSerialDevice.COMID = obj.ComId;
                MYSerialDevice?.SerialDevice?.Dispose();
                MYSerialDevice.IsConnect = false;
            }
            if (!string.IsNullOrEmpty(obj.ComId))
            {
                MYSerialDevice.SerialDevice = SerialDevice.FromIdAsync(obj.ComId).GetAwaiter().GetResult();
                CurrentCOMID = obj.ComId;
                if (MYSerialDevice.SerialDevice != null)
                {
                    MYSerialDevice.SerialDevice.BaudRate = 115200;
                    MYSerialDevice.SerialDevice.Handshake = SerialHandshake.None;
                    MYSerialDevice.SerialDevice.Parity = SerialParity.None;
                    MYSerialDevice.SerialDevice.DataBits = 8;
                    MYSerialDevice.SerialDevice.StopBits = SerialStopBitCount.One;
                    MYSerialDevice.SerialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                    MYSerialDevice.SerialDevice.WriteTimeout = TimeSpan.FromSeconds(10);
                    MYSerialDevice.SerialDevice.IsRequestToSendEnabled = false;
                    MYSerialDevice.SerialDevice.IsDataTerminalReadyEnabled = true;
                    MYSerialDevice.IsConnect = true;
                }
            }
            Messenger.Default.Send(new DebugInfoEvent($"串口重连"));
        }

        private void HandleScreenClickedEvent(ScreenClickedEvent obj)
        {
            if (!obj.fromScreen)
            {
                CurrentCOMID = "";
                if (MYSerialDevice != null)
                {
                    MYSerialDevice.COMID = obj.ComId;
                    MYSerialDevice?.SerialDevice?.Dispose();
                    MYSerialDevice.IsConnect = false;
                }

                if (!string.IsNullOrEmpty(obj.ComId))
                {
                    Messenger.Default.Send(new DebugInfoEvent($"串口切换1"));
                    CurrentCOMID = obj.ComId;
                    if (!string.IsNullOrEmpty(obj.ComId))
                    {
                        MYSerialDevice.SerialDevice = SerialDevice.FromIdAsync(CurrentCOMID).GetAwaiter().GetResult();
                    }
                    Messenger.Default.Send(new DebugInfoEvent($"串口切换2"));
                    if (MYSerialDevice.SerialDevice != null)
                    {
                        MYSerialDevice.SerialDevice.BaudRate = 115200;
                        MYSerialDevice.SerialDevice.Handshake = SerialHandshake.None;
                        MYSerialDevice.SerialDevice.Parity = SerialParity.None;
                        MYSerialDevice.SerialDevice.DataBits = 8;
                        MYSerialDevice.SerialDevice.StopBits = SerialStopBitCount.One;
                        MYSerialDevice.SerialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                        MYSerialDevice.SerialDevice.WriteTimeout = TimeSpan.FromSeconds(10);
                        MYSerialDevice.SerialDevice.IsRequestToSendEnabled = false;
                        MYSerialDevice.SerialDevice.IsDataTerminalReadyEnabled = true;
                        MYSerialDevice.IsConnect = true;
                    }
                }
                Messenger.Default.Send(new DebugInfoEvent($"串口切换"));
            }
            
        }

        private void StartDeviceWatchers()
        {
            // Start all device watchers
            foreach (DeviceWatcher deviceWatcher in mapDeviceWatchersToDeviceSelector.Keys)
            {
                if ((deviceWatcher.Status != DeviceWatcherStatus.Started)
                    && (deviceWatcher.Status != DeviceWatcherStatus.EnumerationCompleted))
                {
                    deviceWatcher.Start();
                }
            }
        }
        private void InitializeDeviceWatchers()
        {

            // Target all Serial Devices present on the system
            var deviceSelector = SerialDevice.GetDeviceSelector();
            var deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);
            AddDeviceWatcher(deviceWatcher, deviceSelector);
        }
        private void AddDeviceWatcher(DeviceWatcher deviceWatcher, String deviceSelector)
        {
            deviceWatcher.Added += new TypedEventHandler<DeviceWatcher, DeviceInformation>(this.OnDeviceAdded);
            deviceWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(this.OnDeviceRemoved);
            mapDeviceWatchersToDeviceSelector.Add(deviceWatcher, deviceSelector);
        }

        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            if (devNoComNo.ContainsKey(args.Id))
            {
                Messenger.Default.Send(new LostScreenEvent { isLocal = true, DeviceInfos = new List<Webapi.DeviceInfo>() { new Webapi.DeviceInfo() { DevNo = devNoComNo[args.Id], BlueNo =args.Id } } }); ;

            }
            if (args.Id == CurrentCOMID)
            {
                MYSerialDevice.SerialDevice.Dispose();
                CurrentCOMID = "";
                MYSerialDevice.IsConnect = false;
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 连接断开"));
            }
        }

        private void OnDeviceAdded(DeviceWatcher sender, DeviceInformation args)
        {
            if (args.Id == CurrentCOMID)
            {
                MYSerialDevice.SerialDevice = SerialDevice.FromIdAsync(CurrentCOMID).GetAwaiter().GetResult();
                if (MYSerialDevice.SerialDevice !=null)
                {
                    MYSerialDevice.SerialDevice.BaudRate = 115200;
                    MYSerialDevice.SerialDevice.Handshake = SerialHandshake.None;
                    MYSerialDevice.SerialDevice.Parity = SerialParity.None;
                    MYSerialDevice.SerialDevice.DataBits = 8;
                    MYSerialDevice.SerialDevice.StopBits = SerialStopBitCount.One;
                    MYSerialDevice.SerialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                    MYSerialDevice.SerialDevice.WriteTimeout = TimeSpan.FromSeconds(10);
                    MYSerialDevice.SerialDevice.IsRequestToSendEnabled = false;
                    MYSerialDevice.SerialDevice.IsDataTerminalReadyEnabled = true;
                    MYSerialDevice.IsConnect = true;
                }
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 恢复连接"));
            }
            if (string.IsNullOrEmpty(CurrentCOMID))
            {
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 新串口自动扫描"));
                ReAutoConnect();
            }
        }
       

        private string ReadMsg { get; set; }
        private void TimerRead_Tick()
        {
            do
            {
                try
                {

                    if (MYSerialDevice!=null && MYSerialDevice.SerialDevice != null&MYSerialDevice.IsConnect)
                    {
                        var count = MYSerialDevice.SerialDevice.BytesReceived;
                        var rBuffer = MYSerialDevice.SerialDevice.InputStream.ReadAsync(new Windows.Storage.Streams.Buffer(50), 50, InputStreamOptions.None).GetAwaiter().GetResult(); ;
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

        private bool InitCOM(string COMID)
        {
            try
            {
                if (MYSerialDevice !=null&&MYSerialDevice.SerialDevice!=null)
                {
                    MYSerialDevice.SerialDevice.Dispose();
                    MYSerialDevice.IsConnect = false;
                }
                if (MYSerialDevice ==null)
                {
                    MYSerialDevice = new MYSerialDevice();
                }
                try
                {
                    MYSerialDevice.SerialDevice = SerialDevice.FromIdAsync(COMID).GetAwaiter().GetResult(); ;
                    MYSerialDevice.COMID = COMID;
                    if (MYSerialDevice != null)
                    {
                        MYSerialDevice.SerialDevice.BaudRate = 115200;
                        MYSerialDevice.SerialDevice.Handshake = SerialHandshake.None;
                        MYSerialDevice.SerialDevice.Parity = SerialParity.None;
                        MYSerialDevice.SerialDevice.DataBits = 8;
                        MYSerialDevice.SerialDevice.StopBits = SerialStopBitCount.One;
                        MYSerialDevice.SerialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                        MYSerialDevice.SerialDevice.WriteTimeout = TimeSpan.FromSeconds(10);
                        MYSerialDevice.SerialDevice.IsRequestToSendEnabled = false;
                        MYSerialDevice.SerialDevice.IsDataTerminalReadyEnabled = true;
                        MYSerialDevice.IsConnect = true;
                    }
                    SendgetMacSendMessage();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{COMID}成功"));
                return true;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new SendEndEvent());
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{COMID}失败，失败原因：{ex.Message}"));
                return false;
            }
        }

        private void SrialPort_DataReceived(string msgreturn)
        {

            try
            {

            



                Messenger.Default.Send(new DebugInfoEvent("接收消息<==  " + MYSerialDevice.SerialDevice.PortName +"  "+ msgreturn));
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
                                //timer.Stop();
                                //CurrentCOMID = MYSerialDevice.COMID;

                                var devNo = getMacmsg.data.Replace(":", "");

                                if (devNoComNo.ContainsKey(MYSerialDevice.COMID))
                                {
                                    devNoComNo[MYSerialDevice.COMID] = devNo;
                                }
                                else
                                {
                                    devNoComNo.Add(MYSerialDevice.COMID, devNo);
                                }

                                Messenger.Default.Send(new FindScreenEvent { isLocal = true ,DeviceInfos = new List<Webapi.DeviceInfo>() { new Webapi.DeviceInfo() { DevNo = devNo, Name = "新屏幕" ,BlueNo = MYSerialDevice.COMID} } }); ;
                                Messenger.Default.Send(new DebugInfoEvent("串口扫描=>  " + MYSerialDevice.SerialDevice.PortName + "连接成功"));
                            }
                            else
                            {
                                Messenger.Default.Send(new DebugInfoEvent("串口扫描+==  " + MYSerialDevice.SerialDevice.PortName + "未返回mac地址未能自动连接"));
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
                            if (index >= msgList.Count)
                            {
                                Messenger.Default.Send(new SendEndEvent());
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
            if (DeviceIdList ==null)
            {
                return;
            }
            COMindex++;
            if (COMindex >= DeviceIdList.Count)
            {
                timer.Stop();
                if (MYSerialDevice != null)
                {
                    MYSerialDevice.IsConnect = false;
                    MYSerialDevice.SerialDevice.Dispose();

                }

                Messenger.Default.Send(new CanChangeScreenEvent());
               
                    Messenger.Default.Send(new InitEndEvent() { endName = "findScreen" });
                

                Messenger.Default.Send(new DebugInfoEvent($"串口扫描结束"));
                return;
            }

            if (DeviceIdList.Count == 0)
            {
                return;
            }
            InitCOM(DeviceIdList[COMindex]);
       
        }



        public void End()

        {
            timer.Elapsed -= Timer_Tick;
            timer?.Stop();
        }
        private bool isReAutoConnect;
        public void AutoConnect()
        {

            isReAutoConnect = false;
            timer.Start(); // 启动计时器
            DeviceIdList = ScanDeviceComportsAsync();
            Messenger.Default.Send(new DebugInfoEvent($"串口扫描==>  发现{DeviceIdList.Count}个串口，串口列表：{String.Join(",", DeviceIdList)}"));
            COMindex = 0;
        }

        public void ReAutoConnect()
        {

            isReAutoConnect = true;
            timer.Start(); // 启动计时器
            DeviceIdList = ScanDeviceComportsAsync();
            Messenger.Default.Send(new DebugInfoEvent($"串口扫描==>  发现{DeviceIdList.Count}个串口，串口列表：{String.Join(",", DeviceIdList)}"));
            COMindex = 0;

        }
  

        public bool ClosePort()
        {
            try
            {

                if (MYSerialDevice != null)
                {
                    MYSerialDevice.SerialDevice?.Dispose();
                    MYSerialDevice.IsConnect = false;
                }
                t.Abort();

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
            index = -1;
            msgList = new List<string>();
            string msg = JsonConvert.SerializeObject(cmd);
            if (MYSerialDevice != null)
            {
                Write(msg);
            }
        }

        bool isSending = false;
        private void Write(string Sendmsg)
        {
            try
            {
                if (isSending == false)
                {
                    isSending = true;
                    if (MYSerialDevice != null)
                    {

                        string str = Sendmsg + "**";



                        // send request cmd 
                        var sendDatas = Encoding.UTF8.GetBytes(str);
                        Messenger.Default.Send(new DebugInfoEvent($"发送消息==>  长度{sendDatas.Length}"));
                        var wBuffer = CryptographicBuffer.CreateFromByteArray(sendDatas);

                        var sw = MYSerialDevice.SerialDevice.OutputStream.WriteAsync(wBuffer).GetAwaiter().GetResult();


                        Messenger.Default.Send(new DebugInfoEvent("发送消息==>  " + MYSerialDevice.SerialDevice.PortName + Sendmsg + "**"));


                    }
                    else
                    {
                        Messenger.Default.Send(new DebugInfoEvent("发送消息==>  失败串口关闭" + MYSerialDevice.SerialDevice.PortName + Sendmsg + "**"));
                    }
                }
                else
                {
                    Messenger.Default.Send(new DebugInfoEvent("发送消息==>  失败串口忙" + MYSerialDevice.SerialDevice.PortName + Sendmsg + "**"));
                }
              
            }
            catch (Exception ex)
            {

                Messenger.Default.Send(new DebugInfoEvent($"发送消息失败 ：{ex.ToString()}"));
            }
            finally
            {
                isSending = false;
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

        public void SendThemeDynamicSendMessage(List<string> cmdlist)
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
